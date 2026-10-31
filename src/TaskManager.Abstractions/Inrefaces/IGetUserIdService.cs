namespace TaskManager.Abstractions.Inrefaces;

public interface IGetUserIdService
{
    Guid Id { get; }
    bool IsAuthenticated { get; }
}
