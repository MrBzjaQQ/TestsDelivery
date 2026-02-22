# TestsDelivery System

## Описание проекта

TestsDelivery - это микросервисная система доставки тестов студентам. Система позволяет создавать и управлять тестами, выдавать их студентам, проверять ответы и формировать результаты.

## Архитектура

Система состоит из 8 микросервисов:

1. **QuestionManagementService** - управление вопросами и тестами
2. **StudentManagementService** - управление студентами
3. **TestCheckingService** - проверка тестов и результатов
4. **FileStorageService** - хранение файлов (изображения, документы)
5. **NotificationService** - отправка уведомлений и писем
6. **IdentityService** - управление пользователями и аутентификация
7. **BffPortalService** - Backend-For-Frontend сервис для портала
8. **Angular 21 UI** - фронтенд приложение

## Технологии

- **Backend**: .NET 10, C#
- **Database**: PostgreSQL
- **Messaging**: RabbitMQ + MassTransit
- **Orchestration**: Docker + Docker Compose
- **Testing**: TestContainers, xUnit, Moq, Refit
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity + JWT
- **Frontend**: Angular 21

## Структура репозитория

```
TestsDelivery/
├── Documentation/
│   ├── solution-overview.md          # Обзор архитектуры решения
│   ├── docker-compose.md             # Docker Compose конфигурация
│   └── deployment.md                 # Руководство по развёртыванию
├── src/
│   ├── QuestionManagementService/
│   ├── StudentManagementService/
│   ├── TestCheckingService/
│   ├── FileStorageService/
│   ├── NotificationService/
│   ├── IdentityService/
│   ├── BffPortalService/
│   └── TestsDelivery.Web/
└── Tests/
    ├── QuestionManagementService.Tests/
    ├── StudentManagementService.Tests/
    └── ...
```

## Микросервисы

### QuestionManagementService
Управление вопросами, банками вопросов и тестами. Поддержка шаблонов тестов.

### StudentManagementService
Регистрация, управление профилями студентов и их прогрессом.

### TestCheckingService
Автоматическая проверка тестов, подсчёт баллов, формирование результатов.

### FileStorageService
Хранение и доставка файлов (изображения вопросов, документы).

### NotificationService
Отправка электронных писем и уведомлений о событиях системы.

### IdentityService
Аутентификация, авторизация, управление пользователями через ASP.NET Core Identity + JWT.

### BffPortalService
Агрегирует данные из других сервисов для фронтенда, реализует паттерн Backend-For-Frontend.

### Angular 21 UI
Интерфейс для студентов, преподавателей и администраторов.

## Коммуникация между сервисами

Микросервисы взаимодействуют через:
- **Synchronous**: HTTP/HTTPS (Refit clients)
- **Asynchronous**: RabbitMQ (MassTransit)

## Запуск

### Локально
```bash
dotnet build
dotnet run --project TestsDelivery.Web
```

### Docker
```bash
docker-compose up --build
```

## Сборка миграций БД

```bash
cd src/{ServiceName}/{ServiceName}.Infrastructure.Database/
dotnet ef migrations add Create_{TableName} --project {ServiceName}.Infrastructure.Database.csproj --startup-project ../{ServiceName}.WebApi/{ServiceName}.WebApi.csproj
```

## CI/CD

Система использует GitLab CI для автоматической сборки, тестирования и развёртывания.
