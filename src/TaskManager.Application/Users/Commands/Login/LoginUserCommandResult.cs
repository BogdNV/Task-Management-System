using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandResult : ITokenResult
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public required TokenDto Token { get; set; }

    public string AccessToken => Token.AccessToken;

    public string RefreshToken => Token.RefreshToken;
}
