using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProjectsHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = _context.Projects
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((request.Offset - 1) * request.Limit)
            .Take(request.Limit)
            .Include(p => p.Tasks)
            .Include(p => p.User);

        var projectList = await projects.ToListAsync();
        var result = _mapper.Map<List<ProjectDto>>(projectList);


        return result;
    }
}
