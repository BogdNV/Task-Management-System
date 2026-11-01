using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public class GetProjectQueryResult
{
    public IList<ProjectDto> Content { get; set; } = new List<ProjectDto>();
}
