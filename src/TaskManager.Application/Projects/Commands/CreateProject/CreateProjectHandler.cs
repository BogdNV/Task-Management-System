using AutoMapper;
using MediatR;
using TaskManager.Application.Projects.DTO;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Projects.Commands;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken ct)
    {

        var project = new ProjectEntity()
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };


        _context.Add(project);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }
}
