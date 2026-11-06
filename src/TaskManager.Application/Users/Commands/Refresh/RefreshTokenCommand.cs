using MediatR;

namespace TaskManager.Application.Users.Commands.Refresh;

public class RefreshTokenCommand : IRequest<RefreshTokenCommandResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}
