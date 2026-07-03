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
}