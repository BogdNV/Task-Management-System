namespace TaskManager.Application.Common;

public class JwtTokenOptions
{
    public string SecretKey { get; set; } = string.Empty;
    // public string Issuer { get; set; } = string.Empty;
    // public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public bool CookieSecure { get; set; } = true;
}
