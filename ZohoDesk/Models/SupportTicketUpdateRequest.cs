namespace ZohoDesk.Models;

/// <summary>
/// Запрос на обновление обращения.
/// </summary>
public sealed class SupportTicketUpdateRequest
{
    /// <summary>
    /// Новый статус обращения.
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Новый приоритет.
    /// </summary>
    public string? Priority { get; init; }

    /// <summary>
    /// Новая категория.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Новая подкатегория.
    /// </summary>
    public string? SubCategory { get; init; }

    /// <summary>
    /// Новое описание.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Пользовательские поля.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; init; }
}