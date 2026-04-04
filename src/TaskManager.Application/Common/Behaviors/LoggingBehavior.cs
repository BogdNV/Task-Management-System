using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskManager.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, Tresponse> : IPipelineBehavior<TRequest, Tresponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, Tresponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, Tresponse>> logger)
    {
        _logger = logger;
    }
    public async Task<Tresponse> Handle(TRequest request, RequestHandlerDelegate<Tresponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("🚀 Начало выполнения {RequestName}: {@Request}",
            requestName, request);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var respone = await next();

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 1000)
                _logger.LogWarning(
                    "⚠️ {RequestName} выполнен медленно: {ElapsedMilliseconds}ms",
                    requestName, stopwatch.ElapsedMilliseconds);
            else
                _logger.LogInformation("✅ {RequestName} выполнен за {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);

            return respone;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "❌ Ошибка при выполнении {RequestName} за {ElapsedMilliseconds}ms: {ErrorMessage}",
                requestName, stopwatch.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}
