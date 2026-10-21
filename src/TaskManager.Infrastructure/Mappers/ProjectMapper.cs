using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Mappers;

public static partial class ProjectMapper
{
    public static Project ToDomain(this ProjectEntity entity)
    {
        return new Project(entity.Id, entity.Name, entity.Description, entity.UserId, entity.CreatedAt);
    }

    public static ProjectEntity ToEntity(this Project project)
    {
        return new ProjectEntity
        {
            Name = project.Name,
            Description = project.Description,
            UserId = project.UserId,
            CreatedAt = project.CreatedAt
        };
    }


}
