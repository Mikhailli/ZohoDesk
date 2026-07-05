using ZohoDesk.Models;

namespace ZohoDesk.Services;

/// <summary>
/// Клиент для работы с тикетами Zoho Desk.
/// </summary>
public interface ITicketsClient
{
    /// <summary>
    /// Создает новый тикет.
    /// </summary>
    Task<Ticket?> CreateAsync(
        CreateTicketRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет существующий тикет.
    /// </summary>
    Task<Ticket?> UpdateAsync(
        string ticketId,
        UpdateTicketRequest request,
        CancellationToken cancellationToken = default);
}