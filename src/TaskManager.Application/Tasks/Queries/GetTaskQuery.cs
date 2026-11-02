using MediatR;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Queries;

public class GetTaskQuery : IRequest<GetTaskQueryResult>
{
    public Guid UserId { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
}
