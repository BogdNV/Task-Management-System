namespace TaskManager.Application.Users.Commands;

public class RegisterUserCommandResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}
