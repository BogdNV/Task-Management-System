using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.UpdateTasks;

public record UpdateTaskStatusCommand(
    int TaskId,
    Status NewStatus,
    int CurrentUserId
);
