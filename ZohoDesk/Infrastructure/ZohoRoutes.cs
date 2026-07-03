using System.Text;
using ZohoDesk.Clients;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Формирует относительные URL для API Zoho Desk.
/// </summary>
public static class ZohoRoutes
{
    private const string ApiVersion = "api/v1";

    /// <summary>
    /// URL коллекции контактов.
    /// </summary>
    public static string Contacts()
        => $"{ApiVersion}/contacts";

    /// <summary>
    /// URL поиска контакта по email.
    /// </summary>
    /// <param name="email">
    /// Email контакта.
    /// </param>
    public static string SearchContact(string email)
        => $"{ApiVersion}/contacts/search?email={Uri.EscapeDataString(email)}";

    /// <summary>
    /// URL расширенного поиска контактов.
    /// </summary>
    /// <param name="parameters">
    /// Параметры поиска.
    /// </param>
    public static string SearchContacts(ContactsSearchParameters parameters)
    {
        var queryBuilder = new StringBuilder($"{ApiVersion}/contacts/search?");
        var hasParams = false;

        if (!string.IsNullOrWhiteSpace(parameters.Email))
        {
            queryBuilder.Append($"email={Uri.EscapeDataString(parameters.Email)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.Phone))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"phone={Uri.EscapeDataString(parameters.Phone)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.FirstName))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"firstName={Uri.EscapeDataString(parameters.FirstName)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.LastName))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"lastName={Uri.EscapeDataString(parameters.LastName)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.FullName))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"fullName={Uri.EscapeDataString(parameters.FullName)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.Mobile))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"mobile={Uri.EscapeDataString(parameters.Mobile)}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.AccountName))
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"accountName={Uri.EscapeDataString(parameters.AccountName)}");
            hasParams = true;
        }

        if (parameters.Limit > 0)
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"limit={parameters.Limit}");
            hasParams = true;
        }

        if (parameters.From > 0)
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"from={parameters.From}");
            hasParams = true;
        }

        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            if (hasParams) queryBuilder.Append('&');
            var sortPrefix = parameters.SortAscending ? "" : "-";
            queryBuilder.Append($"sortBy={sortPrefix}{Uri.EscapeDataString(parameters.SortBy)}");
        }

        return queryBuilder.ToString();
    }

    /// <summary>
    /// URL коллекции тикетов.
    /// </summary>
    public static string Tickets()
        => $"{ApiVersion}/tickets";

    /// <summary>
    /// URL конкретного тикета.
    /// </summary>
    /// <param name="ticketId">
    /// Идентификатор тикета.
    /// </param>
    public static string Ticket(string ticketId)
        => $"{ApiVersion}/tickets/{Uri.EscapeDataString(ticketId)}";

    /// <summary>
    /// URL комментариев тикета.
    /// </summary>
    /// <param name="ticketId">
    /// Идентификатор тикета.
    /// </param>
    public static string TicketComments(string ticketId)
        => $"{ApiVersion}/tickets/{Uri.EscapeDataString(ticketId)}/comments";
}