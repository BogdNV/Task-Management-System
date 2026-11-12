using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.API.Mappings;
using TaskManager.Application.Common;
using TaskManager.Application.Mappings.Project;
using TaskManager.Application.Services;

namespace TaskManager.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IGetUserIdService, GetUserIdService>();
        services.AddAutoMapper(
            cfg => { },
            typeof(RegistrationMappingProfile).Assembly,
            typeof(ProjectMappingProfile).Assembly
        );
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.Configure<JwtTokenOptions>(
            configuration.GetSection("AuthSettings")
        );
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(op =>
            {
                var authSettings = configuration.GetSection("AuthSettings").Get<JwtTokenOptions>();
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings!.SecretKey)),
                    ValidateIssuerSigningKey = true
                };

                op.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                            ctx.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });
        return services;
    }
}
