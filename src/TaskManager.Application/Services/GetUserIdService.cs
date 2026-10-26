using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager.Abstractions.Inrefaces;

namespace TaskManager.Application.Services;

public class GetUserIdService : IGetUserIdService
{
    public Guid GetUserId(string header)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = header.Replace("Bearer ", "");
        var claim = handler.ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(claim!.Value, out Guid userId);
        return userId;
    }
}
