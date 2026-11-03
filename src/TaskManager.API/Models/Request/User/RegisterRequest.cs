using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models.Request;

public class RegisterRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(5)]
    public string Password { get; set; } = string.Empty;
}
