using System.Net.Http.Json;
using ZohoDesk.Infrastructure;
using ZohoDesk.Models;

namespace ZohoDesk.Services;

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

    public Task<ContactsSearchResult?> SearchByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return _apiClient.SendAsync<ContactsSearchResult>(
            () =>
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    ZohoRoutes.SearchContact(email));

                return request;
            },
            cancellationToken);
    }

    public async Task<Contact?> FindFirstByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var result = await SearchByEmailAsync(email, cancellationToken);

        return result?.Data?.FirstOrDefault();
    }

    public Task<ContactsSearchResult?> SearchAsync(
        ContactsSearchParameters parameters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return _apiClient.SendAsync<ContactsSearchResult>(
            () =>
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    ZohoRoutes.SearchContacts(parameters));

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
                    ZohoRoutes.Contacts())
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