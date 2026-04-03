using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Mappers;

public static partial class ProjectMapper
{
    public static Project ToDomain(this ProjectEntity entity)
    {
        return new Project(entity.Id, entity.Name, entity.Description, entity.OwnerId, entity.CreatedAt);
    }

    public static ProjectEntity ToEntity(this Project project)
    {
        return new ProjectEntity
        {
            // Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedAt = project.CreatedAt
        };
    }

    public static TaskItem ToDomain(this TaskItemEntity entity)
    {
        return new TaskItem(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Status,
            entity.Priority,
            entity.ProjectId,
            entity.DueDate
        );
    }

    public static TaskItemEntity ToEntity(this TaskItem task)
    {
        return new TaskItemEntity
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId
        };
    }
}
