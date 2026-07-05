namespace ZohoDesk.Services;

/// <summary>
/// Универсальный клиент для выполнения HTTP-запросов к API Zoho Desk.
/// </summary>
public interface IZohoApiClient
{
    /// <summary>
    /// Выполняет HTTP-запрос и десериализует успешный ответ.
    /// </summary>
    /// <typeparam name="TResponse">
    /// Тип модели ответа.
    /// </typeparam>
    /// <param name="requestFactory">
    /// Фабрика создания HTTP-запроса.
    /// Новый экземпляр запроса создается при каждой повторной попытке.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Десериализованный ответ API.
    /// </returns>
    Task<TResponse?> SendAsync<TResponse>(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет HTTP-запрос без ожидаемого тела ответа.
    /// Используется для операций, возвращающих HTTP 204 No Content.
    /// </summary>
    /// <param name="requestFactory">
    /// Фабрика создания HTTP-запроса.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены операции.
    /// </param>
    Task SendAsync(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken = default);
}