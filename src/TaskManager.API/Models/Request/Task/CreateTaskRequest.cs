using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Models.Request;

public class CreateTaskRequest
{
    [Required]
    public int ProjectId { get; set; }
    [Required]
    [MinLength(3), MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public Status Status { get; set; } = Status.New;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }

}
