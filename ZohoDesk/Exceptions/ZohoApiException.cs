using System.Net;
using ZohoDesk.Models;

namespace ZohoDesk.Exceptions;

public sealed class ZohoApiException(
    HttpStatusCode statusCode,
    string? responseContent,
    string? requestUri,
    ZohoErrorResponse? error = null) : ZohoException(CreateMessage(statusCode, error))
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    public string? ResponseContent { get; } = responseContent;

    public string? RequestUri { get; } = requestUri;

    /// <summary>
    /// Ошибка, возвращенная API Zoho.
    /// </summary>
    public ZohoErrorResponse? Error { get; } = error;

    private static string CreateMessage(
        HttpStatusCode statusCode,
        ZohoErrorResponse? error)
    {
        if (error is null)
        {
            return $"Ошибка API Zoho. Код ответа {(int)statusCode}.";
        }

        return
            $"Ошибка API Zoho. Код {(int)statusCode}. " +
            $"[{error.ErrorCode}] {error.Message}";
    }
}