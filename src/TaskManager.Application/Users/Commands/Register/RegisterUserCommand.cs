using MediatR;

namespace TaskManager.Application.Users.Commands;

public class RegisterUserCommand : IRequest<RegisterUserCommandResult>
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
