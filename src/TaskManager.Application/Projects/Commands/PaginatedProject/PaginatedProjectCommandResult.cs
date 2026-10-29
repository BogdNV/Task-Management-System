using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Commands;

public class PaginatedProjectCommandResult
{
    public IList<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}
