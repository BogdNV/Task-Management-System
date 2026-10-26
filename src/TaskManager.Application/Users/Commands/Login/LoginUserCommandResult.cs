namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandResult
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
