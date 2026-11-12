using MediatR;
using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommand : IRequest<LoginUserCommandResult>, ICommandWithCookie
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool SaveToCookie { get; set; }
}
