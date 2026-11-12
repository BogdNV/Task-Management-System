using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Common;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly Lock _lock = new();
    private readonly ApplicationDbContext _context;
    private readonly IOptions<JwtTokenOptions> _options;
    private readonly IHttpContextAccessor _accessor;
    public JwtTokenService(IOptions<JwtTokenOptions> options, ApplicationDbContext context, IHttpContextAccessor accessor)
    {
        _options = options;
        _context = context;
        _accessor = accessor;
    }

    public string GenerateAccessToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(_options.Value.AccessTokenExpirationMinutes),
            signingCredentials: creds,
            claims: claims

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<RefreshToken?> GetRefreshToken(string token, CancellationToken ct)
    {
        var refreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(t => t.Token == token, ct);

        return refreshToken;
    }

    public async Task RevokeRefreshToken(RefreshToken storedToken, CancellationToken ct, string? replacedByToken = null)
    {
        lock (_lock)
        {
            if (storedToken.RevokedAt is null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(replacedByToken))
                    storedToken.ReplacedByToken = replacedByToken;
            }
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task<string> SaveRefreshToken(Guid userId, CancellationToken ct)
    {
        var storedToken = await _context.RefreshTokens
            .OrderByDescending(t => t.Created)
            .FirstOrDefaultAsync(t => t.UserId == userId, ct);


        var token = new RefreshToken
        {
            UserId = userId,
            Token = GenerateRefreshToken(),
            Created = DateTime.UtcNow
        };
        if (storedToken is not null)
        {
            token.Expires = storedToken.Expires;
            await RevokeRefreshToken(storedToken, ct, token.Token);
        }
        else
            token.Expires = token.Created.AddDays(_options.Value.RefreshTokenExpirationDays);
        _context.RefreshTokens.Add(token);

        await _context.SaveChangesAsync(ct);

        return token.Token;
    }

    public void SetTokenInsideCookie(string accessToken, string refreshToken)
    {
        // SameSite=None работает только вместе с Secure, а Secure отбрасывается
        // браузером на http. Для локального запуска без HTTPS пару переключаем
        // в Lax/не-secure через AuthSettings:CookieSecure.
        var secure = _options.Value.CookieSecure;
        var sameSite = secure ? SameSiteMode.None : SameSiteMode.Lax;

        _accessor.HttpContext?.Response.Cookies.Append("accessToken", accessToken,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(_options.Value.AccessTokenExpirationMinutes),
                HttpOnly = true,
                IsEssential = true,
                Secure = secure,
                SameSite = sameSite
            });

        _accessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(_options.Value.RefreshTokenExpirationDays),
                HttpOnly = true,
                IsEssential = true,
                Secure = secure,
                SameSite = sameSite
            });
    }
}