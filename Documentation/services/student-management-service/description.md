# Student Management Service

## Описание

StudentManagementService управляет регистрацией, профилями и прогрессом студентов. Сервис отслеживает выполнение тестов, сохраняет результаты и предоставляет информацию о текущем статусе обучения.

## Основные возможности

- Регистрация и управление студентами
- Привязка студентов к тестам
- Отслеживание прогресса выполнения тестов
- Получение статистики по студентам и группам
- Управление доступом к тестам по расписанию

## Технический стек

- .NET 10
- PostgreSQL
- Entity Framework Core
- MassTransit (RabbitMQ)
- Refit (HTTP clients)
- ASP.NET Core Identity

## Зависимости

| Пакет | Назначение |
|-------|------------|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | PostgreSQL подключение |
| `MassTransit.RabbitMQ` | Асинхронная коммуникация |
| `Refit.HttpClientFactory` | HTTP клиенты для микросервисов |
| `FluentValidation` | Валидация |
| `Testcontainers.PostgreSQL` | Integration тесты |

## Связи с другими сервисами

- **IdentityService**: Аутентификация, получение данных о пользователях
- **QuestionManagementService**: Получение информации о тестах
- **TestCheckingService**: Получение результатов проверки тестов
- **NotificationService**: Отправка уведомлений о результатах

## API endpoints

### Студенты
- `POST /students` - Зарегистрировать студента
- `GET /students/{id}` - Получить студента
- `PUT /students/{id}` - Обновить профиль
- `GET /students?groupId={id}&status={active}` - Фильтрация

### Тесты для студентов
- `POST /students/{id}/tests/{testId}/assign` - Назначить тест
- `GET /students/{id}/tests` - Получить назначенные тесты
- `GET /students/{id}/tests/active` - Активные тесты
- `POST /students/{id}/tests/{testId}/submit` - Отправить ответы

### Прогресс
- `GET /students/{id}/progress` - Прогресс по всем тестам
- `GET /students/{id}/tests/{testId}/results` - Результаты теста
- `GET /groups/{id}/statistics` - Статистика по группе

## События MassTransit

| Событие | Описание | Источник |
|---------|-----------|----------|
| `TestAssignedEvent` | Тест назначен студенту | BffPortalService |
| `TestSubmittedEvent` | Тест отправлен студентом | StudentManagementService |
| `TestResultReceivedEvent` | Получен результат проверки | TestCheckingService |

## Миграции БД

```bash
cd src/StudentManagementService/StudentManagementService.Infrastructure.Database/
dotnet ef migrations add Create_StudentTables --project StudentManagementService.Infrastructure.Database.csproj --startup-project ../StudentManagementService.WebApi/StudentManagementService.WebApi.csproj
```
