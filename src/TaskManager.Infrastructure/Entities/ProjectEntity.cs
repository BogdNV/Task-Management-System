using System.ComponentModel.DataAnnotations;

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
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public ICollection<TaskItemEntity> Tasks { get; set; } = new List<TaskItemEntity>();

    public DateTime? LastModified { get; set; }
}
