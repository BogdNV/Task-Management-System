using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Context;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public class ClearCompletedTasksCommandHandler : IRequestHandler<ClearCompletedTasksCommand, ClearCompletedTasksCommandResult>
{
    private readonly ApplicationDbContext _context;

    public ClearCompletedTasksCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClearCompletedTasksCommandResult> Handle(ClearCompletedTasksCommand request, CancellationToken cancellationToken)
    {
        var countTasks = await _context.Tasks
            .Where(t => t.ProjectEntity!.UserId == request.UserId
                && t.Status == Status.Completed)
            .ExecuteDeleteAsync(cancellationToken);

        return new ClearCompletedTasksCommandResult { DeletedCount = countTasks };
    }
}
