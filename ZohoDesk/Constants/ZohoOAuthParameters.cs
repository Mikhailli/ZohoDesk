namespace ZohoDesk.Constants;

/// <summary>
/// Названия параметров OAuth-запросов,
/// используемых для получения Access Token.
/// </summary>
public static class ZohoOAuthParameters
{
    /// <summary>
    /// Идентификатор OAuth-приложения.
    /// </summary>
    public const string ClientId = "client_id";

    /// <summary>
    /// Секрет OAuth-приложения.
    /// </summary>
    public const string ClientSecret = "client_secret";

    /// <summary>
    /// Refresh Token.
    /// </summary>
    public const string RefreshToken = "refresh_token";

    /// <summary>
    /// Тип OAuth Grant.
    /// </summary>
    public const string GrantType = "grant_type";

    /// <summary>
    /// Значение Grant Type для обновления Access Token.
    /// </summary>
    public const string RefreshTokenGrant = "refresh_token";

    /// <summary>
    /// Код авторизации (grant token).
    /// </summary>
    public const string Code = "code";

    /// <summary>
    /// URI перенаправления.
    /// </summary>
    public const string RedirectUri = "redirect_uri";

    /// <summary>
    /// Тип ответа.
    /// </summary>
    public const string ResponseType = "response_type";

    /// <summary>
    /// Область доступа (scopes).
    /// </summary>
    public const string Scope = "scope";

    /// <summary>
    /// Тип доступа (online/offline).
    /// </summary>
    public const string AccessType = "access_type";

    /// <summary>
    /// Параметр state для защиты от CSRF.
    /// </summary>
    public const string State = "state";

    /// <summary>
    /// Prompt для согласия пользователя.
    /// </summary>
    public const string Prompt = "prompt";
}