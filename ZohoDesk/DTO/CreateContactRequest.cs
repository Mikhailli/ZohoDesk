using System.Text.Json.Serialization;

namespace ZohoDesk.DTO;

/// <summary>
/// Запрос на создание контакта.
/// </summary>
public sealed class CreateContactRequest
{
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
    /// Адрес электронной почты.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Номер телефона.
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
}