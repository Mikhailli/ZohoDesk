using System.Text.Json.Serialization;

namespace ZohoDesk.DTO;

/// <summary>
/// Результат поиска контактов Zoho Desk.
/// </summary>
public sealed class ContactsSearchResult
{
    /// <summary>
    /// Список найденных контактов.
    /// </summary>
    [JsonPropertyName("data")]
    public List<Contact>? Data { get; init; }

    /// <summary>
    /// Количество найденных контактов.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }
}
