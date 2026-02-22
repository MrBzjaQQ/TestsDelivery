# File Storage Service

## Описание

FileStorageService хранит и доставляет файлы (изображения, документы) для вопросов и тестов.

## Основные возможности

- Загрузка файлов (images, PDF, documents)
- Получение файлов по ID
- Удаление файлов
- Поддержка various file types
- Сжатие изображений

## Технический стек

- .NET 10
- PostgreSQL (BLOB storage)
- Entity Framework Core
- MassTransit (RabbitMQ)
- ImageSharp (image processing)

## API endpoints

- `POST /files/upload` - Загрузить файл
- `GET /files/{id}` - Получить файл
- `DELETE /files/{id}` - Удалить файл
- `GET /files/{id}/thumbnail` - Получить thumbnail

## Миграции БД

```bash
cd src/FileStorageService/FileStorageService.Infrastructure.Database/
dotnet ef migrations add Create_FileTable --project FileStorageService.Infrastructure.Database.csproj --startup-project ../FileStorageService.WebApi/FileStorageService.WebApi.csproj
```