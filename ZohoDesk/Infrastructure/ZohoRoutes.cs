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
}