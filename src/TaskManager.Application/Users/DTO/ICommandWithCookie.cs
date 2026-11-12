namespace TaskManager.Application.Users.DTO;

public interface ICommandWithCookie
{
    bool SaveToCookie { get; set; }
}
