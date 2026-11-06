using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.Token)
            .HasColumnName("token")
            .IsRequired();

        builder.Property(t => t.Created)
            .HasColumnName("created")
            .IsRequired();

        builder.Property(t => t.Expires)
            .HasColumnName("expires")
            .IsRequired();

    }
}
