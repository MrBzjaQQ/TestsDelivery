# Identity Service

## Описание

IdentityService управляет пользователями, аутентификацией и авторизацией через ASP.NET Core Identity + JWT tokens.

## Основные возможности

- Регистрация пользователей
- Аутентификация (Login/Logout)
- Управление ролями (Student, Teacher, Admin)
- JWT токены
- Восстановление пароля
- Подтверждение email

## Технический стек

- .NET 10
- PostgreSQL
- ASP.NET Core Identity
- Entity Framework Core
- JWT Authentication
- MassTransit (RabbitMQ)

## API endpoints

- `POST /auth/register` - Регистрация
- `POST /auth/login` - Вход
- `POST /auth/refresh` - Обновить токен
- `GET /users/{id}` - Получить пользователя
- `POST /users/{id}/roles` - Назначить роль
- `POST /auth/forgot-password` - Запрос сброса пароля
- `POST /auth/reset-password` - Сброс пароля

## События MassTransit

| Событие | Описание | Получатель |
|---------|-----------|-----------|
| `UserRegisteredEvent` | Новый пользователь создан | NotificationService |

## Миграции БД

```bash
cd src/IdentityService/IdentityService.Infrastructure.Database/
dotnet ef migrations add Create_IdentityTables --project IdentityService.Infrastructure.Database.csproj --startup-project ../IdentityService.WebApi/IdentityService.WebApi.csproj
```