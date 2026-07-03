using ZohoDesk.DTO;

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

    /// <summary>
    /// Обменивает grant token (authorization code) на access token и refresh token.
    /// Используется один раз при первоначальной настройке Self Client авторизации.
    /// </summary>
    /// <param name="grantToken">
    /// Grant token (authorization code), полученный через Zoho Developer Console.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Ответ с access token, refresh token и временем жизни.
    /// </returns>
    Task<ZohoAccessTokenResponse> ExchangeCodeForTokenAsync(
        string grantToken,
        CancellationToken cancellationToken = default);
}
