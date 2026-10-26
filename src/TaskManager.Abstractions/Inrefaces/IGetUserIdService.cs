namespace TaskManager.Abstractions.Inrefaces;

public interface IGetUserIdService
{
    Guid GetUserId(string header);
}
