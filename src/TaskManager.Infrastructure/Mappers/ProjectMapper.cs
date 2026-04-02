using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Mappers;

public static class ProjectMapper
{
    public static Project ToDomain(this ProjectEntity entity)
    {
        return new Project(entity.Id, entity.Name, entity.Description, entity.OwnerId, entity.CreatedAt);
    }

    public static ProjectEntity ToEntity(this Project project)
    {
        return new ProjectEntity
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedAt = project.CreatedAt
        };
    }
}
