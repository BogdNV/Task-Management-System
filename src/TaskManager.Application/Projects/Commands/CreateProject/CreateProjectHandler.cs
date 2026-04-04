using MediatR;
using TaskManager.Application.Mappers;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _repository;

    public CreateProjectHandler(IProjectRepository repository)
    {
        _repository = repository;
    }
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        // 1. Маппинг Command → Domain
        var project = request.ToDomain();

        // 2. Сохранение через репозиторий
        var savedProject = await _repository.AddAsync(project);

        // 3. Маппинг Domain → DTO для ответа
        return savedProject.ToDto();
    }
}
