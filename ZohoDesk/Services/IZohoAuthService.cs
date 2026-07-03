namespace ZohoDesk.Services;

/// <summary>
/// Предоставляет методы для получения и обновления OAuth Access Token,
/// используемого при работе с API Zoho Desk.
/// </summary>
public interface IZohoAuthService
{
    /// <summary>
    /// Возвращает действующий Access Token.
    /// Если срок действия токена истек или подходит к концу,
    /// выполняется его автоматическое обновление.
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Принудительно обновляет Access Token,
    /// игнорируя локальный кэш.
    /// Используется после получения ответа 401 Unauthorized.
    /// </summary>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    Task<string> RefreshAccessTokenAsync(
        CancellationToken cancellationToken = default);
}
