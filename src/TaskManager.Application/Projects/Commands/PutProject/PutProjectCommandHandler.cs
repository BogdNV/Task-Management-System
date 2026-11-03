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

    public async Task<PutProjectCommandResult> Handle(PutProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == request.UserId);
        if (project is null) throw new NotFoundException("Проект не найден");

        project.Title = request.Title;
        project.Description = request.Description;
        project.LastModified = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PutProjectCommandResult>(project);
    }
}
