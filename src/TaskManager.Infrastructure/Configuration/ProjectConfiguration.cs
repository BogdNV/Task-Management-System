using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

        builder.Property(p => p.Description)
                .IsRequired();

        builder.Property(p => p.CreatedAt)
                .IsRequired();

        builder.Property(p => p.OwnerId)
                .IsRequired();
    }
}
