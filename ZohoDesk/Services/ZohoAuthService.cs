using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ZohoDesk.Authentication;
using ZohoDesk.Constants;
using ZohoDesk.DTO;
using ZohoDesk.Models;
using ZohoDesk.Options;

namespace ZohoDesk.Services;

/// <summary>
/// Сервис получения и обновления OAuth Access Token.
/// </summary>
public sealed class ZohoAuthService(
    HttpClient httpClient,
    IOptions<ZohoDeskOptions> options,
    IZohoTokenStore tokenStore,
    ILogger<ZohoAuthService> logger) : IZohoAuthService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ZohoDeskOptions _options = options.Value;
    private readonly IZohoTokenStore _tokenStore = tokenStore;
    private readonly ILogger<ZohoAuthService> _logger = logger;

    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <inheritdoc />
    public async Task<ZohoAccessTokenResponse> ExchangeCodeForTokenAsync(
        string grantToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(grantToken);

        _logger.LogInformation("Обмен grant token на access/refresh tokens.");

        var url = QueryHelpers.AddQueryString(
            ZohoEndpoints.OAuthToken,
            new Dictionary<string, string?>
            {
                [ZohoOAuthParameters.Code] = grantToken,
                [ZohoOAuthParameters.ClientId] = _options.ClientId,
                [ZohoOAuthParameters.ClientSecret] = _options.ClientSecret,
                [ZohoOAuthParameters.GrantType] = "authorization_code",
                [ZohoOAuthParameters.RedirectUri] = _options.RedirectUri
            });

        using var response = await _httpClient.PostAsync(
            url,
            null,
            cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Ошибка обмена grant token. Статус: {Status}. Ответ: {Response}",
                response.StatusCode,
                json);

            throw new InvalidOperationException(
                $"Не удалось обменять grant token. Код: {response.StatusCode}. Ответ: {json}");
        }

        var tokenResponse = JsonSerializer.Deserialize<ZohoAccessTokenResponse>(
            json,
            JsonOptions);

        if (tokenResponse is null)
        {
            throw new InvalidOperationException("Получен пустый ответ OAuth.");
        }

        // Сохраняем токен в хранилище
        var token = ZohoToken.Create(
            tokenResponse.AccessToken,
            tokenResponse.ExpiresIn);

        await _tokenStore.SaveAsync(token, cancellationToken);

        _logger.LogInformation(
            "Токены успешно получены. Access token действителен до {ExpiresAt}. Refresh token сохранён.",
            token.ExpiresAtUtc);

        return tokenResponse;
    }

    /// <inheritdoc />
    public async Task<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        var token = await _tokenStore.GetAsync(cancellationToken);

        if (token is not null &&
            !token.RequiresRefresh)
        {
            return token.AccessToken;
        }

        return await RefreshAccessTokenAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> RefreshAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            var token = await _tokenStore.GetAsync(cancellationToken);

            if (token is not null &&
                !token.RequiresRefresh)
            {
                return token.AccessToken;
            }

            _logger.LogInformation("Получение нового Access Token Zoho.");

            if (string.IsNullOrWhiteSpace(_options.RefreshToken))
            {
                throw new InvalidOperationException(
                    "Refresh token не настроен. Выполните первоначальную авторизацию через ExchangeCodeForTokenAsync.");
            }

            var url = QueryHelpers.AddQueryString(
                ZohoEndpoints.OAuthToken,
                new Dictionary<string, string?>
                {
                    [ZohoOAuthParameters.RefreshToken] = _options.RefreshToken,
                    [ZohoOAuthParameters.ClientId] = _options.ClientId,
                    [ZohoOAuthParameters.ClientSecret] = _options.ClientSecret,
                    [ZohoOAuthParameters.GrantType] = "refresh_token"
                });

            using var response =
                await _httpClient.PostAsync(
                    url,
                    null,
                    cancellationToken);

            var json =
                await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Ошибка получения Access Token. Статус: {Status}. Ответ: {Response}",
                    response.StatusCode,
                    json);

                throw new InvalidOperationException(
                    "Не удалось получить Access Token Zoho.");
            }

            var tokenResponse =
                JsonSerializer.Deserialize<ZohoAccessTokenResponse>(
                    json,
                    JsonOptions);

            if (tokenResponse is null)
            {
                throw new InvalidOperationException(
                    "Получен пустой ответ OAuth.");
            }

            var currentToken = ZohoToken.Create(
                tokenResponse.AccessToken,
                tokenResponse.ExpiresIn);

            await _tokenStore.SaveAsync(
            currentToken,
            cancellationToken);

            _logger.LogInformation(
                "Access Token успешно обновлен. Действителен до {ExpiresAt}.",
                currentToken.ExpiresAtUtc);

            return currentToken.AccessToken;
        }
        finally
        {
            _lock.Release();
        }
    }
}