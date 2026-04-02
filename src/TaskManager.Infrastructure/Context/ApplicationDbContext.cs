using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Configuration;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
    }
}
