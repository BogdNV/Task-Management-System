using MediatR;

namespace TaskManager.Application.Tasks.Commands;

public class DeleteTaskCommand : IRequest<DeleteTaskCommandResult>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}
