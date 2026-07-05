using System.Text.Json.Serialization;

namespace ZohoDesk.Models;

/// <summary>
/// Контакт Zoho Desk.
/// </summary>
public sealed class Contact
{
    /// <summary>
    /// Идентификатор контакта.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Имя.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Фамилия.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Email.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Телефон.
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    [JsonPropertyName("createdTime")]
    public DateTimeOffset? CreatedTime { get; init; }

    /// <summary>
    /// Дата последнего изменения.
    /// </summary>
    [JsonPropertyName("modifiedTime")]
    public DateTimeOffset? ModifiedTime { get; init; }
}