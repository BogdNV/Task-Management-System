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
        var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
        builder.Services.AddInfrastructure(connectionString);
        builder.Services.AddApplication(builder);

        var app = builder.Build();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
