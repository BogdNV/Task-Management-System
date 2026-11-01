using TaskManager.Application.Projects.DTO;

namespace TaskManager.API.Models.Response.Project;

public class GetPagidatedProjectResponse
{
    public IList<ProjectDto> Content { get; set; } = new List<ProjectDto>();
}
