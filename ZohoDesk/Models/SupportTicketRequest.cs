namespace ZohoDesk.Models;

/// <summary>
/// Запрос на создание обращения в службу поддержки.
/// </summary>
public sealed class SupportTicketRequest
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Телефон пользователя.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Тема обращения.
    /// </summary>
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    /// Описание обращения.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Приоритет обращения.
    /// </summary>
    public string? Priority { get; init; }

    /// <summary>
    /// Категория обращения.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Подкатегория обращения.
    /// </summary>
    public string? SubCategory { get; init; }

    /// <summary>
    /// Пользовательские поля.
    /// </summary>
    public Dictionary<string, object>? CustomFields { get; init; }
}