namespace TaskManager.Application.Projects.DTO;

public record CreateProjectDto(
    string Name,
    string Description,
    int OwnerId
);
