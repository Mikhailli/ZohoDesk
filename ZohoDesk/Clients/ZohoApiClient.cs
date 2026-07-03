using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using ZohoDesk.Constants;
using ZohoDesk.Infrastructure;
using ZohoDesk.Options;
using ZohoDesk.Services;

namespace ZohoDesk.Clients;

/// <summary>
/// Универсальный клиент для выполнения HTTP-запросов к API Zoho Desk.
/// Отвечает за:
/// - получение Access Token;
/// - автоматическое обновление токена;
/// - повторные попытки выполнения запросов;
/// - добавление обязательных HTTP-заголовков;
/// - обработку ответов API.
/// </summary>
/// <remarks>
/// Создает экземпляр клиента API Zoho Desk.
/// </remarks>
public sealed class ZohoApiClient(
    HttpClient httpClient,
    IZohoAuthService authService,
    IOptions<ZohoDeskOptions> options,
    ILogger<ZohoApiClient> logger) : IZohoApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IZohoAuthService _authService = authService;
    private readonly ZohoDeskOptions _options = options.Value;
    private readonly ILogger<ZohoApiClient> _logger = logger;

    public async Task<TResponse?> SendAsync<TResponse>(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken = default)
    {
        using var response = await ExecuteAsync(
            requestFactory,
            cancellationToken);

        return await response.ReadAsAsync<TResponse>(
            cancellationToken);
    }

    public async Task SendAsync(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken = default)
    {
        using var response = await ExecuteAsync(
            requestFactory,
            cancellationToken);

        await response.ReadAsAsync<object>(
            cancellationToken);
    }

    /// <summary>
    /// Выполняет HTTP-запрос с автоматическим обновлением токена
    /// и повторными попытками при временных ошибках.
    /// </summary>
    private async Task<HttpResponseMessage> ExecuteAsync(
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken)
    {
        var accessToken =
            await _authService.GetAccessTokenAsync(cancellationToken);

        var authorizationRetryPerformed = false;

        for (var retryAttempt = 0; ; retryAttempt++)
        {
            using var request = PrepareRequest(
                requestFactory,
                accessToken);

            _logger.LogInformation(
                "Отправка запроса {Method} {Uri}",
                request.Method,
                request.RequestUri);

            var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Запрос выполнен успешно. Код ответа {StatusCode}",
                    response.StatusCode);

                return response;
            }

            if (!authorizationRetryPerformed &&
                response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();

                _logger.LogWarning(
                    "Получен ответ 401 Unauthorized. Выполняется обновление Access Token.");

                accessToken = await _authService.RefreshAccessTokenAsync(
                    cancellationToken);

                authorizationRetryPerformed = true;

                continue;
            }

            if (_options.Retry.Enabled &&
                ShouldRetry(response.StatusCode) &&
                retryAttempt < _options.Retry.Delays.Length)
            {
                response.Dispose();

                var delay = _options.Retry.Delays[retryAttempt];

                _logger.LogWarning(
                    "Получен код {StatusCode}. Повторная попытка через {Delay}.",
                    response.StatusCode,
                    delay);

                await Task.Delay(
                    delay,
                    cancellationToken);

                continue;
            }

            return response;
        }
    }

    /// <summary>
    /// Создает HTTP-запрос и добавляет обязательные заголовки.
    /// </summary>
    private HttpRequestMessage PrepareRequest(
        Func<HttpRequestMessage> requestFactory,
        string accessToken)
    {
        var request = requestFactory();

        AddDefaultHeaders(
            request,
            accessToken);

        return request;
    }

    /// <summary>
    /// Добавляет обязательные HTTP-заголовки.
    /// </summary>
    private void AddDefaultHeaders(
        HttpRequestMessage request,
        string accessToken)
    {
        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                ZohoHeaders.OAuthScheme,
                accessToken);

        request.Headers.Remove(
            ZohoHeaders.OrganizationId);

        request.Headers.Add(
            ZohoHeaders.OrganizationId,
            _options.OrganizationId);

        request.Headers.Accept.Clear();

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                ZohoContentTypes.ApplicationJson));
    }

    /// <summary>
    /// Определяет необходимость повторной попытки.
    /// </summary>
    private static bool ShouldRetry(
        HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.TooManyRequests
            || (int)statusCode >= 500;
    }
}