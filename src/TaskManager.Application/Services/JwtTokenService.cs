using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
    private readonly ApplicationDbContext _context;
    private readonly IOptions<JwtTokenOptions> _options;
    public JwtTokenService(IOptions<JwtTokenOptions> options, ApplicationDbContext context)
    {
        _options = options;
        _context = context;
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
            expires: DateTime.UtcNow.Add(_options.Value.Expires),
            signingCredentials: creds,
            claims: claims

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken?> GetRefreshToken(string token, CancellationToken ct)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token, ct);
        if (refreshToken is null || !refreshToken.IsActive)
            return null;
        return refreshToken;
    }

    public async Task RevokeRefreshToken(string storedToken, CancellationToken ct, string? replacedByToken = null)
    {
        var token = await GetRefreshToken(storedToken, ct);
        if (token is null)
            throw new UnauthorizedAccessException("Токен устарел или не найден");

        token.RevokedAt = DateTime.UtcNow;
        if (!string.IsNullOrEmpty(replacedByToken))
            token.ReplacedByToken = replacedByToken;


        await _context.SaveChangesAsync(ct);
    }

    public async Task SaveRefreshToken(Guid userId, string refreshToken, int daysToExpire, CancellationToken ct)
    {
        var token = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            Created = DateTime.UtcNow
        };

        var storedToken = await _context.RefreshTokens
            .OrderByDescending(t => t.Created)
            .LastOrDefaultAsync(t => t.UserId == userId);
        if (storedToken is not null)
            token.Expires = storedToken.Expires;
        else
            token.Expires = token.Created.AddDays(daysToExpire);
        _context.RefreshTokens.Add(token);

        await _context.SaveChangesAsync(ct);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}