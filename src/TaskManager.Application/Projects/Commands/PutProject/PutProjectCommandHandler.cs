using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class PutProjectHandler : IRequestHandler<PutProjectCommand, PutProjectCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public PutProjectHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PutProjectCommandResult> Handle(PutProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == command.Id);
        if (project is null) throw new NotFoundException("Проект не найден");

        project.Title = command.Title;
        project.Description = command.Description;
        project.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PutProjectCommandResult>(project);
    }
}
