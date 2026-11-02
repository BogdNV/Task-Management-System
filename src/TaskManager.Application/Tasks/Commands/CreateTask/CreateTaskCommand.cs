using MediatR;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands;

public class CreateTaskCommand : IRequest<CreateTaskCommandResult>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ProjectId { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid UserId { get; set; }
}
