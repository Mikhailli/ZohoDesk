using System.Net.Http.Json;
using ZohoDesk.DTO;
using ZohoDesk.Infrastructure;

namespace ZohoDesk.Clients;

/// <summary>
/// Клиент для работы с тикетами Zoho Desk.
/// </summary>
/// <remarks>
/// Создает экземпляр клиента для работы с тикетами.
/// </remarks>
/// <param name="apiClient">
/// Универсальный клиент API Zoho Desk.
/// </param>
public sealed class TicketsClient(
    IZohoApiClient apiClient) : ITicketsClient
{
    private readonly IZohoApiClient _apiClient = apiClient;

    public Task<Ticket?> CreateAsync(
        CreateTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _apiClient.SendAsync<Ticket>(
            () =>
            {
                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    ZohoRoutes.Tickets());

                httpRequest.Content = JsonContent.Create(
                    request,
                    options: JsonOptionsProvider.Default);

                return httpRequest;
            },
            cancellationToken);
    }

    public Task<Ticket?> UpdateAsync(
        string ticketId,
        UpdateTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ticketId);
        ArgumentNullException.ThrowIfNull(request);

        return _apiClient.SendAsync<Ticket>(
            () =>
            {
                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Patch,
                    ZohoRoutes.Ticket(ticketId));

                httpRequest.Content = JsonContent.Create(
                    request,
                    options: JsonOptionsProvider.Default);

                return httpRequest;
            },
            cancellationToken);
    }
}