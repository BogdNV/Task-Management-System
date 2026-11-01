namespace TaskManager.API.Models.Response.Project;

public class PutProjectResponse
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
