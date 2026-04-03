namespace TaskManager.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    string Name,

    string Description,

    int OwnerId
);
