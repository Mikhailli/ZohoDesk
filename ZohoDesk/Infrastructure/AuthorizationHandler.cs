using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using ZohoDesk.Constants;
using ZohoDesk.Options;
using ZohoDesk.Services;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Обработчик, автоматически добавляющий OAuth-токен
/// и идентификатор организации ко всем запросам Zoho Desk.
/// </summary>
/// <remarks>
/// Создает экземпляр обработчика.
/// </remarks>
public sealed class AuthorizationHandler(
    IZohoAuthService authService,
    IOptions<ZohoDeskOptions> options) : DelegatingHandler
{
    private readonly IZohoAuthService _authService = authService;
    private readonly ZohoDeskOptions _options = options.Value;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken =
            await _authService.GetAccessTokenAsync(cancellationToken);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                ZohoHeaders.OAuthScheme,
                accessToken);

        request.Headers.Remove(ZohoHeaders.OrganizationId);

        request.Headers.Add(
            ZohoHeaders.OrganizationId,
            _options.OrganizationId);

        return await base.SendAsync(
            request,
            cancellationToken);
    }
}