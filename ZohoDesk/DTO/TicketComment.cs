using System.Text.Json.Serialization;

namespace ZohoDesk.DTO;

/// <summary>
/// Комментарий к тикету Zoho Desk.
/// </summary>
public sealed class TicketComment
{
    /// <summary>
    /// Идентификатор комментария.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Содержимое комментария.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>
    /// Является ли комментарий публичным.
    /// </summary>
    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; init; }

    /// <summary>
    /// Тип содержимого: html или plainText.
    /// </summary>
    [JsonPropertyName("contentType")]
    public string? ContentType { get; init; }

    /// <summary>
    /// Время создания комментария.
    /// </summary>
    [JsonPropertyName("commentedTime")]
    public DateTimeOffset? CommentedTime { get; init; }

    /// <summary>
    /// Время последнего изменения.
    /// </summary>
    [JsonPropertyName("modifiedTime")]
    public DateTimeOffset? ModifiedTime { get; init; }

    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    [JsonPropertyName("commenterId")]
    public string? CommenterId { get; init; }

    /// <summary>
    /// Информация об авторе комментария.
    /// </summary>
    [JsonPropertyName("commenter")]
    public Commenter? Commenter { get; init; }

    /// <summary>
    /// Список вложений комментария.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<CommentAttachment>? Attachments { get; init; }
}

/// <summary>
/// Автор комментария.
/// </summary>
public sealed class Commenter
{
    /// <summary>
    /// Имя автора.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Фамилия автора.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    /// <summary>
    /// Полное имя автора.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// URL фотографии автора.
    /// </summary>
    [JsonPropertyName("photoURL")]
    public string? PhotoURL { get; init; }

    /// <summary>
    /// Email автора.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// Роль автора.
    /// </summary>
    [JsonPropertyName("roleName")]
    public string? RoleName { get; init; }

    /// <summary>
    /// Тип автора (AGENT, CONTACT).
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>
/// Вложение комментария.
/// </summary>
public sealed class CommentAttachment
{
    /// <summary>
    /// Идентификатор вложения.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Имя файла.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Размер файла в байтах.
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; init; }

    /// <summary>
    /// URL для скачивания вложения.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }
}
