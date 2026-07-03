using System.Net.Http.Json;
using ZohoDesk.DTO;
using ZohoDesk.Infrastructure;

namespace ZohoDesk.Clients;

/// <summary>
/// Клиент для работы с контактами Zoho Desk.
/// </summary>
/// <remarks>
/// Создает экземпляр клиента контактов.
/// </remarks>
/// <param name="apiClient">
/// Универсальный API-клиент Zoho Desk.
/// </param>
public sealed class ContactsClient(
    IZohoApiClient apiClient) : IContactsClient
{
    private readonly IZohoApiClient _apiClient = apiClient;

    public Task<Contact?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return _apiClient.SendAsync<Contact>(
            () =>
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    ZohoRoutes.SearchContact(email));

                return request;
            },
            cancellationToken);
    }

    public Task<Contact?> CreateAsync(
        CreateContactRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _apiClient.SendAsync<Contact>(
            () =>
            {
                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    ZohoRoutes.Contacts());

                httpRequest.Content = JsonContent.Create(
                    request,
                    options: JsonOptionsProvider.Default);

                return httpRequest;
            },
            cancellationToken);
    }
}