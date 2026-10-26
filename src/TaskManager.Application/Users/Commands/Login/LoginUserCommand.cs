using MediatR;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommand : IRequest<LoginUserCommandResult>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
