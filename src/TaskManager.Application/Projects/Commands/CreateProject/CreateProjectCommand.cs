using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Commands;

public class CreateProjectCommand : IRequest<ProjectDto>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
