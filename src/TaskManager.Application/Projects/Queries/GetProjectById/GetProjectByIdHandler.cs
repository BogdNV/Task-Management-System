using MediatR;
using TaskManager.Application.Mappers;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectRepository _repository;
    public GetProjectByIdHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);

        return project?.ToDto();
    }
}
