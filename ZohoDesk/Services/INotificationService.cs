namespace ZohoDesk.Services;

/// <summary>
/// Сервис уведомлений о критических ошибках.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Отправляет уведомление о критической ошибке.
    /// </summary>
    /// <param name="message">
    /// Сообщение об ошибке.
    /// </param>
    /// <param name="exception">
    /// Исключение, если есть.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    Task NotifyFailureAsync(
        string message,
        Exception? exception = null,
        CancellationToken cancellationToken = default);
}
