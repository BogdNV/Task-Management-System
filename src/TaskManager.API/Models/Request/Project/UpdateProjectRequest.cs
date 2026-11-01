using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models.Request;

public class UpdateProjectRequest
{
    public Guid UserId { get; set; }
    public int Id { get; set; }

    [MinLength(5)]
    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}
