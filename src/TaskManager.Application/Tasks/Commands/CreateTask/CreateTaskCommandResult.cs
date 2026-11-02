using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands;

public class CreateTaskCommandResult
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    // public ProjectEntity? ProjectEntity { get; set; }
}
