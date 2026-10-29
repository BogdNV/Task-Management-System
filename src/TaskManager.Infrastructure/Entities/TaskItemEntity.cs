using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Entities;

public class TaskItemEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public ProjectEntity? ProjectEntity { get; set; }
}
