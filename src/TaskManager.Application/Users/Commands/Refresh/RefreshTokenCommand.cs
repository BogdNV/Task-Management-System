using MediatR;
using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands.Refresh;

public class RefreshTokenCommand : IRequest<RefreshTokenCommandResult>, ICommandWithCookie
{
    public string? RefreshToken { get; set; }
    public bool SaveToCookie { get; set; } = true;
}
