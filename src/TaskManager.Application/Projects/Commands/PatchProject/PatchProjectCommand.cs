using MediatR;

namespace TaskManager.Application.Projects.Commands;

public class PatchProjectCommand : IRequest
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}
