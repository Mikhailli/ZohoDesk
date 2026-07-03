using System.Net.Http.Json;
using ZohoDesk.DTO;
using ZohoDesk.Exceptions;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Методы расширения для работы с <see cref="HttpResponseMessage"/>.
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Десериализует успешный ответ API.
    /// </summary>
    /// <typeparam name="TResponse">
    /// Тип ожидаемой модели.
    /// </typeparam>
    /// <param name="response">
    /// HTTP-ответ.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Десериализованная модель.
    /// </returns>
    /// <exception cref="ZohoApiException">
    /// Возникает, если API вернул ошибку.
    /// </exception>
    public static async Task<TResponse?> ReadAsAsync<TResponse>(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentLength == 0)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(
                JsonOptionsProvider.Default,
                cancellationToken);
        }

        await ThrowApiExceptionAsync(
            response,
            cancellationToken);

        return default;
    }

    /// <summary>
    /// Генерирует исключение на основании ответа API.
    /// </summary>
    private static async Task ThrowApiExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ZohoErrorResponse? error = null;

        try
        {
            error = await response.Content.ReadFromJsonAsync<ZohoErrorResponse>(
                JsonOptionsProvider.Default,
                cancellationToken);
        }
        catch
        {
            // Если тело ответа не является JSON,
            // будет выброшено стандартное исключение ниже.
        }

        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

        throw new ZohoApiException(
            response.StatusCode,
            rawContent,
            response.RequestMessage?.RequestUri?.ToString(),
            error);
    }
}