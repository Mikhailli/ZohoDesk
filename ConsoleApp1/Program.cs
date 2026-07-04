using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZohoDesk.Constants;
using ZohoDesk.Infrastructure;
using ZohoDesk.Models;
using ZohoDesk.Services;

var builder = Host.CreateApplicationBuilder(args);

// Добавляем конфигурацию из appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Регистрируем Zoho сервисы
builder.Services.AddZohoDeskIntegration(builder.Configuration);

// Добавляем логирование в консоль
builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Debug);
});

var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("🚀 Запуск тестирования Zoho Desk модуля...");

    // Получаем сервис
    var zohoDesk = host.Services.GetRequiredService<IZohoDeskService>();

    await TestZohoDeskAsync(zohoDesk, logger);
}
catch (Exception ex)
{
    logger.LogError(ex, "❌ Тестирование завершилось ошибкой");
    throw;
}

static async Task TestZohoDeskAsync(IZohoDeskService zohoDesk, ILogger logger)
{
    // ======================================================================
    // ТЕСТ 1: Создание тикета с новым контактом
    // ======================================================================
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 1: Создание тикета с новым контактом");
    logger.LogInformation("────────────────────────────────────────────────────");

    var testTicket = await zohoDesk.CreateTicketAsync(
        new SupportTicketRequest
        {
            Email = "test.user@example.com",
            FirstName = "Тест",
            LastName = "Пользователь",
            Subject = "Тестовый тикет - Ошибка платежа",
            Description = "Детали ошибки:\n" +
                          "Транзакция: TX-12345-6789\n" +
                          "Сумма: 1000.00 RUB\n" +
                          "Ошибка: Недостаточно средств на счете\n" +
                          "Время: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Category = TicketCategories.TechnicalProblem,
            Priority = "High"
        });

    logger.LogInformation("✅ Тикет создан: ID = {TicketId}, Status = {Status}",
        testTicket.Id, testTicket.Status);
    logger.LogInformation("   Приоритет: {Priority}", testTicket.Priority);
    logger.LogInformation("   Создан: {CreatedTime}", testTicket.CreatedTime);

    // ======================================================================
    // ТЕСТ 2: Создание тикета с существующим контактом
    // ======================================================================
    await Task.Delay(1000);
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 2: Создание тикета с существующим контактом");
    logger.LogInformation("────────────────────────────────────────────────────");

    var existingTicket = await zohoDesk.CreateTicketAsync(
        new SupportTicketRequest
        {
            Email = "test.user@example.com",  // Тот же email
            FirstName = "Тест",
            LastName = "Пользователь",
            Subject = "Системная ошибка #ERR-2024-001",
            Description = "Произошла системная ошибка:\n" +
                          "Код: ERR_INTERNAL\n" +
                          "Модуль: PaymentProcessor\n" +
                          "Стек: ...",
            Category = TicketCategories.TechnicalProblem,
            Priority = "Medium"
        });

    logger.LogInformation("✅ Тикет создан: ID = {TicketId}", existingTicket.Id);

    // ======================================================================
    // ТЕСТ 3: Обновление тикета
    // ======================================================================
    await Task.Delay(1000);
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 3: Обновление тикета");
    logger.LogInformation("────────────────────────────────────────────────────");

    var updatedTicket = await zohoDesk.UpdateTicketAsync(
        existingTicket.Id,
        new SupportTicketUpdateRequest
        {
            Status = "In Progress",
            Description = "Статус обновлен автоматически. Тикет взят в работу."
        });

    logger.LogInformation("✅ Тикет обновлен: ID = {TicketId}, Новый статус = {Status}",
        updatedTicket.Id, updatedTicket.Status);

    // ======================================================================
    // ТЕСТ 4: Добавление комментария (публичный)
    // ======================================================================
    await Task.Delay(1000);
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 4: Добавление публичного комментария");
    logger.LogInformation("────────────────────────────────────────────────────");

    await zohoDesk.AddCommentAsync(
        existingTicket.Id,
        new CreateTicketCommentRequest
        {
            Content = $"<p><strong>Клиент подтвердил информацию</strong></p>" +
                      $"<p>Дата подтверждения: {DateTime.Now:dd.MM.yyyy HH:mm}</p>" +
                      $"<p>Дополнительные данные проверены</p>",
            IsPublic = true,
            ContentType = "html"
        });

    logger.LogInformation("✅ Публичный комментарий добавлен");

    // ======================================================================
    // ТЕСТ 5: Добавление комментария (приватный, @упоминание агента)
    // ======================================================================
    await Task.Delay(1000);
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 5: Добавление приватного комментария с упоминанием");
    logger.LogInformation("────────────────────────────────────────────────────");

    // Если у вас есть ZUID агента, раскомментируйте и замените на реальный
    // long agentZuid = 123456789; 
    // await zohoDesk.AddCommentAsync(
    //     existingTicket.Id,
    //     new CreateTicketCommentRequest
    //     {
    //         Content = $"zsu[@user:{agentZuid}]zsu Пожалуйста, обработайте этот тикет в приоритетном порядке",
    //         IsPublic = false,
    //         ContentType = "html"
    //     });

    logger.LogInformation("ℹ️  Комментарий с упоминанием (закомментирован, нужен реальный ZUID агента)");

    // ======================================================================
    // ТЕСТ 6: Тестирование обработки ошибок (несуществующий тикет)
    // ======================================================================
    await Task.Delay(1000);
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("📝 ТЕСТ 6: Обработка ошибки (несуществующий тикет)");
    logger.LogInformation("────────────────────────────────────────────────────");

    try
    {
        await zohoDesk.AddCommentAsync(
            "non-existent-ticket-id",
            new CreateTicketCommentRequest
            {
                Content = "Этот комментарий не должен добавиться",
                IsPublic = true,
                ContentType = "plain"
            });
    }
    catch (Exception ex)
    {
        logger.LogWarning("⚠️  Ожидаемая ошибка: {Message}", ex.Message);
        logger.LogWarning("   Ошибка корректно обработана");
    }

    // ======================================================================
    // ИТОГИ
    // ======================================================================
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("🎉 ТЕСТИРОВАНИЕ ЗАВЕРШЕНО УСПЕШНО!");
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("✅ Создано тикетов: 2");
    logger.LogInformation("✅ Обновлено тикетов: 1");
    logger.LogInformation("✅ Добавлено комментариев: 1");
    logger.LogInformation("✅ Проверена обработка ошибок");
    logger.LogInformation("────────────────────────────────────────────────────");
    logger.LogInformation("Проверьте созданные тикеты в Zoho Desk:");
    logger.LogInformation("   - Тикет 1: {TicketId1}", testTicket.Id);
    logger.LogInformation("   - Тикет 2: {TicketId2}", existingTicket.Id);
    logger.LogInformation("────────────────────────────────────────────────────");
}