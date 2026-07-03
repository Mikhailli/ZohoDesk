using System.Net;

namespace ZohoDesk.Exceptions;

/// <summary>
/// Исключение, возникающее при ошибках OAuth-аутентификации.
/// </summary>
/// <remarks>
/// Создает исключение.
/// </remarks>
public sealed class ZohoAuthenticationException(
    HttpStatusCode statusCode,
    string? responseContent) : ZohoException($"Ошибка OAuth-аутентификации. Код ответа: {(int)statusCode}.")
{
    /// <summary>
    /// HTTP-статус ответа.
    /// </summary>
    public HttpStatusCode StatusCode { get; } = statusCode;

    /// <summary>
    /// Текст ответа сервера.
    /// </summary>
    public string? ResponseContent { get; } = responseContent;
}