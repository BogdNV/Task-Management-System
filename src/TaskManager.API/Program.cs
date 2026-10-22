using Microsoft.AspNetCore.Identity;
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
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();
        var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
        builder.Services.AddInfrastructure(connectionString);
        builder.Services.AddScoped(typeof(IPasskeyHandler<>), typeof(PasswordHasher<>));
        builder.Services.AddApplication(builder);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
