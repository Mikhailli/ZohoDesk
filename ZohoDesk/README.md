# Zoho Desk Integration Library

Библиотека для интеграции с Zoho Desk API на C#.

## Содержание

- [Требования](#требования)
- [Установка](#установка)
- [Настройка OAuth 2.0](#настройка-oauth-20)
- [Конфигурация](#конфигурация)
- [Использование](#использование)
- [Архитектура](#архитектура)
- [Соответствие требованиям](#соответствие-требованиям)
- [Тестирование](#тестирование)

## Требования

- .NET 8.0 или выше
- Zoho Desk аккаунт с API доступом
- OAuth 2.0 учетные данные (Client ID, Client Secret, Refresh Token, Org ID)

## Установка

1. Добавьте проект в решение или скопируйте папку `ZohoDesk`
2. Добавьте ссылку на проект в ваш `csproj` файл:

```xml
<ProjectReference Include="..\ZohoDesk\ZohoDesk.csproj" />
```

3. Установите необходимые NuGet пакеты (они установятся автоматически):
   - Microsoft.Extensions.Http
   - Microsoft.Extensions.Options
   - Microsoft.Extensions.DependencyInjection.Abstractions

## Настройка OAuth 2.0

### Шаг 1: Создание Self Client в Zoho Developer Console

Для backend-сервисов без взаимодействия с пользователем используйте **Self Client**:

1. Перейдите на [Zoho API Console](https://api-console.zoho.com/)
2. Выберите **Self Client** → **Create Now**
3. Скопируйте `Client ID` и `Client Secret`

### Шаг 2: Генерация Grant Token

1. В той же консоли перейдите во вкладку **Generate Code**
2. Введите scopes (через запятую):
```
Desk.tickets.READ,Desk.tickets.WRITE,Desk.tickets.UPDATE,Desk.contacts.READ,Desk.contacts.WRITE,Desk.search.READ,Desk.basic.READ
```
3. Выберите время действия (например, 10 минут)
4. Введите описание
5. Нажмите **Generate** → скопируйте grant token

### Шаг 3: Обмен Grant Token на Access/Refresh Tokens

**Вариант A: Через код (рекомендуется)**

```csharp
public class TokenSetupService
{
    private readonly IZohoAuthService _authService;
    private readonly ILogger<TokenSetupService> _logger;

    public TokenSetupService(IZohoAuthService authService, ILogger<TokenSetupService> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task SetupTokensAsync(string grantToken)
    {
        var response = await _authService.ExchangeCodeForTokenAsync(grantToken);
        
        _logger.LogInformation(
            "Access Token: {AccessToken}\nRefresh Token: {RefreshToken}\nExpires In: {ExpiresIn} seconds",
            response.AccessToken,
            response.RefreshToken,
            response.ExpiresIn);

        // Сохраните refresh_token в appsettings.json!
        Console.WriteLine($"Refresh Token: {response.RefreshToken}");
    }
}
```

**Вариант B: Через curl**

```bash
curl -X POST "https://accounts.zoho.com/oauth/v2/token" \
  -d "code=ВАШ_GRANT_TOKEN" \
  -d "client_id=ВАШ_CLIENT_ID" \
  -d "client_secret=ВАШ_CLIENT_SECRET" \
  -d "redirect_uri=https://selfclient" \
  -d "grant_type=authorization_code"
```

Ответ:
```json
{
  "access_token": "1000.abc...",
  "refresh_token": "1000.xyz...",
  "expires_in": 3600,
  "token_type": "Bearer"
}
```

### Шаг 4: Сохранение Refresh Token

Добавьте полученный refresh token в конфигурацию:

```json
{
  "ZohoDesk": {
    "ClientId": "ваш_client_id",
    "ClientSecret": "ваш_client_secret",
    "RefreshToken": "полученный_refresh_token",
    "OrganizationId": "ваш_org_id",
    "DepartmentId": "идентификатор_отдела"
  }
}
```

**Важно:** Refresh token бессрочный и не меняется при обновлении access token.

## Конфигурация

### Полная конфигурация

```json
{
  "ZohoDesk": {
    "ClientId": "ваш_client_id",
    "ClientSecret": "ваш_client_secret",
    "RefreshToken": "ваш_refresh_token",
    "OrganizationId": "ваш_org_id",
    "DepartmentId": "идентификатор_отдела",
    "AccountsBaseUrl": "https://accounts.zoho.com",
    "DeskBaseUrl": "https://desk.zoho.com",
    "RedirectUri": "https://selfclient",
    "RequestTimeout": "00:00:30",
    "Retry": {
      "Enabled": true,
      "Delays": ["00:00:05", "00:00:30", "00:02:00"]
    }
  }
}
```

### Регистрация сервисов

```csharp
using ZohoDesk.Infrastructure;

builder.Services.AddZohoDeskIntegration(builder.Configuration);
```

## Использование

### Внедрение зависимости

```csharp
public class MyService
{
    private readonly IZohoDeskService _zohoDesk;

    public MyService(IZohoDeskService zohoDesk)
    {
        _zohoDesk = zohoDesk;
    }
}
```

### Создание тикета при ошибке платежа

```csharp
using ZohoDesk.Constants;
using ZohoDesk.Models;

public class PaymentService
{
    private readonly IZohoDeskService _zohoDesk;
    private readonly ILogger<PaymentService> _logger;

    public async Task HandlePaymentError(PaymentErrorEvent error)
    {
        try
        {
            var ticket = await _zohoDesk.CreateTicketAsync(
                new SupportTicketRequest
                {
                    Email = error.UserEmail,
                    FirstName = error.FirstName,
                    LastName = error.LastName,
                    Subject = $"Ошибка платежа: {error.TransactionId}",
                    Description = $"Детали ошибки:\n" +
                                  $"Транзакция: {error.TransactionId}\n" +
                                  $"Сумма: {error.Amount}\n" +
                                  $"Ошибка: {error.ErrorMessage}",
                    Category = TicketCategories.TechnicalProblem,
                    Priority = "High"
                });

            _logger.LogInformation(
                "Создан тикет {TicketId} для ошибки платежа {TransactionId}",
                ticket.Id,
                error.TransactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка создания тикета для ошибки платежа");
        }
    }
}
```

### Создание тикета при системной ошибке

```csharp
public class SystemErrorService
{
    private readonly IZohoDeskService _zohoDesk;

    public async Task HandleSystemError(SystemErrorEvent error)
    {
        await _zohoDesk.CreateTicketAsync(
            new SupportTicketRequest
            {
                Email = error.UserEmail ?? "system@company.com",
                FirstName = error.FirstName ?? "System",
                LastName = error.LastName ?? "Error",
                Subject = $"Системная ошибка: {error.Code}",
                Description = error.Details,
                Priority = "Medium"
            });
    }
}
```

### Обновление тикета

```csharp
public async Task UpdateTicketStatus(string ticketId, string newStatus)
{
    var ticket = await _zohoDesk.UpdateTicketAsync(
        ticketId,
        new SupportTicketUpdateRequest
        {
            Status = newStatus,
            Description = "Статус обновлен автоматически"
        });
}
```

### Добавление комментария

```csharp
public async Task AddCommentToTicket(string ticketId, string comment)
{
    await _zohoDesk.AddCommentAsync(
        ticketId,
        new CreateTicketCommentRequest
        {
            Content = comment,
            IsPublic = true,
            ContentType = "html"
        });
}
```

### Упоминание агента в комментарии

```csharp
public async Task AssignToAgent(string ticketId, long agentZuid, string agentName)
{
    await _zohoDesk.AddCommentAsync(
        ticketId,
        new CreateTicketCommentRequest
        {
            Content = $"zsu[@user:{agentZuid}]zsu Пожалуйста, обработайте этот тикет",
            IsPublic = false,
            ContentType = "html"
        });
}
```

## Архитектура

### Структура проекта

```
ZohoDesk/
├── Authentication/              # OAuth 2.0 аутентификация
│   ├── DistributedZohoTokenStore.cs  # Хранилище токенов (distributed cache)
│   ├── IZohoTokenStore.cs            # Интерфейс хранилища
│   └── RefreshAccessTokenRequest.cs  # Модель запроса обновления токена
│
├── Clients/                     # API клиенты
│   ├── IZohoApiClient.cs            # Универсальный HTTP клиент
│   ├── ZohoApiClient.cs             # Реализация с retry логикой
│   ├── IContactsClient.cs           # Клиент контактов
│   ├── ContactsClient.cs            # Реализация клиента контактов
│   ├── ITicketsClient.cs            # Клиент тикетов
│   ├── TicketsClient.cs             # Реализация клиента тикетов
│   ├── ICommentsClient.cs           # Клиент комментариев
│   └── CommentsClient.cs            # Реализация клиента комментариев
│
├── Constants/                   # Константы
│   ├── TicketCategories.cs          # Стандартные категории тикетов
│   ├── ZohoContentTypes.cs          # Content-Type заголовки
│   ├── ZohoEndpoints.cs             # OAuth endpoints
│   ├── ZohoHeaders.cs               # HTTP заголовки
│   └── ZohoOAuthParameters.cs       # OAuth параметры
│
├── DTO/                         # Data Transfer Objects
│   ├── Contact.cs                   # Модель контакта
│   ├── ContactsSearchResult.cs      # Результат поиска контактов
│   ├── CreateContactRequest.cs      # Запрос создания контакта
│   ├── Ticket.cs                    # Модель тикета
│   ├── CreateTicketRequest.cs       # Запрос создания тикета
│   ├── UpdateTicketRequest.cs       # Запрос обновления тикета
│   ├── TicketComment.cs             # Модель комментария
│   ├── CreateTicketCommentRequest.cs # Запрос создания комментария
│   ├── ZohoAccessTokenResponse.cs   # Ответ OAuth токена
│   └── ZohoErrorResponse.cs         # Модель ошибки API
│
├── Exceptions/                  # Исключения
│   ├── ZohoException.cs             # Базовое исключение
│   ├── ZohoApiException.cs          # Ошибка API
│   └── ZohoAuthenticationException.cs # Ошибка аутентификации
│
├── Infrastructure/              # Инфраструктура
│   ├── HttpResponseMessageExtensions.cs # Методы расширения для ответов
│   ├── JsonOptionsProvider.cs       # Настройки JSON сериализации
│   ├── ServiceCollectionExtensions.cs # Регистрация DI
│   └── ZohoRoutes.cs                # Формирование URL маршрутов
│
├── Models/                      # Модели для внешнего использования
│   ├── SupportTicketRequest.cs      # Запрос создания тикета
│   ├── SupportTicketUpdateRequest.cs # Запрос обновления тикета
│   └── ZohoToken.cs                 # Модель токена
│
├── Options/                     # Конфигурация
│   ├── ZohoDeskOptions.cs           # Основные настройки
│   └── RetryOptions.cs              # Настройки retry
│
└── Services/                    # Бизнес-логика
    ├── IZohoAuthService.cs          # Интерфейс сервиса аутентификации
    ├── ZohoAuthService.cs           # Получение и обновление токенов
    ├── IZohoDeskService.cs          # Интерфейс основного сервиса
    ├── ZohoDeskService.cs           # Основной сервис для работы с тикетами
    ├── INotificationService.cs      # Интерфейс уведомлений
    └── LogNotificationService.cs    # Логирование критических ошибок
```

### Описание классов

#### Authentication

| Класс | Описание |
|-------|----------|
| [`DistributedZohoTokenStore`](ZohoDesk/Authentication/DistributedZohoTokenStore.cs) | Хранит токены в распределенном кэше с шифрованием. Использует `IDistributedCache`. |
| [`IZohoTokenStore`](ZohoDesk/Authentication/IZohoTokenStore.cs) | Интерфейс для хранения токенов. Позволяет заменить реализацию. |

#### Clients

| Класс | Описание |
|-------|----------|
| [`ZohoApiClient`](ZohoDesk/Clients/ZohoApiClient.cs) | Универсальный HTTP клиент. Выполняет запросы с автоматическим обновлением токена и retry логикой. |
| [`ContactsClient`](ZohoDesk/Clients/ContactsClient.cs) | Клиент для работы с контактами: поиск, создание. |
| [`TicketsClient`](ZohoDesk/Clients/TicketsClient.cs) | Клиент для работы с тикетами: создание, обновление. |
| [`CommentsClient`](ZohoDesk/Clients/CommentsClient.cs) | Клиент для работы с комментариями: создание. |

#### Services

| Класс | Описание |
|-------|----------|
| [`ZohoAuthService`](ZohoDesk/Services/ZohoAuthService.cs) | Управляет OAuth токенами: получение, обновление, кэширование. |
| [`ZohoDeskService`](ZohoDesk/Services/ZohoDeskService.cs) | Основной сервис для внешнего использования. Создает/обновляет тикеты, добавляет комментарии. |
| [`LogNotificationService`](ZohoDesk/Services/LogNotificationService.cs) | Логирует критические ошибки после исчерпания попыток. |

## Соответствие требованиям

### 1. OAuth 2.0 авторизация ✅

| Требование | Реализация | Файл |
|------------|------------|------|
| Получение access_token и refresh_token | `ZohoAuthService.GetAccessTokenAsync` | [`ZohoAuthService.cs:35-47`](ZohoDesk/Services/ZohoAuthService.cs:35) |
| Хранение токенов в зашифрованном виде | `DistributedZohoTokenStore` с Data Protection | [`DistributedZohoTokenStore.cs`](ZohoDesk/Authentication/DistributedZohoTokenStore.cs) |
| Автоматическое обновление при 401 | `ZohoApiClient.ExecuteAsync` | [`ZohoApiClient.cs:96-110`](ZohoDesk/Clients/ZohoApiClient.cs:96) |

### 2. API-клиент ✅

| Операция | Метод | URL | Файл |
|----------|-------|-----|------|
| Создание контакта | `POST /api/v1/contacts` | `ContactsClient.CreateAsync` | [`ContactsClient.cs:39-59`](ZohoDesk/Clients/ContactsClient.cs:39) |
| Поиск контакта по email | `GET /api/v1/contacts/search?email=...` | `ContactsClient.SearchByEmailAsync` | [`ContactsClient.cs:21-37`](ZohoDesk/Clients/ContactsClient.cs:21) |
| Создание тикета | `POST /api/v1/tickets` | `TicketsClient.CreateAsync` | [`TicketsClient.cs:21-41`](ZohoDesk/Clients/TicketsClient.cs:21) |
| Обновление тикета | `PATCH /api/v1/tickets/{id}` | `TicketsClient.UpdateAsync` | [`TicketsClient.cs:43-65`](ZohoDesk/Clients/TicketsClient.cs:43) |
| Добавление комментария | `POST /api/v1/tickets/{id}/comments` | `CommentsClient.CreateAsync` | [`CommentsClient.cs:21-41`](ZohoDesk/Clients/CommentsClient.cs:21) |

### 3. Обработка событий ✅

| Событие | Действие | Пример использования |
|---------|----------|---------------------|
| Ошибка платежа | Создать тикет с категорией "Техническая проблема" | `Category = TicketCategories.TechnicalProblem` |
| Системная ошибка | Создать тикет | `CreateTicketAsync` с нужными параметрами |
| Обновление статуса | Обновить тикет | `UpdateTicketAsync` |

### 4. Логирование ✅

| Требование | Реализация | Файл |
|------------|------------|------|
| Время запроса | `LogInformation` с timestamp | [`ZohoApiClient.cs:80-83`](ZohoDesk/Clients/ZohoApiClient.cs:80) |
| URL и метод | `LogInformation` | [`ZohoApiClient.cs:80-83`](ZohoDesk/Clients/ZohoApiClient.cs:80) |
| Статус ответа | `LogInformation` | [`ZohoApiClient.cs:89-93`](ZohoDesk/Clients/ZohoApiClient.cs:89) |
| Текст ошибки | `LogError` в `LogNotificationService` | [`LogNotificationService.cs:25-35`](ZohoDesk/Services/LogNotificationService.cs:25) |

### 5. Обработка ошибок ✅

| Код ответа | Действие | Файл |
|------------|----------|------|
| 401 Unauthorized | Обновить токен, повторить 1 раз | [`ZohoApiClient.cs:96-110`](ZohoDesk/Clients/ZohoApiClient.cs:96) |
| 429 Too Many Requests | Повторить через 5, 30, 120 сек | [`ZohoApiClient.cs:114-132`](ZohoDesk/Clients/ZohoApiClient.cs:114) |
| 5xx Server Error | Повторить через 5, 30, 120 сек | [`ZohoApiClient.cs:114-132`](ZohoDesk/Clients/ZohoApiClient.cs:114) |
| 3 неудачи | Уведомить разработчика (в лог) | [`ZohoApiClient.cs:134-142`](ZohoDesk/Clients/ZohoApiClient.cs:134) |

### 6. Технические требования ✅

| Требование | Реализация |
|------------|------------|
| HttpClient | `IHttpClientFactory` через `AddHttpClient` |
| JSON | `System.Text.Json` с camelCase |
| Конфигурация | `appsettings.json` + `IOptions<ZohoDeskOptions>` |

## Тестирование

### Unit Tests

Создайте тестовый проект:

```bash
dotnet new xunit -n ZohoDesk.Tests
dotnet add ZohoDesk.Tests reference ZohoDesk/ZohoDesk.csproj
dotnet add ZohoDesk.Tests package Moq
dotnet add ZohoDesk.Tests package FluentAssertions
```

Пример теста:

```csharp
public class ZohoDeskServiceTests
{
    [Fact]
    public async Task CreateTicketAsync_ShouldCreateContact_WhenNotExists()
    {
        // Arrange
        var contactsClient = new Mock<IContactsClient>();
        var ticketsClient = new Mock<ITicketsClient>();
        var commentsClient = new Mock<ICommentsClient>();
        var options = Options.Create(new ZohoDeskOptions { DepartmentId = "123" });
        var logger = new Mock<ILogger<ZohoDeskService>>();

        contactsClient
            .Setup(x => x.FindFirstByEmailAsync("test@example.com", default))
            .ReturnsAsync((Contact?)null);

        contactsClient
            .Setup(x => x.CreateAsync(It.IsAny<CreateContactRequest>(), default))
            .ReturnsAsync(new Contact { Id = "contact-1" });

        ticketsClient
            .Setup(x => x.CreateAsync(It.IsAny<CreateTicketRequest>(), default))
            .ReturnsAsync(new Ticket { Id = "ticket-1" });

        var service = new ZohoDeskService(
            contactsClient.Object,
            ticketsClient.Object,
            commentsClient.Object,
            options,
            logger.Object);

        var request = new SupportTicketRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Subject = "Test",
            Description = "Test description"
        };

        // Act
        var ticket = await service.CreateTicketAsync(request);

        // Assert
        ticket.Should().NotBeNull();
        ticket.Id.Should().Be("ticket-1");
        contactsClient.Verify(x => x.CreateAsync(It.IsAny<CreateContactRequest>(), default), Times.Once);
    }
}
```

### Integration Tests

Для интеграционных тестов используйте Zoho Desk Sandbox:

```csharp
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullWorkflow_ShouldWork()
    {
        // 1. Создать тикет
        var createRequest = new
        {
            email = "test@example.com",
            firstName = "Test",
            lastName = "User",
            subject = "Integration Test",
            description = "Test description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/tickets", createRequest);
        createResponse.EnsureSuccessStatusCode();

        var ticket = await createResponse.Content.ReadFromJsonAsync<Ticket>();

        // 2. Добавить комментарий
        var commentRequest = new
        {
            content = "Test comment",
            isPublic = true
        };

        var commentResponse = await _client.PostAsJsonAsync(
            $"/api/tickets/{ticket.Id}/comments",
            commentRequest);
        commentResponse.EnsureSuccessStatusCode();

        // 3. Обновить статус
        var updateRequest = new { status = "Closed" };
        var updateResponse = await _client.PatchAsJsonAsync(
            $"/api/tickets/{ticket.Id}",
            updateRequest);
        updateResponse.EnsureSuccessStatusCode();
    }
}
```

### Ручное тестирование

1. Запустите приложение с настройками для sandbox
2. Вызовите API endpoints через Swagger или Postman
3. Проверьте логи на наличие всех обязательных полей
4. Проверьте retry логику, временно отключив Zoho API

## Критерии приёмки

- [x] Модуль авторизуется в Zoho Desk API
- [x] Может создать контакт и тикет
- [x] Может обновить существующий тикет
- [x] Может добавить комментарий к тикету
- [x] Все запросы логируются
- [x] Ошибки обрабатываются (повторные попытки, уведомления)

## Лицензия

MIT License
