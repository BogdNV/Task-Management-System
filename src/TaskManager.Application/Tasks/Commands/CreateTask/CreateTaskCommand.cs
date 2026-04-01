namespace TaskManager.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string Description,
    int ProjectId,
    DateTime? DueDate,
    int CurrentUserId
);
