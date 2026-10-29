using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Projects.DTO;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Commands;

public class PaginatedProjectCommandHandler : IRequestHandler<PaginatedProjectCommand, PaginatedProjectCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PaginatedProjectCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedProjectCommandResult> Handle(PaginatedProjectCommand request, CancellationToken cancellationToken)
    {
        var projects = _context.Projects
            .AsNoTracking()
            .Where(p => p.UserId == request.UserId)
            .OrderBy(p => p.Id)
            .Skip((request.Offset - 1) * request.Limit)
            .Take(request.Limit)
            .Include(p => p.Tasks);

        var projList = await projects.ToListAsync(cancellationToken);

        var result = new PaginatedProjectCommandResult
        {
            Projects = _mapper.Map<List<ProjectDto>>(projList)

        };
        return result;
    }
}
