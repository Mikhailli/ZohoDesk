namespace ZohoDesk.Models;

/// <summary>
/// Представляет текущий OAuth Access Token,
/// используемый для авторизации в Zoho Desk API.
/// </summary>
public sealed class ZohoToken
{
    /// <summary>
    /// Access Token.
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    /// Дата и время окончания действия токена (UTC).
    /// </summary>
    public required DateTime ExpiresAtUtc { get; init; }

    /// <summary>
    /// Возвращает <c>true</c>, если срок действия токена истек.
    /// </summary>
    public bool IsExpired =>
        DateTime.UtcNow >= ExpiresAtUtc;

    /// <summary>
    /// Возвращает <c>true</c>, если токен необходимо обновить.
    /// Обновление выполняется заранее, чтобы избежать ошибок
    /// авторизации во время выполнения запроса.
    /// </summary>
    public bool RequiresRefresh =>
        DateTime.UtcNow >= ExpiresAtUtc.AddMinutes(-2);

    /// <summary>
    /// Оставшееся время жизни токена.
    /// Если токен уже истек, возвращается TimeSpan.Zero.
    /// </summary>
    public TimeSpan RemainingLifetime =>
        IsExpired
            ? TimeSpan.Zero
            : ExpiresAtUtc - DateTime.UtcNow;

    /// <summary>
    /// Создает экземпляр токена.
    /// </summary>
    public static ZohoToken Create(
        string accessToken,
        int expiresIn)
    {
        return new ZohoToken
        {
            AccessToken = accessToken,
            ExpiresAtUtc = DateTime.UtcNow.AddSeconds(expiresIn)
        };
    }
}
