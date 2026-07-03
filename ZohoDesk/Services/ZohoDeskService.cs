using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZohoDesk.Clients;
using ZohoDesk.DTO;
using ZohoDesk.Models;
using ZohoDesk.Options;

namespace ZohoDesk.Services;

/// <summary>
/// Сервис для работы с обращениями Zoho Desk.
/// </summary>
public sealed class ZohoDeskService : IZohoDeskService
{
    private readonly IContactsClient _contactsClient;
    private readonly ITicketsClient _ticketsClient;
    private readonly ICommentsClient _commentsClient;
    private readonly ZohoDeskOptions _options;
    private readonly ILogger<ZohoDeskService> _logger;

    /// <summary>
    /// Создает экземпляр сервиса.
    /// </summary>
    public ZohoDeskService(
        IContactsClient contactsClient,
        ITicketsClient ticketsClient,
        ICommentsClient commentsClient,
        IOptions<ZohoDeskOptions> options,
        ILogger<ZohoDeskService> logger)
    {
        _contactsClient = contactsClient;
        _ticketsClient = ticketsClient;
        _commentsClient = commentsClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<Ticket> CreateTicketAsync(
        SupportTicketRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation(
            "Создание обращения для пользователя {Email}.",
            request.Email);

        // Ищем контакт
        var contact = await _contactsClient.FindFirstByEmailAsync(
            request.Email,
            cancellationToken);

        // Если контакт отсутствует — создаем его
        if (contact is null)
        {
            _logger.LogInformation(
                "Контакт {Email} не найден. Создаем новый.",
                request.Email);

            contact = await _contactsClient.CreateAsync(
                new CreateContactRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone
                },
                cancellationToken);

            if (contact is null)
            {
                throw new InvalidOperationException(
                    "Не удалось создать контакт Zoho Desk.");
            }
        }

        // Создаем тикет
        var ticket = await _ticketsClient.CreateAsync(
            new CreateTicketRequest
            {
                Subject = request.Subject,
                Description = request.Description,
                ContactId = contact.Id,
                DepartmentId = _options.DepartmentId,
                Priority = request.Priority,
                Category = request.Category,
                SubCategory = request.SubCategory,
                CustomFields = request.CustomFields
            },
            cancellationToken);

        if (ticket is null)
        {
            throw new InvalidOperationException(
                "Zoho Desk не вернул созданный тикет.");
        }

        _logger.LogInformation(
            "Тикет {TicketId} успешно создан.",
            ticket.Id);

        return ticket;
    }

    public async Task<Ticket> UpdateTicketAsync(
        string ticketId,
        SupportTicketUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ticketId);
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation(
            "Обновление тикета {TicketId}.",
            ticketId);

        var ticket = await _ticketsClient.UpdateAsync(
            ticketId,
            new UpdateTicketRequest
            {
                Status = request.Status,
                Priority = request.Priority,
                Category = request.Category,
                SubCategory = request.SubCategory,
                Description = request.Description,
                CustomFields = request.CustomFields
            },
            cancellationToken);

        if (ticket is null)
        {
            throw new InvalidOperationException(
                $"Не удалось обновить тикет '{ticketId}'.");
        }

        _logger.LogInformation(
            "Тикет {TicketId} успешно обновлен.",
            ticket.Id);

        return ticket;
    }

    public async Task<TicketComment> AddCommentAsync(
        string ticketId,
        CreateTicketCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ticketId);
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation(
            "Добавление комментария к тикету {TicketId}.",
            ticketId);

        var comment = await _commentsClient.CreateAsync(
            ticketId,
            request,
            cancellationToken);

        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Не удалось добавить комментарий к тикету '{ticketId}'.");
        }

        _logger.LogInformation(
            "Комментарий {CommentId} успешно добавлен к тикету {TicketId}.",
            comment.Id,
            ticketId);

        return comment;
    }
}