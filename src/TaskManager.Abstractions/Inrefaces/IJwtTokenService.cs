using TaskManager.Infrastructure.Entities;

namespace TaskManager.Abstractions.Inrefaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId);
    Task<RefreshToken?> GetRefreshToken(string token, CancellationToken ct);
    Task RevokeRefreshToken(RefreshToken storedToken, CancellationToken ct, string? replacedByToken = null);
    Task<string> SaveRefreshToken(Guid userId, CancellationToken ct);
    void SetTokenInsideCookie(string accessToken, string refreshToken);
}
