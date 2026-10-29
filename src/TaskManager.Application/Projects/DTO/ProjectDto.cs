using TaskManager.Application.Tasks.DTO;

namespace TaskManager.Application.Projects.DTO;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IList<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
};
