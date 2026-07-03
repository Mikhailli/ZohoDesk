using ZohoDesk.DTO;

namespace ZohoDesk.Clients;

/// <summary>
/// Клиент для работы с контактами Zoho Desk.
/// </summary>
public interface IContactsClient
{
    /// <summary>
    /// Ищет контакт по адресу электронной почты.
    /// </summary>
    /// <param name="email">
    /// Email контакта.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Найденный контакт или <c>null</c>, если контакт отсутствует.
    /// </returns>
    Task<Contact?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Создает новый контакт.
    /// </summary>
    /// <param name="request">
    /// Данные нового контакта.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Созданный контакт.
    /// </returns>
    Task<Contact?> CreateAsync(
        CreateContactRequest request,
        CancellationToken cancellationToken = default);
}