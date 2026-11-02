using TaskManager.Application.Tasks.DTO;

namespace TaskManager.Application.Tasks.Queries;

public class GetTaskQueryResult
{
    public IList<TaskDto> Tasks { get; set; } = new List<TaskDto>();
}
