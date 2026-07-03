using ZohoDesk.DTO;
using ZohoDesk.Models;

namespace ZohoDesk.Services;

/// <summary>
/// Сервис для работы с обращениями в Zoho Desk.
/// </summary>
public interface IZohoDeskService
{
    /// <summary>
    /// Создает новое обращение.
    /// </summary>
    Task<Ticket> CreateTicketAsync(
        SupportTicketRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет существующее обращение.
    /// </summary>
    Task<Ticket> UpdateTicketAsync(
        string ticketId,
        SupportTicketUpdateRequest request,
        CancellationToken cancellationToken = default);
}