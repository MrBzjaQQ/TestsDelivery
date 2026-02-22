# File Storage Service - Architecture

## Структура проекта

### Слой Domain

```
FileStorageService.Domain/
├── Entities/
│   ├── File.cs                        # Файл для хранения
│   └── FileMetadata.cs                # Метаданные
├── ValueObjects/
│   ├── FileName.cs                    # Имя файла
│   ├── FileContentType.cs             # MIME type
│   └── FileSize.cs                    # Размер файла
└── Exceptions/
    ├── FileNotFoundException.cs
    └── InvalidFileException.cs
```

### Слой Application

```
FileStorageService.Application/
├── Services/
│   ├── FileUploadService.cs
│   ├── FileDownloadService.cs
│   └── FileDeleteService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── UploadFileRequest.cs
│   │   └── DownloadFileRequest.cs
│   └── Responses/
│       ├── FileDto.cs
│       └── UploadResponseDto.cs
└── Contracts/
    ├── IFileUploadService.cs
    ├── IFileDownloadService.cs
    └── IFileDeleteService.cs
```

### Слой Infrastructure

```
FileStorageService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── FileStorageDbContext.cs
│   │   └── IFileStorageDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_FileTable.cs
│   │   └── FileStorageDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   └── FileRepository.cs
└── FileProcessing/
    ├── ImageProcessor.cs
    ├── ThumbnailGenerator.cs
    └── FileValidator.cs
```

### Слой WebApi

```
FileStorageService.WebApi/
├── Controllers/
│   ├── FilesController.cs
│   └── ThumbnailsController.cs
├── Settings/
│   ├── AppSettings.cs
│   └── FileStorageSettings.cs
├── Shared/
│   └── ResponseResultModel.cs
├── HealthChecks/
│   └── PostgreSqlHealthCheck.cs
├── Handler/
│   └── CustomExceptionHandler.cs
├── Factories/
│   ├── ProblemDetailsFactory.cs
│   └── IProblemDetailsFactory.cs
├── Constants/
│   ├── ProblemDetailsConstants.cs
│   └── LogMessageConstants.cs
├── DependencyInjection.cs
└── Program.cs
```

### Слой Tests

```
FileStorageService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── FileUploadServiceTests.cs
│   │   └── ImageProcessorTests.cs
│   └── Controllers/
│       └── FilesControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   └── DbTestsBase.cs
    └── Controllers/
        └── FilesControllerTests.cs
```

### Layer Flow

```
Client Request
    ↓
WebApi (Controller)
    ↓
Application (Service)
    ↓
Infrastructure (Repository/File Processor)
    ↓
PostgreSQL
    ↓
Response
```

## Dependency Injection

```csharp
public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        
        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();
        builder.Services.AddImageProcessing();
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### File Upload Service

```csharp
public class FileUploadService : IFileUploadService
{
    private readonly IFileRepository _fileRepository;
    private readonly IImageProcessor _imageProcessor;
    private readonly ILogger<FileUploadService> _logger;

    public async Task<UploadResponseDto> UploadFileAsync(
        IFormFile file,
        Guid ownerId,
        CancellationToken ct)
    {
        // Validate file
        var validation = _fileValidator.Validate(file);
        if (!validation.IsValid)
        {
            throw new InvalidFileException(validation.ErrorMessage);
        }

        // Process image if needed
        byte[] processedBytes = file.FileContentType switch
        {
            "image/jpeg" or "image/png" => await _imageProcessor.ProcessAsync(file.Bytes),
            _ => file.Bytes
        };

        // Store file
        var fileEntity = new File
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = processedBytes.Length,
            Data = processedBytes,
            OwnerId = ownerId,
            IsImage = file.ContentType.StartsWith("image/")
        };

        await _fileRepository.AddAsync(fileEntity, ct);

        return new UploadResponseDto
        {
            Id = fileEntity.Id,
            FileName = fileEntity.FileName,
            Size = fileEntity.Size,
            ContentType = fileEntity.ContentType,
            UploadedAt = fileEntity.CreatedAt
        };
    }
}
```

### Image Processor

```csharp
public class ImageProcessor : IImageProcessor
{
    public async Task<byte[]> ProcessAsync(byte[] imageBytes)
    {
        // Load image
        using var image = Image.Load(imageBytes);

        // Resize if too large
        if (image.Width > 1920 || image.Height > 1080)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Width = 1920,
                Height = 1080
            }));
        }

        // Compress
        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpgEncoder { Quality = 85 });

        return memoryStream.ToArray();
    }

    public async Task<byte[]> GenerateThumbnailAsync(byte[] imageBytes)
    {
        using var image = Image.Load(imageBytes);
        
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Crop,
            Width = 200,
            Height = 200
        }));

        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new JpgEncoder { Quality = 75 });

        return memoryStream.ToArray();
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs |
| **Infrastructure** | Data access | DbContext, Repositories, File Processing |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests** | Tests | Unit and Integration tests |