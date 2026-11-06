using MediatR;

namespace TaskManager.Application.Users.Commands.Logout;

public class LogoutUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}
