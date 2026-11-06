using TaskManager.Application.Users.DTO;

namespace TaskManager.API.Models.Response;

public class RegisterResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required TokenDto Token { get; set; }
}
