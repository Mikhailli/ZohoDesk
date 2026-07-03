using System.Text.Json.Serialization;

namespace ZohoDesk.DTO;

/// <summary>
/// Запрос на создание комментария к тикету.
/// </summary>
public sealed class CreateTicketCommentRequest
{
    /// <summary>
    /// Содержимое комментария.
    /// Максимальная длина: 32000 символов.
    /// Для упоминания агента используйте формат: zsu[@user:{zuid}]zsu
    /// Для упоминания команды используйте формат: zsu[@team:{teamId}_{teamName}]zsu
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Является ли комментарий публичным.
    /// По умолчанию: true.
    /// </summary>
    [JsonPropertyName("isPublic")]
    public bool IsPublic { get; init; } = true;

    /// <summary>
    /// Тип содержимого: html или plainText.
    /// По умолчанию: html.
    /// Максимальная длина: 100 символов.
    /// </summary>
    [JsonPropertyName("contentType")]
    public string ContentType { get; init; } = "html";

    /// <summary>
    /// Список идентификаторов вложений.
    /// </summary>
    [JsonPropertyName("attachmentIds")]
    public List<string>? AttachmentIds { get; init; }
}
