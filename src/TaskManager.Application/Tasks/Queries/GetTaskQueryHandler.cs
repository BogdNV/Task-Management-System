using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Tasks.DTO;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Tasks.Queries;

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, GetTaskQueryResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTaskQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTaskQueryResult> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = _context.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectEntity!.UserId == request.UserId);

        if (request.Status.HasValue)
            tasks = tasks.Where(t => t.Status == request.Status.Value);

        if (request.Priority.HasValue)
            tasks = tasks.Where(t => t.Priority == request.Priority.Value);

        await tasks.ToListAsync(cancellationToken);

        var result = new GetTaskQueryResult { Tasks = _mapper.Map<List<TaskDto>>(tasks) };
        return result;
    }
}
