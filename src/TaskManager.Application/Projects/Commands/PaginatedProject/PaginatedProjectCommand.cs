using MediatR;

namespace TaskManager.Application.Projects.Commands;

public class PaginatedProjectCommand : IRequest<PaginatedProjectCommandResult>
{
    public Guid UserId { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
}
