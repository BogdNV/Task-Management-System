using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class PatchProjectHandler : IRequestHandler<PatchProjectCommand, PatchProjectCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public PatchProjectHandler(ApplicationDbContext context, IMapper mapper, TimeProvider timeProvider)
    {
        _context = context;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }

    public async Task<PatchProjectCommandResult> Handle(PatchProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == request.UserId, cancellationToken);
        if (project is null) throw new NotFoundException("Проект не найден");

        if (request.Title is not null)
            project.Title = request.Title;
        if (request.Description is not null)
            project.Description = request.Description;
        project.LastModified = _timeProvider.GetUtcNow().UtcDateTime;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PatchProjectCommandResult>(project);
    }
}
