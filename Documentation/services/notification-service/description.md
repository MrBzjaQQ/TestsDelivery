# Notification Service

## Описание

NotificationService отправляет электронные письма и уведомления студентам и преподавателям через RabbitMQ + MassTransit.

## Основные возможности

- Отправка email уведомлений (SMTP)
- Push уведомления
- Шаблоны писем
- Очередь уведомлений
- Повторная отправка при ошибках

## Технический стек

- .NET 10
- PostgreSQL (notification queue)
- MailKit (SMTP)
- MassTransit (RabbitMQ)
- Handlebars (email templates)

## API endpoints

- `POST /notifications/send-email` - Отправить email
- `POST /notifications/test-created/{testId}` - Уведомление о новом тесте
- `POST /notifications/result-ready/{studentId}/{testId}` - Результат готов
- `GET /notifications/pending` - ПENDING уведомления

## События MassTransit

| Событие | Описание | Источник |
|---------|-----------|----------|
| `TestCreatedEvent` | Создан новый тест | QuestionManagementService |
| `TestResultReceivedEvent` | Результат готов | TestCheckingService |
| `UserRegisteredEvent` | Новый пользователь | IdentityService |

## Миграции БД

```bash
cd src/NotificationService/NotificationService.Infrastructure.Database/
dotnet ef migrations add Create_NotificationTable --project NotificationService.Infrastructure.Database.csproj --startup-project ../NotificationService.WebApi/NotificationService.WebApi.csproj
```