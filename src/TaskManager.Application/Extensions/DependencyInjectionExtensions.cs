using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
