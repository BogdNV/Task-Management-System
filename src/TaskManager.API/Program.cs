using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using TaskManager.API.Extensions;
using TaskManager.API.Middleware;
using TaskManager.Application.Extensions;
using TaskManager.Infrastructure.Extensions;

namespace TaskManager.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Введите токен",
                Name = "auth",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
            });
        });
        builder.Services.AddOpenApi();
        var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
        builder.Services.AddApiServices(builder.Configuration);
        builder.Services.AddInfrastructure(connectionString!);
        builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        builder.Services.AddApplication(builder);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Frontend из wwwroot раздаётся с тем же origin, что и API,
        // поэтому CORS не требуется, а куки остаются first-party.
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
