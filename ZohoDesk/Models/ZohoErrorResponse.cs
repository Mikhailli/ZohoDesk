using System.Text.Json.Serialization;

namespace ZohoDesk.Models;

/// <summary>
/// Стандартная модель ошибки API Zoho.
/// </summary>
public sealed class ZohoErrorResponse
{
    /// <summary>
    /// Код ошибки.
    /// </summary>
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; init; }

    /// <summary>
    /// Описание ошибки.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}