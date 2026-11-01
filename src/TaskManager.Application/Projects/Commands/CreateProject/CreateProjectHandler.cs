using AutoMapper;
using MediatR;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Projects.Commands;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CreateProjectCommandResult> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        var project = _mapper.Map<ProjectEntity>(request);

        _context.Add(project);
        project.CreatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        return _mapper.Map<CreateProjectCommandResult>(project);
    }
}
