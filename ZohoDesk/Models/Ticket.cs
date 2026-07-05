using System.Text.Json.Serialization;

namespace ZohoDesk.Models;

/// <summary>
/// Тикет Zoho Desk.
/// </summary>
public sealed class Ticket
{
    /// <summary>
    /// Идентификатор тикета.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Номер тикета.
    /// </summary>
    [JsonPropertyName("ticketNumber")]
    public string? TicketNumber { get; init; }

    /// <summary>
    /// Тема обращения.
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    /// Описание обращения.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Статус тикета.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Приоритет тикета.
    /// </summary>
    [JsonPropertyName("priority")]
    public string? Priority { get; init; }

    /// <summary>
    /// Канал создания тикета.
    /// </summary>
    [JsonPropertyName("channel")]
    public string? Channel { get; init; }

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