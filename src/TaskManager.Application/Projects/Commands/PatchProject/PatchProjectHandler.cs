using MediatR;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Commands.PatchProject;

public class PatchProjectHandler : IRequestHandler<PatchProjectCommand>
{
    private readonly IProjectRepository _repository;
    public PatchProjectHandler(IProjectRepository repository)
    {
        _repository = repository;

    }
    public async Task Handle(PatchProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);
        if (project is null) return;

        if (string.IsNullOrEmpty(request.Name)
            && string.IsNullOrEmpty(request.Description))
            return;

        if (!string.IsNullOrEmpty(request.Name)) project.UpdateName(request.Name);
        if (!string.IsNullOrEmpty(request.Description)) project.UpdateDescription(request.Description);

        await _repository.UpdateAsync(project);
    }
}
