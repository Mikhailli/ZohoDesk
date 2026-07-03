using ZohoDesk.DTO;

namespace ZohoDesk.Clients;

/// <summary>
/// Клиент для работы с комментариями к тикетам Zoho Desk.
/// </summary>
public interface ICommentsClient
{
    /// <summary>
    /// Создает комментарий к тикету.
    /// </summary>
    /// <param name="ticketId">
    /// Идентификатор тикета.
    /// </param>
    /// <param name="request">
    /// Данные комментария.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Созданный комментарий.
    /// </returns>
    Task<TicketComment?> CreateAsync(
        string ticketId,
        CreateTicketCommentRequest request,
        CancellationToken cancellationToken = default);
}
