using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models.Request;

public class CreateProjectRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
