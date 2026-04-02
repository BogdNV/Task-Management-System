using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Configuration;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItemEntity>
{
    public void Configure(EntityTypeBuilder<TaskItemEntity> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(t => t.Description)
                .HasMaxLength(1000)
                .IsRequired();

        builder.HasOne(t => t.ProjectEntity)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue((int)Status.New);

        builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue((int)Priority.Medium);

        builder.HasIndex(t => t.ProjectId);
    }
}
