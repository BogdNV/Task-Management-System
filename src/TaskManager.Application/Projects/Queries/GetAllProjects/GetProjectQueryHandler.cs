using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Projects.DTO;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, GetProjectQueryResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProjectsHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetProjectQueryResult> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<ProjectEntity> queryable = _context.Projects.AsNoTracking();

        if (request.UserId is not null)
        {
            queryable = queryable.Where(p => p.UserId == request.UserId);
        }

        var projectList = await queryable
            .Include(p => p.Tasks)
            .Include(p => p.User)
            .AsSplitQuery()
            .OrderBy(p => p.Id)
            .Skip((request.Offset - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);


        return new GetProjectQueryResult { Content = _mapper.Map<List<ProjectDto>>(projectList) };
    }
}
