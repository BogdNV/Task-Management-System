namespace TaskManager.Application.Users.DTO;

public interface ITokenResult
{
    string AccessToken { get; }
    string RefreshToken { get; }
}
