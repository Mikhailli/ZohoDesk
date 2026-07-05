using ZohoDesk.Models;

namespace ZohoDesk.Services;

/// <summary>
/// Параметры поиска контактов.
/// </summary>
public sealed class ContactsSearchParameters
{
    /// <summary>
    /// Email контакта.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Телефон контакта.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Имя контакта.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Фамилия контакта.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Полное имя контакта.
    /// </summary>
    public string? FullName { get; init; }

    /// <summary>
    /// Мобильный телефон контакта.
    /// </summary>
    public string? Mobile { get; init; }

    /// <summary>
    /// Название аккаунта.
    /// </summary>
    public string? AccountName { get; init; }

    /// <summary>
    /// Количество результатов (1-100).
    /// По умолчанию: 10.
    /// </summary>
    public int Limit { get; init; } = 10;

    /// <summary>
    /// Начальный индекс для выборки (0-4999).
    /// По умолчанию: 0.
    /// </summary>
    public int From { get; init; } = 0;

    /// <summary>
    /// Поле для сортировки.
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    /// Порядок сортировки (true - по возрастанию, false - по убыванию).
    /// </summary>
    public bool SortAscending { get; init; } = true;
}

/// <summary>
/// Клиент для работы с контактами Zoho Desk.
/// </summary>
public interface IContactsClient
{
    /// <summary>
    /// Ищет контакты по адресу электронной почты.
    /// </summary>
    /// <param name="email">
    /// Email контакта.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Результат поиска с списком найденных контактов.
    /// </returns>
    Task<ContactsSearchResult?> SearchByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ищет первый контакт по адресу электронной почты.
    /// Удобный метод для получения единственного контакта.
    /// </summary>
    /// <param name="email">
    /// Email контакта.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Первый найденный контакт или <c>null</c>, если контакт отсутствует.
    /// </returns>
    Task<Contact?> FindFirstByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет расширенный поиск контактов.
    /// </summary>
    /// <param name="parameters">
    /// Параметры поиска.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Результат поиска с списком найденных контактов.
    /// </returns>
    Task<ContactsSearchResult?> SearchAsync(
        ContactsSearchParameters parameters,
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