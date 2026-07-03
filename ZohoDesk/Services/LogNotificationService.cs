using Microsoft.Extensions.Logging;

namespace ZohoDesk.Services;

/// <summary>
/// Сервис уведомлений, записывающий ошибки в лог.
/// </summary>
/// <remarks>
/// Создает экземпляр сервиса уведомлений.
/// </remarks>
/// <param name="logger">
/// Логгер для записи уведомлений.
/// </param>
public sealed class LogNotificationService(
    ILogger<LogNotificationService> logger) : INotificationService
{
    private readonly ILogger<LogNotificationService> _logger = logger;

    public Task NotifyFailureAsync(
        string message,
        Exception? exception = null,
        CancellationToken cancellationToken = default)
    {
        if (exception is not null)
        {
            _logger.LogError(
                exception,
                "Критическая ошибка Zoho Desk API: {Message}",
                message);
        }
        else
        {
            _logger.LogError(
                "Критическая ошибка Zoho Desk API: {Message}",
                message);
        }

        return Task.CompletedTask;
    }
}
