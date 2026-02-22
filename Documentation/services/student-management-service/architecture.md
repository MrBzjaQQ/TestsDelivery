# Student Management Service - Architecture

## Структура проекта

### Слой Domain

```
StudentManagementService.Domain/
├── Entities/
│   ├── Student.cs                     # Сущность студента
│   ├── TestAssignment.cs              # Назначение теста студенту
│   ├── TestProgress.cs                # Прогресс прохождения теста
│   └── StudyGroup.cs                  # Учебная группа
├── ValueObjects/
│   ├── StudentProfile.cs              # Профиль студента
│   ├── TestStatus.cs                  # Статус теста (NotStarted, InProgress, Completed)
│   └── Grade.cs                       # Оценка
└── Exceptions/
    ├── StudentNotFoundException.cs
    ├── TestAssignmentNotFoundException.cs
    └── StudentAlreadyEnrolledException.cs
```

### Слой Application

```
StudentManagementService.Application/
├── Services/
│   ├── StudentService.cs
│   ├── TestAssignmentService.cs
│   ├── ProgressTrackingService.cs
│   └── GroupService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── RegisterStudentRequest.cs
│   │   ├── UpdateStudentProfileRequest.cs
│   │   └── SubmitTestRequest.cs
│   └── Responses/
│       ├── StudentDto.cs
│       ├── TestAssignmentDto.cs
│       └── ProgressReportDto.cs
├── Specifications/
│   ├── StudentSpecs/
│   │   ├── GroupEqualsSpecification.cs
│   │   ├── StudentActiveSpecification.cs
│   └── TestAssignmentSpecs/
│       ├── TestNotExpiredSpecification.cs
│       └── AssignmentExistsSpecification.cs
├── Contracts/
│   ├── IStudentService.cs
│   ├── ITestAssignmentService.cs
│   └── IProgressTrackingService.cs
└── Exceptions/
    ├── TestNotEligibleException.cs
    └── ProgressAlreadyExistsException.cs
```

### Слой Infrastructure

```
StudentManagementService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── StudentDbContext.cs
│   │   └── IStudentDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_StudentTables.cs
│   │   └── StudentDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   ├── StudentRepository.cs
│   ├── TestAssignmentRepository.cs
│   └── ProgressRepository.cs
├── MassTransit/
│   ├── Consumer/
│   │   ├── TestResultReceivedConsumer.cs
│   │   └── TestAssignedConsumer.cs
│   └── MessageTypes/
│       ├── TestResultReceivedEvent.cs
│       └── TestAssignedEvent.cs
└── External/
    ├── HttpClientFactories/
    │   ├── IdentityServiceClientFactory.cs
    │   └── TestCheckingServiceClientFactory.cs
    └── Clients/
        ├── IIdentityServiceClient.cs (Refit)
        └── ITestCheckingServiceClient.cs (Refit)
```

### Слой WebApi

```
StudentManagementService.WebApi/
├── Controllers/
│   ├── StudentsController.cs
│   ├── TestsController.cs
│   ├── ProgressController.cs
│   └── GroupsController.cs
├── Settings/
│   ├── AppSettings.cs
│   └── ExternalServiceSettings.cs
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
StudentManagementService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── StudentServiceTests.cs
│   │   ├── TestAssignmentServiceTests.cs
│   │   └── ProgressTrackingServiceTests.cs
│   ├── Specifications/
│   │   └── GroupEqualsSpecificationTests.cs
│   └── Controllers/
│       └── StudentsControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   ├── DbTestsBase.cs
    │   └── TestDataSeeder.cs
    ├── Controllers/
    │   └── StudentsControllerTests.cs
    └── Services/
        └── ProgressTrackingServiceTests.cs
```

## Layer Flow

```
Client Request
    ↓
WebApi (Controller)
    ↓
Application (Service)
    ↓
Application (Specification)
    ↓
Infrastructure (Repository/HTTP Client)
    ↓
PostgreSQL / External Service
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
        builder.Services.AddIdentityServiceClient(builder.Configuration);
        builder.Services.AddTestCheckingServiceClient(builder.Configuration);
        
        // MassTransit
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
        
        // Exception handling
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        // Health checks
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### Generic Repository

```csharp
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly StudentDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(StudentDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        Guard.IsNotNull(entity);
        await _dbSet.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }

    public virtual async Task<IEnumerable<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken token)
    {
        return await query.ToListAsync(token);
    }
}
```

### Unit of Work

```csharp
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IStudentDbContext context)
    {
        StudentRepository = new StudentRepository(context);
        TestAssignmentRepository = new TestAssignmentRepository(context);
        ProgressRepository = new ProgressRepository(context);
    }

    public IGenericRepository<Student> StudentRepository { get; set; }
    public IGenericRepository<TestAssignment> TestAssignmentRepository { get; set; }
    public IGenericRepository<TestProgress> ProgressRepository { get; set; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
```

### Refit HTTP Client

```csharp
// Interface
public interface IIdentityServiceClient
{
    [Get("/api/v1/users/{id}")]
    Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken ct);
}

// Registration
public static class DependencyInjection
{
    public static IServiceCollection AddIdentityServiceClient(this IServiceCollection services, IConfiguration config)
    {
        var url = config.GetValue<string>("ExternalServices:IdentityServiceUrl");
        
        services.AddRefitClient<IIdentityServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(url));
        
        return services;
    }
}
```

### MassTransit Consumer

```csharp
public class TestResultReceivedConsumer : IConsumer<TestResultReceivedEvent>
{
    private readonly ITestProgressService _progressService;
    private readonly ILogger<TestResultReceivedConsumer> _logger;

    public TestResultReceivedConsumer(
        ITestProgressService progressService,
        ILogger<TestResultReceivedConsumer> logger)
    {
        _progressService = progressService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestResultReceivedEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Processing test result for student {StudentId}", @event.StudentId);
        
        await _progressService.UpdateTestProgress(
            @event.StudentId,
            @event.TestId,
            @event.Score,
            @event.IsPassed,
            context.CancellationToken);
    }
}
```
