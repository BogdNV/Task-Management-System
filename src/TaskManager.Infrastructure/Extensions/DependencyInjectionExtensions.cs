using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();

        return services;
    }
}
