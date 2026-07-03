using ZohoDesk.Constants;

namespace ZohoDesk.Authentication;

/// <summary>
/// Модель запроса на получение нового Access Token
/// с использованием Refresh Token.
/// </summary>
public sealed class RefreshAccessTokenRequest
{
    /// <summary>
    /// Идентификатор OAuth-приложения.
    /// </summary>
    public required string ClientId { get; init; }

    /// <summary>
    /// Секрет OAuth-приложения.
    /// </summary>
    public required string ClientSecret { get; init; }

    /// <summary>
    /// Refresh Token.
    /// </summary>
    public required string RefreshToken { get; init; }

    /// <summary>
    /// Преобразует модель в набор Query-параметров.
    /// </summary>
    public IReadOnlyDictionary<string, string> ToQueryParameters()
    {
        return new Dictionary<string, string>
        {
            [ZohoOAuthParameters.ClientId] = ClientId,
            [ZohoOAuthParameters.ClientSecret] = ClientSecret,
            [ZohoOAuthParameters.RefreshToken] = RefreshToken,
            [ZohoOAuthParameters.GrantType] = ZohoOAuthParameters.RefreshTokenGrant
        };
    }
}