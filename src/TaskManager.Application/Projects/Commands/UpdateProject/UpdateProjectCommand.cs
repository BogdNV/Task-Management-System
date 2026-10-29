using MediatR;

namespace TaskManager.Application.Projects.Commands;

public class PutProjectCommand : IRequest
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}