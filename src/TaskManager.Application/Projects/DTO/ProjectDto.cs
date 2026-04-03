namespace TaskManager.Application.Projects.DTO;

public record ProjectDto(
    int Id,
    string Name,
    string Description,
    int OwnerId
);
