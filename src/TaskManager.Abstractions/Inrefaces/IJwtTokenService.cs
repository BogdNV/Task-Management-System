using TaskManager.Infrastructure.Entities;

namespace TaskManager.Abstractions.Inrefaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId);
    string GenerateRefreshToken();
    Task<RefreshToken?> GetRefreshToken(string token, CancellationToken ct);
    Task RevokeRefreshToken(string storedToken, CancellationToken ct, string? replacedByToken = null);
    Task SaveRefreshToken(Guid userId, string refreshToken, int daysToExpire, CancellationToken ct);
}
