using TaskManager.Application.Exceptions;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Commands.UpdateTasks;

public class UpdateTaskStatusHandler
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateTaskStatusHandler(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(UpdateTaskStatusCommand command, CancellationToken ct)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId);
        if (task is null)
            throw new NotFoundException($"Задача c ID {command.TaskId} не найдена");

        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project is null || command.CurrentUserId != project.OwnerId)
            throw new ForbiddenException("Нет прав на изменения статуса в текущей задаче");

        task.ChangeStatus(command.NewStatus);

        await _taskRepository.UpdateAsync(task);

        return task.Id;
    }
}
