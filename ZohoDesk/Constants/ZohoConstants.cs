namespace ZohoDesk.Constants;

public static class ZohoConstants
{
    #region Content Types

    /// <summary>
    /// JSON.
    /// </summary>
    public const string ApplicationJson = "application/json";

    #endregion

    #region ZohoHeaders

    /// <summary>
    /// Заголовок с идентификатором организации Zoho Desk.
    /// </summary>
    public const string OrganizationId = "orgId";

    /// <summary>
    /// Схема авторизации Zoho OAuth.
    /// </summary>
    public const string OAuthScheme = "Zoho-oauthtoken";

    #endregion

    #region ZohoOAuthParameters

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
    /// Значение Grant Type для обмена grant token на access/refresh tokens.
    /// </summary>
    public const string AuthorizationTokenGrant = "authorization_code";

    /// <summary>
    /// Код авторизации (grant token).
    /// </summary>
    public const string Code = "code";

    /// <summary>
    /// URI перенаправления.
    /// </summary>
    public const string RedirectUri = "redirect_uri";

    #endregion
}
