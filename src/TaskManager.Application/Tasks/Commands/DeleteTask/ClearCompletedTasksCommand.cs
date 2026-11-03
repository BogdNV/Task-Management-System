using MediatR;

namespace TaskManager.Application.Tasks.Commands;

public class ClearCompletedTasksCommand : IRequest<ClearCompletedTasksCommandResult>
{
    public Guid UserId { get; set; }
}
