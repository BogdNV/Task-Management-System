using MediatR;

namespace TaskManager.Application.Projects.Commands;

public class CreateProjectCommand : IRequest<CreateProjectCommandResult>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
