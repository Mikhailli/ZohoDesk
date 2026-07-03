using System.ComponentModel.DataAnnotations;

namespace ZohoDesk.Options;

/// <summary>
/// Настройки интеграции с Zoho Desk.
/// Загружаются из секции "ZohoDesk" файла appsettings.json
/// или другого источника конфигурации.
/// </summary>
public sealed class ZohoDeskOptions
{
    /// <summary>
    /// Имя секции конфигурации.
    /// </summary>
    public const string SectionName = "ZohoDesk";

    /// <summary>
    /// Идентификатор OAuth-приложения.
    /// </summary>
    [Required]
    public required string ClientId { get; init; }

    /// <summary>
    /// Секрет OAuth-приложения.
    /// </summary>
    [Required]
    public required string ClientSecret { get; init; }

    /// <summary>
    /// Постоянный Refresh Token.
    /// Используется для получения нового Access Token.
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }

    /// <summary>
    /// Идентификатор организации в Zoho Desk.
    /// Передается в заголовке каждого запроса.
    /// </summary>
    [Required]
    public required string OrganizationId { get; init; }

    /// <summary>
    /// Базовый адрес OAuth API Zoho Accounts.
    /// </summary>
    [Required]
    [Url]
    public required string AccountsBaseUrl { get; init; }

    /// <summary>
    /// Базовый адрес API Zoho Desk.
    /// </summary>
    [Required]
    [Url]
    public required string DeskBaseUrl { get; init; }

    /// <summary>
    /// Максимальное время ожидания HTTP-запроса.
    /// </summary>
    public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Настройки повторных попыток.
    /// </summary>
    public RetryOptions Retry { get; init; } = new();

    /// <summary>
    /// Идентификатор департамента Zoho Desk.
    /// </summary>
    public required string DepartmentId { get; init; }
}
