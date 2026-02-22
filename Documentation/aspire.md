# .NET Aspire для TestsDelivery

## Введение

[.NET Aspire](https://learn.microsoft.com/ru-ru/dotnet/aspire/) — это стек для создания распределённых приложений, который упрощает разработку, запуск, отладку и развёртывание микросервисных приложений. Для учебного проекта TestsDelivery Aspire предоставит:

- **Единый опыт разработки** — запуск всей системы одной командой
- **Автоматическая оркестрация** — управление зависимостями между сервисами
- **Встроенная наблюдаемость** — OpenTelemetry, логирование, метрики
- **Service Discovery** — автоматическая конфигурация взаимодействия сервисов

## Структура решения с Aspire

```
TestsDelivery.sln
├── TestsDelivery.AppHost/           # Оркестрация приложения
├── TestsDelivery.ServiceDefaults/   # Общие настройки для всех сервисов
│
├── IdentityService/
│   ├── IdentityService.Domain/
│   ├── IdentityService.Application/
│   ├── IdentityService.Infrastructure/
│   └── IdentityService.WebApi/
│
├── QuestionManagementService/
│   ├── QuestionManagementService.Domain/
│   ├── QuestionManagementService.Application/
│   ├── QuestionManagementService.Infrastructure/
│   └── QuestionManagementService.WebApi/
│
├── StudentManagementService/
│   ├── StudentManagementService.Domain/
│   ├── StudentManagementService.Application/
│   ├── StudentManagementService.Infrastructure/
│   └── StudentManagementService.WebApi/
│
├── TestCheckingService/
│   ├── TestCheckingService.Domain/
│   ├── TestCheckingService.Application/
│   ├── TestCheckingService.Infrastructure/
│   └── TestCheckingService.WebApi/
│
├── FileStorageService/
│   ├── FileStorageService.Domain/
│   ├── FileStorageService.Application/
│   ├── FileStorageService.Infrastructure/
│   └── FileStorageService.WebApi/
│
├── NotificationService/
│   ├── NotificationService.Domain/
│   ├── NotificationService.Application/
│   ├── NotificationService.Infrastructure/
│   └── NotificationService.WebApi/
│
├── BffPortalService/
│   ├── BffPortalService.Domain/
│   ├── BffPortalService.Application/
│   ├── BffPortalService.Infrastructure/
│   └── BffPortalService.WebApi/
│
└── TestsDelivery.Ui/               # Angular UI (frontend)
```

## Компоненты Aspire

### 1. AppHost (TestsDelivery.AppHost)

Отвечает за оркестрацию всех сервисов и ресурсов:

```csharp
// TestsDelivery.AppHost/Program.cs
var builder = DistributedApplication.CreateBuilder(args);

// Ресурсы баз данных PostgreSQL
var postgresIdentity = builder.AddPostgres("postgres-identity", port: 5432)
    .WithDatabase("identitydb");
var postgresQuestion = builder.AddPostgres("postgres-question", port: 5433)
    .WithDatabase("questiondb");
var postgresStudent = builder.AddPostgres("postgres-student", port: 5434)
    .WithDatabase("studentdb");
var postgresChecking = builder.AddPostgres("postgres-checking", port: 5435)
    .WithDatabase("checkingdb");
var postgresFiles = builder.AddPostgres("postgres-files", port: 5436)
    .WithDatabase("filesdb");
var postgresNotif = builder.AddPostgres("postgres-notification", port: 5437)
    .WithDatabase("notificationdb");

// RabbitMQ для messaging
var rabbitmq = builder.AddRabbitMQ("rabbitmq", port: 5672);

// Redis для кэширования и сессий
var redis = builder.AddRedis("redis", port: 6379);

// Микросервисы
var identityService = builder.AddProject<Projects.IdentityService_WebApi>("identity-service")
    .WithReference(postgresIdentity)
    .WithReference(rabbitmq)
    .WaitFor(postgresIdentity);

var questionService = builder.AddProject<Projects.QuestionManagementService_WebApi>("question-service")
    .WithReference(postgresQuestion)
    .WithReference(rabbitmq)
    .WaitFor(postgresQuestion);

var studentService = builder.AddProject<Projects.StudentManagementService_WebApi>("student-service")
    .WithReference(postgresStudent)
    .WithReference(rabbitmq)
    .WaitFor(postgresStudent);

var checkingService = builder.AddProject<Projects.TestCheckingService_WebApi>("checking-service")
    .WithReference(postgresChecking)
    .WithReference(rabbitmq)
    .WaitFor(postgresChecking);

var fileService = builder.AddProject<Projects.FileStorageService_WebApi>("file-service")
    .WithReference(postgresFiles)
    .WaitFor(postgresFiles);

var notificationService = builder.AddProject<Projects.NotificationService_WebApi>("notification-service")
    .WithReference(postgresNotif)
    .WithReference(rabbitmq)
    .WithReference(redis)
    .WaitFor(postgresNotif);

var bffService = builder.AddProject<Projects.BffPortalService_WebApi>("bff-portal-service")
    .WithReference(identityService)
    .WithReference(questionService)
    .WithReference(studentService)
    .WithReference(checkingService)
    .WithReference(fileService)
    .WithReference(notificationService)
    .WithReference(redis)
    .WaitFor(identityService);

// Angular UI (Frontend)
builder.AddNpmApp("ui", "../TestsDelivery.Ui")
    .WithReference(bffService)
    .WaitFor(bffService);

builder.Build().Run();
```

### 2. ServiceDefaults (TestsDelivery.ServiceDefaults)

Общие настройки для всех сервисов:

```csharp
// TestsDelivery.ServiceDefaults/Extensions.cs
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(
        this IHostApplicationBuilder builder)
    {
        builder.AddDefaultLogging();
        builder.AddOpenTelemetry();

        builder.Services.AddHealthChecks()
            .AddNpgSql()
            .AddRabbitMQ();

        builder.Services.AddServiceDiscovery();

        return builder;
    }
}
```

## Конфигурация сервисов

### Program.cs каждого сервиса

```csharp
// Пример: IdentityService.WebApi/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDataSource("IdentityDb");

builder.Services.AddMassTransit(rabbitmqOptions =>
{
    rabbitmqOptions.Host = builder.Configuration["RabbitMQ:Host"];
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
```

### Конфигурация подключения к ресурсам

Aspire автоматически внедряет настройки подключения через переменные окружения:

```csharp
// Чтение строки подключения к PostgreSQL
builder.Configuration.GetConnectionString("postgres-identity");

// Чтение настроек RabbitMQ
builder.Configuration["RabbitMQ:Host"];
builder.Configuration["RabbitMQ:Port"];
```

## Конфигурация для разработки

### Запуск Aspire Dashboard

```bash
cd TestsDelivery.AppHost
dotnet run
```

Aspire автоматически запустит:
- Все сервисы в правильном порядке (с учётом зависимостей)
- PostgreSQL контейнеры
- RabbitMQ контейнер
- Redis контейнер
- Angular UI
- Dashboard по адресу `http://localhost:18888`

### Aspire Dashboard

Dashboard предоставляет:
- Мониторинг всех сервисов
- Логи в реальном времени
- Health checks статус
- Трассировку запросов (OpenTelemetry)
- Доступ к БД через Web UI

## Конфигурация для продакшена

### С публикацией в Azure

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddAzurePostgresFlexibleServer("postgres");
var rabbitmq = builder.AddRabbitMQ("rabbitmq");
var redis = builder.AddAzureRedis("redis");

var identityService = builder.AddProject<Projects.IdentityService_WebApi>("identity-service")
    .WithReference(postgres)
    .WithReference(rabbitmq);

builder.Build().Run();
```

### Helm chart для Kubernetes

Aspire генерирует манифесты для Kubernetes:

```bash
dotnet publish -c Release -o ./publish
```

## Преимущества для учебного проекта

| Возможность | Описание |
|-------------|----------|
| **Единый запуск** | `dotnet run` из AppHost запускает всё приложение |
| **Автозависимости** | Сервисы ждут готовности БД и RabbitMQ |
| **Service Discovery** | URL сервисов автоматически через environment variables |
| **Dashboard** | Удобный UI для отладки и мониторинга |
| **OpenTelemetry** | Встроенная трассировка между сервисами |
| **Консистентность** | Одинаковая архитектура локально и в продакшене |

## Установка

### Установка Aspire workload

```bash
dotnet workload install aspire
```

### Создание проекта

```bash
# Добавить AppHost
dotnet new aspire-apphost -n TestsDelivery.AppHost

# Добавить ServiceDefaults  
dotnet new aspire-servicedefaults -n TestsDelivery.ServiceDefaults

# Добавить к решению
dotnet sln add TestsDelivery.AppHost/TestsDelivery.AppHost.csproj
dotnet sln add TestsDelivery.ServiceDefaults/TestsDelivery.ServiceDefaults.csproj
```

## Миграция с docker-compose

Текущий `docker-compose.yml` можно заменить на Aspire:

| docker-compose | Aspire |
|----------------|--------|
| `ports` | Автоматически через `.WithEndpoint()` |
| `depends_on` | `.WaitFor()` |
| `networks` | Автоматически |
| `environment` | Автоматически через `.WithReference()` |
| Health checks | Встроенные |

## Ресурсы

- [Документация .NET Aspire](https://learn.microsoft.com/ru-ru/dotnet/aspire/)
- [Aspire на русском](https://learn.microsoft.com/ru-ru/dotnet/aspire/fundamentals/)
- [Обучение Aspire](https://learn.microsoft.com/training/paths/dotnet-aspire/)
