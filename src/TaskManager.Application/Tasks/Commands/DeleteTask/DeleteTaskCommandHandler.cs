using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DeleteTaskCommandResult>
{
    private readonly ApplicationDbContext _context;

    public DeleteTaskCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteTaskCommandResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.ProjectEntity!.UserId == request.UserId);

        if (task is null) throw new UnauthorizedAccessException("Не найдено или доступ запрещен");

        var id = task.Id;

        _context.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteTaskCommandResult
        {
            Id = id
        };
    }
}
