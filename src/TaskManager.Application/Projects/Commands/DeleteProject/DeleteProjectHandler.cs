using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteProjectHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProjectCommand common, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == common.Id, cancellationToken);
        if (project is null) throw new NotFoundException("Проект не найден!");
        if (project.UserId != common.UserId) throw new ForbiddenException("Нет прав на удаление");
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
