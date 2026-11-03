using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["MEDIATR_LICENSE_KEY"];
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
