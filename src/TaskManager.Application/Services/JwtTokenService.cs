using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Common;

namespace TaskManager.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtTokenOptions _options;
    public JwtTokenService(IOptions<JwtTokenOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateAccessToken(Guid id, string name)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim("id", id.ToString()),
            new Claim("userName", name)
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes),
            issuer: _options.Issuer,
            audience: _options.Audience,
            signingCredentials: creds,
            claims: claims

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
