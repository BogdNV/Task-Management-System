using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Entities;

public class TaskItemEntity
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(5)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public Status Status { get; set; } = Status.New;
    [Required]
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public ProjectEntity? ProjectEntity { get; set; }
}
