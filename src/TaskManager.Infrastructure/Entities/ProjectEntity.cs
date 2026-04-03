using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Entities;

public class ProjectEntity
{
    public int Id { get; set; }
    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public int OwnerId { get; set; }

    public List<TaskItemEntity> Tasks { get; set; } = new();

    public DateTime? LastModified { get; set; }
}
