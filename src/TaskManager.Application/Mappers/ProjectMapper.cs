using TaskManager.Application.Projects.Commands.CreateProject;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappers;

public static partial class ProjectMapper
{
    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto(
            project.Id,
            project.Name,
            project.Description,
            project.OwnerId
        );
    }

    public static Project ToDomain(this CreateProjectCommand dto)
    {
        return new Project(dto.Name, dto.Description, dto.OwnerId);
    }
}
