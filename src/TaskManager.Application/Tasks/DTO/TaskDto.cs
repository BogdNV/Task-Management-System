using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.DTO;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }

}
