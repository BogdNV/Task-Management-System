using MediatR;
using TaskManager.Application.Mappers;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _repository;

    public GetAllProjectsHandler(IProjectRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync();

        return projects.Select(p => p.ToDto());
    }
}
