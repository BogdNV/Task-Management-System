using MediatR;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands;

public class UpdateTaskCommand : IRequest<UpdateTaskCommandResult>
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
