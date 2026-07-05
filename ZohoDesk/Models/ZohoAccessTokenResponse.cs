using System.Text.Json.Serialization;

namespace ZohoDesk.Models;

/// <summary>
/// Ответ OAuth API при получении Access Token.
/// </summary>
public sealed class ZohoAccessTokenResponse
{
    /// <summary>
    /// Access Token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    /// <summary>
    /// Refresh Token.
    /// Возвращается только при первой авторизации.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    /// <summary>
    /// Тип токена.
    /// </summary>
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    /// <summary>
    /// Время жизни токена в секундах.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }
}
