namespace ZohoDesk.Constants;

/// <summary>
/// Имена HTTP-заголовков, используемых при работе с API Zoho Desk.
/// </summary>
public static class ZohoHeaders
{
    /// <summary>
    /// Заголовок авторизации.
    /// </summary>
    public const string Authorization = "Authorization";

    /// <summary>
    /// Заголовок с идентификатором организации Zoho Desk.
    /// </summary>
    public const string OrganizationId = "orgId";

    /// <summary>
    /// Заголовок типа содержимого запроса.
    /// </summary>
    public const string ContentType = "Content-Type";

    /// <summary>
    /// Заголовок типа принимаемого ответа.
    /// </summary>
    public const string Accept = "Accept";

    /// <summary>
    /// Схема авторизации Zoho OAuth.
    /// </summary>
    public const string OAuthScheme = "Zoho-oauthtoken";
}
