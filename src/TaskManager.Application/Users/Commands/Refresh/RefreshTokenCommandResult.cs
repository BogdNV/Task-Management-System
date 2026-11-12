using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands.Refresh;

public class RefreshTokenCommandResult : ITokenResult
{
    public required TokenDto Token { get; set; }

    public string AccessToken => Token.AccessToken;

    public string RefreshToken => Token.RefreshToken;
}
