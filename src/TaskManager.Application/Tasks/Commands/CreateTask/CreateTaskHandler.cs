using TaskManager.Application.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public class CreateTaskHandler
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateTaskHandler(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(CreateTaskCommand command, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(command.ProjectId);
        if (project is null || project.OwnerId != command.CurrentUserId)
            throw new ForbiddenException("Нет прав на создание задачи в этом проекте");

        var task = new TaskItem(command.Title, command.Description, command.ProjectId, command.DueDate);

        await _taskRepository.AddAsync(task);
        return task.Id;
    }
}
