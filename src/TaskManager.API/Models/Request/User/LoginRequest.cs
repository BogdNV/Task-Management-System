using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models.Request;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(5)]
    public string Password { get; set; } = string.Empty;
}
