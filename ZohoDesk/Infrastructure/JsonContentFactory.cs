using System.Text;
using System.Text.Json;
using ZohoDesk.Constants;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Фабрика для создания JSON-содержимого HTTP-запросов.
/// </summary>
public static class JsonContentFactory
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Создает HTTP-содержимое из объекта.
    /// </summary>
    public static HttpContent Create<T>(T value)
    {
        var json = JsonSerializer.Serialize(
            value,
            JsonOptions);

        return new StringContent(
            json,
            Encoding.UTF8,
            ZohoContentTypes.ApplicationJson);
    }
}