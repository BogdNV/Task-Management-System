namespace TaskManager.Abstractions.Inrefaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid id);
}
