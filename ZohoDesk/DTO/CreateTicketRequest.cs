using System.Text.Json.Serialization;

namespace ZohoDesk.DTO;

/// <summary>
/// Запрос на создание тикета.
/// </summary>
public sealed class CreateTicketRequest
{
    /// <summary>
    /// Тема обращения.
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    /// Описание обращения.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Идентификатор контакта.
    /// </summary>
    [JsonPropertyName("contactId")]
    public string ContactId { get; init; } = string.Empty;

    /// <summary>
    /// Идентификатор подразделения.
    /// </summary>
    [JsonPropertyName("departmentId")]
    public string DepartmentId { get; init; } = string.Empty;

    /// <summary>
    /// Приоритет тикета.
    /// </summary>
    [JsonPropertyName("priority")]
    public string? Priority { get; init; }

    /// <summary>
    /// Статус тикета.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Канал создания тикета.
    /// </summary>
    [JsonPropertyName("channel")]
    public string? Channel { get; init; }

    /// <summary>
    /// Категория обращения.
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; init; }

    /// <summary>
    /// Подкатегория обращения.
    /// </summary>
    [JsonPropertyName("subCategory")]
    public string? SubCategory { get; init; }

    /// <summary>
    /// Классификация обращения.
    /// </summary>
    [JsonPropertyName("classification")]
    public string? Classification { get; init; }

    /// <summary>
    /// Email клиента.
    /// Используется, если контакт еще не создан.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Телефон клиента.
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }

    /// <summary>
    /// Дополнительные пользовательские поля.
    /// </summary>
    [JsonPropertyName("customFields")]
    public Dictionary<string, object>? CustomFields { get; init; }
}