# Test Checking Service

## Описание

TestCheckingService проверяет тесты студентов, вычисляет баллы и хранит результаты. Сервис принимает ответы студентов и сравнивает с правильными ответами.

## Основные возможности

- Автоматическая проверка тестов
- Подсчет баллов по правильным ответам
- Определение прохождения/провала теста
- Хранение результатов с детализацией
- Отправка уведомлений о готовых результатах

## Технический стек

- .NET 10
- PostgreSQL
- Entity Framework Core
- MassTransit (RabbitMQ)
- ASP.NET Core
- xUnit, TestContainers

## Зависимости

| Пакет | Назначение |
|-------|------------|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | PostgreSQL подключение |
| `MassTransit.RabbitMQ` | Асинхронная коммуникация |
| `FluentValidation` | Валидация |
| `Testcontainers.PostgreSQL` | Integration тесты |

## Связи с другими сервисами

- **StudentManagementService**: Получает тесты для проверки
- **QuestionManagementService**: Получает правильные ответы
- **NotificationService**: Отправляет уведомления о результатах

## API endpoints

### Проверка тестов
- `POST /tests/{testId}/check` - Проверить тест
- `POST /tests/{testId}/check/batch` - Проверить несколько тестов

### Результаты
- `GET /tests/{testId}/results/{studentId}` - Получить результат студента
- `GET /tests/{testId}/results` - Все результаты по тесту
- `GET /students/{studentId}/results` - Все результаты студента

## События MassTransit

| Событие | Описание | Получатели |
|---------|-----------|------------|
| `TestResultReceivedEvent` | Результат проверки готов | StudentManagementService |
| `TestSubmittedEvent` | Тест отправлен на проверку | Self |

## Миграции БД

```bash
cd src/TestCheckingService/TestCheckingService.Infrastructure.Database/
dotnet ef migrations add Create_TestResultTable --project TestCheckingService.Infrastructure.Database.csproj --startup-project ../TestCheckingService.WebApi/TestCheckingService.WebApi.csproj
```