using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Models.Request;

public class UpdateTaskRequest
{
    [MinLength(3), MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
