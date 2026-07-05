using System.Text.Json.Serialization;

namespace ZohoDesk.Models;

/// <summary>
/// Запрос на обновление тикета.
/// </summary>
public sealed class UpdateTicketRequest
{
    /// <summary>
    /// Тема обращения.
    /// </summary>
    [JsonPropertyName("subject")]
    public string? Subject { get; init; }

    /// <summary>
    /// Описание обращения.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

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
    /// Идентификатор сотрудника, назначенного на тикет.
    /// </summary>
    [JsonPropertyName("assigneeId")]
    public string? AssigneeId { get; init; }

    /// <summary>
    /// Дополнительные пользовательские поля.
    /// </summary>
    [JsonPropertyName("customFields")]
    public Dictionary<string, object>? CustomFields { get; init; }
}