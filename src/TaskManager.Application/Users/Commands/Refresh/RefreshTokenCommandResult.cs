using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands.Refresh;

public class RefreshTokenCommandResult
{
    public required TokenDto Token { get; set; }
}
