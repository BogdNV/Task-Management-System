using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Common;

namespace TaskManager.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IOptions<JwtTokenOptions> _options;
    public JwtTokenService(IOptions<JwtTokenOptions> options)
    {
        _options = options;
    }

    public string GenerateAccessToken(Guid id)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString())
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_options.Value.Expires),
            signingCredentials: creds,
            claims: claims

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
