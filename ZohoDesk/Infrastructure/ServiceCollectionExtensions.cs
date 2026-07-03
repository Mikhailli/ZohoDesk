using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZohoDesk.Authentication;
using ZohoDesk.Clients;
using ZohoDesk.Options;
using ZohoDesk.Services;

namespace ZohoDesk.Infrastructure;

/// <summary>
/// Методы расширения для регистрации сервисов Zoho Desk.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы интеграции с Zoho Desk.
    /// </summary>
    /// <param name="services">
    /// Коллекция сервисов.
    /// </param>
    /// <param name="configuration">
    /// Конфигурация приложения.
    /// </param>
    /// <returns>
    /// Коллекция сервисов.
    /// </returns>
    public static IServiceCollection AddZohoDeskIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ZohoDeskOptions>(
            configuration.GetSection(ZohoDeskOptions.SectionName));

        services.AddHttpClient<IZohoApiClient, ZohoApiClient>();

        services.AddScoped<IContactsClient, ContactsClient>();

        services.AddScoped<ITicketsClient, TicketsClient>();

        services.AddScoped<IZohoDeskService, ZohoDeskService>();

        services.AddDistributedMemoryCache();

        services.AddSingleton<IZohoTokenStore, DistributedZohoTokenStore>();

        return services;
    }
}