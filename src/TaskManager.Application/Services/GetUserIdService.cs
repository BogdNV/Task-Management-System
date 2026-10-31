using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManager.Abstractions.Inrefaces;

namespace TaskManager.Application.Services;

public class GetUserIdService : IGetUserIdService
{
    private readonly IHttpContextAccessor _accessor;

    public GetUserIdService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid Id
    {
        get
        {
            var userClaim = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userClaim, out Guid userId) ? userId : Guid.Empty;
        }
    }

    public bool IsAuthenticated => _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

}
