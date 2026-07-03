namespace ZohoDesk.Constants;

/// <summary>
/// URL-адреса эндпоинтов Zoho Desk API.
/// </summary>
public static class ZohoEndpoints
{
    /// <summary>
    /// Эндпоинт получения Access Token по Refresh Token.
    /// </summary>
    public const string OAuthToken = "/oauth/v2/token";

    /// <summary>
    /// Эндпоинт работы с контактами.
    /// </summary>
    public const string Contacts = "/api/v1/contacts";

    /// <summary>
    /// Эндпоинт поиска контактов.
    /// </summary>
    public const string SearchContacts = "/api/v1/contacts/search";

    /// <summary>
    /// Эндпоинт работы с тикетами.
    /// </summary>
    public const string Tickets = "/api/v1/tickets";

    /// <summary>
    /// Возвращает эндпоинт конкретного тикета.
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета.</param>
    public static string Ticket(string ticketId)
        => $"{Tickets}/{ticketId}";

    /// <summary>
    /// Возвращает эндпоинт комментариев тикета.
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета.</param>
    public static string TicketComments(string ticketId)
        => $"{Tickets}/{ticketId}/message";

    /// <summary>
    /// Возвращает эндпоинт поиска контакта по электронной почте.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    public static string SearchContactByEmail(string email)
        => $"{SearchContacts}?email={Uri.EscapeDataString(email)}";
}
