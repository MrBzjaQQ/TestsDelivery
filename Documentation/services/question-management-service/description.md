# Question Management Service

## Описание

QuestionManagementService - микросервис для управления вопросами, банками вопросов и тестами. Сервис поддерживает создание, редактирование, копирование тестов и шаблонов.

## Основные возможности

- Создание и управление вопросами разного типа (выбор, ввод, сопоставление)
- Создание банков вопросов и их группировка
- Создание тестов на основе шаблонов
- Динамическая генерация тестов по банку вопросов
- Копирование и импорт/экспорт тестов
- Маркировка вопросов по категориям и сложности

## Технический стек

- .NET 10
- PostgreSQL
- Entity Framework Core
- MassTransit (RabbitMQ)
- ASP.NET Core Identity (для авторизации)
- TestContainers для интеграционного тестирования

## Зависимости

| Пакет | Назначение |
|-------|------------|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | Подключение к PostgreSQL |
| `MassTransit.RabbitMQ` | Асинхронная коммуникация |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Управление пользователями |
| `FluentValidation` | Валидация DTO |
| `FluentAssertions` | Агрессивные тесты |
| `Testcontainers.PostgreSQL` | Integration тесты |

## Связи с другими сервисами

- **IdentityService**: Аутентификация пользователей
- **BffPortalService**: Поставляет данные для UI
- **StudentManagementService**: Получает запросы на создание тестов
- **TestCheckingService**: Отправляет тесты на проверку

## API endpoints

### Вопросы
- `POST /questions` - Создать вопрос
- `GET /questions/{id}` - Получить вопрос
- `PUT /questions/{id}` - Обновить вопрос
- `DELETE /questions/{id}` - Удалить вопрос
- `GET /questions?category={category}&difficulty={difficulty}` - Фильтрация

### Банки вопросов
- `POST /question-banks` - Создать банк
- `GET /question-banks/{id}` - Получить банк
- `GET /question-banks/{id}/questions` - Вопросы банка
- `POST /question-banks/{id}/questions` - Добавить вопрос в банк

### Тесты
- `POST /tests` - Создать тест
- `GET /tests/{id}` - Получить тест
- `GET /tests?templateId={id}` - Тесты по шаблону
- `POST /tests/{id}/copy` - Копировать тест
- `POST /tests/{id}/generate` - Генерировать тест из банка

### Шаблоны
- `POST /templates` - Создать шаблон
- `GET /templates/{id}` - Получить шаблон
- `PUT /templates/{id}` - Обновить шаблон

## События MassTransit

| Событие | Описание | Получатели |
|---------|-----------|------------|
| `TestCreatedEvent` | Создан новый тест | NotificationService |
| `QuestionBankUpdatedEvent` | Обновлён банк вопросов | StudentManagementService |

## Миграции БД

```bash
cd src/QuestionManagementService/QuestionManagementService.Infrastructure.Database/
dotnet ef migrations add Create_QuestionTable --project QuestionManagementService.Infrastructure.Database.csproj --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```
