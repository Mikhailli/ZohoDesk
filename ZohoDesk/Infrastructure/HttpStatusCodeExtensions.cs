using System.Net;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Методы расширения для <see cref="HttpStatusCode"/>.
/// </summary>
public static class HttpStatusCodeExtensions
{
    /// <summary>
    /// Определяет, требуется ли повторить запрос.
    /// </summary>
    /// <param name="statusCode">
    /// HTTP-статус ответа.
    /// </param>
    public static bool IsTransientError(this HttpStatusCode statusCode)
    {
        return statusCode is
            HttpStatusCode.TooManyRequests or
            HttpStatusCode.InternalServerError or
            HttpStatusCode.BadGateway or
            HttpStatusCode.ServiceUnavailable or
            HttpStatusCode.GatewayTimeout;
    }

    /// <summary>
    /// Определяет, связана ли ошибка с авторизацией.
    /// </summary>
    /// <param name="statusCode">
    /// HTTP-статус ответа.
    /// </param>
    public static bool IsAuthenticationError(this HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.Unauthorized;
    }

    /// <summary>
    /// Определяет, является ли ответ успешным.
    /// </summary>
    /// <param name="statusCode">
    /// HTTP-статус ответа.
    /// </param>
    public static bool IsSuccess(this HttpStatusCode statusCode)
    {
        var code = (int)statusCode;

        return code >= 200 && code < 300;
    }
}