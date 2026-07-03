using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Предоставляет единые настройки сериализации JSON,
/// используемые во всей библиотеке.
/// </summary>
public static class JsonOptionsProvider
{
    /// <summary>
    /// Настройки сериализации JSON.
    /// </summary>
    public static JsonSerializerOptions Default { get; } = Create();

    /// <summary>
    /// Создает экземпляр настроек сериализации.
    /// </summary>
    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            // Игнорировать регистр имен свойств.
            PropertyNameCaseInsensitive = true,

            // Не сериализовать свойства со значением null.
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

            // Красивое форматирование не требуется.
            WriteIndented = false
        };

        // Преобразование enum в строковое представление.
        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}