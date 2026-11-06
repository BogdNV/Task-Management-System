namespace TaskManager.Infrastructure.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByToken { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => ReplacedByToken is null && RevokedAt is null && !IsExpired;

}