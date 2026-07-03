namespace ZohoDesk.Options;

/// <summary>
/// Настройки повторных попыток выполнения HTTP-запросов.
/// </summary>
public sealed class RetryOptions
{
    /// <summary>
    /// Включить механизм повторных попыток.
    /// </summary>
    public bool Enabled { get; init; } = true;

    /// <summary>
    /// Интервалы между повторными попытками.
    /// </summary>
    public TimeSpan[] Delays { get; init; } =
    [
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(30),
        TimeSpan.FromSeconds(120)
    ];
}