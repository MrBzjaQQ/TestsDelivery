# Bff Portal Service

## Описание

BffPortalService агрегирует данные из других микросервисов для фронтенда (Backend-For-Frontend паттерн).

## Основные возможности

- Агрегация данных из microservices
- Кэширование результатов
- Аутентификация запросов
- Формирование ответов для UI
- Фильтрация и сортировка данных

## Технический стек

- .NET 10
- PostgreSQL (cached data)
- Refit (HTTP clients)
- MemoryCache (caching)
- ASP.NET Core Identity (auth)

## API endpoints

### Auth API (проксируется в IdentityService)

- `POST /auth/register` - Регистрация пользователя
- `POST /auth/login` - Вход в систему
- `POST /auth/refresh` - Обновление токена
- `POST /auth/logout` - Выход из системы
- `POST /auth/forgot-password` - Запрос сброса пароля
- `POST /auth/reset-password` - Сброс пароля

### Portal API

- `GET /portal/students/profile` - Профиль студента
- `GET /portal/tests/available` - Доступные тесты
- `GET /portal/groups/{id}/analytics` - Аналитика группы
- `POST /portal/tests/{id}/start` - Начать тест

## HTTP Clients

- IdentityService (Refit) - аутентификация и авторизация пользователей
- QuestionManagementService (Refit) - управление вопросами и тестами
- StudentManagementService (Refit) - управление студентами и группами
- TestCheckingService (Refit) - проверка результатов тестов

## Взаимодействие с IdentityService

BffPortalService выступает единой точкой входа для Angular UI и проксирует все запросы аутентификации в IdentityService. Это упрощает архитектуру для pet-проекта — фронтенд работает только с одним сервисом (порт 8080).

### Архитектура аутентификации

```
Angular UI ──> BffPortalService (8080) ──> IdentityService (8081)
                    │
                    └── Проксирование auth-запросов
                    └── Валидация JWT токенов (локально)
```

### Как работает аутентификация

1. **Регистрация/Вход**: UI отправляет запрос на BffPortalService, который проксирует его в IdentityService
2. **Получение токена**: IdentityService возвращает JWT токены через BffPortalService
3. **Использование токена**: UI передаёт токен в заголовке `Authorization: Bearer <token>` при последующих запросах
4. **Валидация токена**: BffPortalService валидирует токен локально через Microsoft.AspNetCore.Authentication.JwtBearer (без обращения к IdentityService)
5. **Извлечение данных пользователя**: Из токена извлекается `userId` (claim `sub` или `NameIdentifier`)

### Роли пользователей

| Роль | Описание |
|------|----------|
| Student | Прохождение тестов, просмотр результатов |
| Teacher | Создание тестов, просмотр результатов студентов |
| Admin | Полный доступ к системе |