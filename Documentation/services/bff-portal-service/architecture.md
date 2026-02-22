# Bff Portal Service - Architecture

## Структура проекта

### Слой Domain

```
BffPortalService.Domain/
├── Entities/
│   ├── PortalCacheItem.cs             # Кэшированные данные
│   └── PortalSettings.cs              # Настройки портала
├── ValueObjects/
│   ├── PortalDataKey.cs               # Ключ кэша
│   └── CacheDuration.cs               | Duration
└── Exceptions/
    ├── PortalDataNotFoundException.cs
    └── ServiceUnavailableException.cs
```

### Слой Application

```
BffPortalService.Application/
├── Services/
│   ├── StudentPortalService.cs
│   ├── TestsPortalService.cs
│   ├── GroupAnalyticsService.cs
│   └── CacheService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── GetStudentProfileRequest.cs
│   │   └── GetTestsRequest.cs
│   └── Responses/
│       ├── StudentProfileDto.cs
│       ├── AvailableTestsDto.cs
│       └── GroupAnalyticsDto.cs
├── Specifications/
│   ├── PortalSpecs/
│   │   ├── UserHasAccessSpecification.cs
│   │   └── CacheNotExpiredSpecification.cs
│   └── GroupSpecs/
│       ├── GroupExistsSpecification.cs
│       └── GroupHasStudentsSpecification.cs
├── Contracts/
│   ├── IStudentPortalService.cs
│   ├── ITestsPortalService.cs
│   └── IGroupAnalyticsService.cs
└── Exceptions/
    └── DataAggregationException.cs
```

### Слой Infrastructure

```
BffPortalService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── BffPortalDbContext.cs
│   │   └── IBffPortalDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_PortalCacheTable.cs
│   │   └── BffPortalDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   └── PortalCacheRepository.cs
├── External/
│   ├── HttpClientFactories/
│   │   ├── IdentityServiceClientFactory.cs
│   │   ├── QuestionServiceClientFactory.cs
│   │   ├── StudentServiceClientFactory.cs
│   │   └── TestCheckingServiceClientFactory.cs
│   └── Clients/
│       ├── IIdentityServiceClient.cs (Refit)
│       ├── IQuestionServiceClient.cs (Refit)
│       ├── IStudentServiceClient.cs (Refit)
│       └── ITestCheckingServiceClient.cs (Refit)
└── Caching/
    ├── MemoryCacheService.cs
    └── CacheKeyGenerator.cs
```

### Слой WebApi

```
BffPortalService.WebApi/
├── Controllers/
│   ├── AuthController.cs
│   ├── PortalController.cs
│   ├── StudentsController.cs
│   ├── TestsController.cs
│   └── GroupsController.cs
├── Settings/
│   ├── AppSettings.cs
│   └── ExternalServiceUrls.cs
├── Shared/
│   └── ResponseResultModel.cs
├── HealthChecks/
│   ├── PostgreSqlHealthCheck.cs
│   └── ExternalServiceHealthCheck.cs
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
BffPortalService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── StudentPortalServiceTests.cs
│   │   ├── TestsPortalServiceTests.cs
│   │   └── GroupAnalyticsServiceTests.cs
│   └── Controllers/
│       └── PortalControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
│   │   └── HttpClientFixture.cs
    └── Services/
        └── StudentPortalServiceIntegrationTests.cs
```

### Layer Flow

```
Client Request
    ↓
WebApi (Controller)
    ↓
Application (Service)
    ↓
Application (Specification)
    ↓
Infrastructure (HTTP Client/Cache)
    ↓
Microservices / Cache / PostgreSQL
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
        
        // Database
        builder.Services.AddDatabase(settings.ConnectionString);
        
        // Application services
        builder.Services.AddApplicationServices();
        
        // HTTP clients
        builder.Services.AddHttpClient("IdentityService", client =>
        {
            client.BaseAddress = new Uri(settings.ServiceUrls.IdentityServiceUrl);
        });
        
        builder.Services.AddHttpClient("QuestionService", client =>
        {
            client.BaseAddress = new Uri(settings.ServiceUrls.QuestionServiceUrl);
        });
        
        builder.Services.AddHttpClient("StudentService", client =>
        {
            client.BaseAddress = new Uri(settings.ServiceUrls.StudentServiceUrl);
        });
        
        builder.Services.AddHttpClient("TestCheckingService", client =>
        {
            client.BaseAddress = new Uri(settings.ServiceUrls.TestCheckingServiceUrl);
        });
        
        // Refit clients
        builder.Services.AddRefitClient<IIdentityServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.ServiceUrls.IdentityServiceUrl));
        
        builder.Services.AddRefitClient<IQuestionServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.ServiceUrls.QuestionServiceUrl));
        
        builder.Services.AddRefitClient<IStudentServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.ServiceUrls.StudentServiceUrl));
        
        builder.Services.AddRefitClient<ITestCheckingServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.ServiceUrls.TestCheckingServiceUrl));
        
        // Cache
        builder.Services.AddMemoryCache();
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck")
            .AddCheck<ExternalServiceHealthCheck>("ExternalServicesHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### HTTP Client Factory

```csharp
// Add HTTP client
builder.Services.AddHttpClient("StudentService", client =>
{
    client.BaseAddress = new Uri(settings.ServiceUrls.StudentServiceUrl);
});

// Use in service
public class StudentPortalService : IStudentPortalService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    public async Task<StudentProfileDto> GetStudentProfileAsync(Guid studentId, string accessToken, CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("StudentService");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await client.GetAsync($"api/v1/students/{studentId}", ct);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<StudentProfileDto>(ct);
            return content;
        }
        
        throw new ServiceUnavailableException("Student service unavailable");
    }
}
```

### Cache Service

```csharp
public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;

    public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration, CancellationToken ct)
    {
        if (_cache.TryGetValue(key, out T cached))
        {
            _logger.LogDebug("Cache hit for key {Key}", key);
            return cached;
        }

        _logger.LogDebug("Cache miss for key {Key}, fetching data", key);
        
        var result = await factory();
        
        _cache.Set(key, result, expiration);
        
        return result;
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs, Specifications |
| **Infrastructure** | Data access | DbContext, Repositories, HTTP Clients, Cache |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests** | Tests | Unit and Integration tests |