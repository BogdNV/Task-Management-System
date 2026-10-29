using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class PatchProjectHandler : IRequestHandler<PatchProjectCommand>
{
    private readonly ApplicationDbContext _context;

    public PatchProjectHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PatchProjectCommand common, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == common.Id, cancellationToken);
        if (project is null) throw new NotFoundException("not found");
        if (project.UserId != common.UserId) throw new ForbiddenException();


        if (string.IsNullOrEmpty(common.Title)
            && string.IsNullOrEmpty(common.Description))
            throw new Exception("пусто");

        if (!string.IsNullOrEmpty(common.Title)) project.Title = common.Title;
        if (!string.IsNullOrEmpty(common.Description)) project.Description = common.Description;
        project.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
