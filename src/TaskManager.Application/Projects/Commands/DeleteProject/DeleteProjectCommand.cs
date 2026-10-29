using MediatR;

namespace TaskManager.Application.Projects.Commands;

public class DeleteProjectCommand : IRequest
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
}
