using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class PutProjectHandler : IRequestHandler<PutProjectCommand>
{
    private readonly ApplicationDbContext _context;
    public PutProjectHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PutProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == command.Id);
        if (project is null) return;

        if (project.UserId != command.UserId) throw new UnauthorizedException("Unauthrized");


        project.Title = command.Title;
        project.Description = command.Description;
        project.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
