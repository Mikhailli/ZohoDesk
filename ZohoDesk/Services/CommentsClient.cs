using System.Net.Http.Json;
using ZohoDesk.Infrastructure;
using ZohoDesk.Models;

namespace ZohoDesk.Services;

/// <summary>
/// Клиент для работы с комментариями к тикетам Zoho Desk.
/// </summary>
/// <remarks>
/// Создает экземпляр клиента для работы с комментариями.
/// </remarks>
/// <param name="apiClient">
/// Универсальный клиент API Zoho Desk.
/// </param>
public sealed class CommentsClient(
    IZohoApiClient apiClient) : ICommentsClient
{
    private readonly IZohoApiClient _apiClient = apiClient;

    public Task<TicketComment?> CreateAsync(
        string ticketId,
        CreateTicketCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ticketId);
        ArgumentNullException.ThrowIfNull(request);

        return _apiClient.SendAsync<TicketComment>(
            () =>
            {
                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    ZohoRoutes.TicketComments(ticketId))
                {
                    Content = JsonContent.Create(
                        request,
                        options: JsonOptionsProvider.Default)
                };

                return httpRequest;
            },
            cancellationToken);
    }
}
