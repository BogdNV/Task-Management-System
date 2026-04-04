using MediatR;
using TaskManager.Application.Mappers;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Commands.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _repository;
    public UpdateProjectHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);
        if (project is null) return;

        project.UpdateName(request.Name);
        project.UpdateDescription(request.Description);

        await _repository.UpdateAsync(project);
    }
}
