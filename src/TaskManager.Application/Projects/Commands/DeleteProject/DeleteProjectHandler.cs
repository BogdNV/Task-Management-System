using MediatR;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Projects.Commands.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _repository;
    public DeleteProjectHandler(IProjectRepository repository)
    {
        _repository = repository;

    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
