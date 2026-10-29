namespace TaskManager.Infrastructure.Entities;

public class ProjectEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public ICollection<TaskItemEntity> Tasks { get; set; } = new List<TaskItemEntity>();

    public DateTime? LastModified { get; set; }
}
