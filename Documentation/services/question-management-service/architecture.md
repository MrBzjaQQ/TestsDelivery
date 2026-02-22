# Question Management Service - Architecture

## Структура проекта

### Слой Domain (Domain Model)

```
QuestionManagementService.Domain/
├── Entities/
│   ├── Question.cs                    # Сущность вопроса
│   ├── QuestionBank.cs                # Сущность банка вопросов
│   ├── Test.cs                        # Сущность теста
│   ├── TestTemplate.cs                # Сущность шаблона теста
│   └── AnswerOption.cs                # Вариант ответа
├── ValueObjects/
│   ├── Category.cs                    # Объект категории
│   ├── DifficultyLevel.cs             # Уровень сложности
│   └── QuestionText.cs               # Объект текста вопроса
└── Exceptions/
    ├── QuestionNotFoundException.cs
    ├── QuestionBankNotFoundException.cs
    └── TestValidationException.cs
```

### Слой Application (Business Logic)

```
QuestionManagementService.Application/
├── Services/
│   ├── QuestionService.cs
│   ├── QuestionBankService.cs
│   ├── TestService.cs
│   └── TestTemplateService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── CreateQuestionRequest.cs
│   │   ├── UpdateQuestionRequest.cs
│   │   └── CreateTestRequest.cs
│   └── Responses/
│       ├── QuestionDto.cs
│       ├── TestDto.cs
│       └── QuestionBankDto.cs
├── Specifications/
│   ├── QuestionSpecs/
│   │   ├── CategoryEqualsSpecification.cs
│   │   ├── DifficultyEqualsSpecification.cs
│   │   └── QuestionBankContainsQuestionSpecification.cs
│   └── TestSpecs/
│       ├── TemplateEqualsSpecification.cs
│       └── TestNotExpiredSpecification.cs
├── Contracts/
│   ├── IQuestionService.cs
│   ├── IQuestionBankService.cs
│   └── ITestService.cs
└── Exceptions/
    ├── QuestionAlreadyExistsException.cs
    └── TestGenerationFailedException.cs
```

### Слой Infrastructure (Data Access)

```
QuestionManagementService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── QuestionDbContext.cs
│   │   └── IQuestionDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_QuestionTables.cs
│   │   └── QuestionDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs           # Реализация generic репозитория
│   ├── UnitOfWork.cs                  # Unit of Work pattern
│   ├── QuestionRepository.cs
│   ├── QuestionBankRepository.cs
│   └── TestRepository.cs
└── MassTransit/
    ├── Consumer/
    │   ├── TestCreatedConsumer.cs
    │   └── QuestionBankUpdatedConsumer.cs
    └── MessageTypes/
        ├── TestCreatedEvent.cs
        └── QuestionBankUpdatedEvent.cs
```

### Слой WebApi (API Layer)

```
QuestionManagementService.WebApi/
├── Controllers/
│   ├── QuestionsController.cs
│   ├── QuestionBanksController.cs
│   ├── TestsController.cs
│   └── TestTemplatesController.cs
├── Settings/
│   ├── AppSettings.cs                 # General settings
│   └── RabbitMQSettings.cs           # RabbitMQ configuration
├── Shared/
│   └── ResponseResultModel.cs         # Response pattern
├── HealthChecks/
│   └── PostgreSqlHealthCheck.cs       # Health check for DB
├── Handler/
│   └── CustomExceptionHandler.cs      # Global exception handler
├── Factories/
│   ├── ProblemDetailsFactory.cs
│   └── IProblemDetailsFactory.cs
├── Constants/
│   ├── ProblemDetailsConstants.cs
│   └── LogMessageConstants.cs
├── DependencyInjection.cs             # Service registration
└── Program.cs                         # Entry point
```

### Слой Tests (Tests)

```
QuestionManagementService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── QuestionServiceTests.cs
│   │   ├── QuestionBankServiceTests.cs
│   │   └── TestServiceTests.cs
│   ├── Specifications/
│   │   ├── CategoryEqualsSpecificationTests.cs
│   │   └── TestNotExpiredSpecificationTests.cs
│   └── controllers/
│       └── QuestionsControllerTests.cs
├── Integration/
│   ├── TestInfrastructure/
│   │   ├── DatabaseFixture.cs         # TestContainers setup
│   │   ├── DbTestsBase.cs             # Base test class
│   │   └── TestDataSeeder.cs          # Test data
│   ├── Controllers/
│   │   ├── QuestionsControllerTests.cs
│   │   └── TestsControllerTests.cs
│   └── Repositories/
│       └── QuestionRepositoryTests.cs
└── Integration/
    ├── MassTransit/
    │   └── TestCreatedConsumerTests.cs
    └── Services/
        └── TestGenerationServiceTests.cs
```

## Layer Flow

```
Client Request
    ↓
WebApi (Controller)
    ↓
Application (Service)
    ↓
Application (Specification for filtering)
    ↓
Infrastructure (Repository)
    ↓
Infrastructure (EF Core DbContext)
    ↓
PostgreSQL Database
```

## Dependency Injection Structure

```csharp
// WebApi layer
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
        
        // Exception handling
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        // Factories
        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        
        // Health checks
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### Generic Repository Pattern

```csharp
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly QuestionDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(QuestionDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        await _dbSet.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.AnyAsync(predicate, token);
    }
}
```

### Unit of Work Pattern

```csharp
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IQuestionDbContext context)
    {
        QuestionRepository = new QuestionRepository(context);
        QuestionBankRepository = new QuestionBankRepository(context);
        TestRepository = new TestRepository(context);
    }

    public IGenericRepository<Question> QuestionRepository { get; set; }
    public IGenericRepository<QuestionBank> QuestionBankRepository { get; set; }
    public IGenericRepository<Test> TestRepository { get; set; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
```

### Specification Pattern

```csharp
public class CategoryEqualsSpecification : ISpecification<Question>
{
    private readonly Category _category;

    public CategoryEqualsSpecification(Category category)
    {
        _category = category;
    }

    public bool IsSatisfied(Question item)
    {
        return item.Category == _category;
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs, Specifications |
| **Infrastructure** | Data access | DbContext, Repositories, EF Migrations |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests/Unit** | Unit tests | Service, Specification, Controller tests |
| **Tests/Integration** | Integration tests | Database, API, MassTransit tests |

## Technology Stack per Layer

| Layer | Technologies | NuGet Packages |
|-------|-------------|----------------|
| Domain | .NET 10, C# | - |
| Application | MediatR (optional), FluentValidation | FluentValidation |
| Infrastructure | EF Core, Npgsql, RabbitMQ | Npgsql.EntityFrameworkCore.PostgreSQL, MassTransit.RabbitMQ |
| WebApi | ASP.NET Core | Microsoft.AspNetCore.Mvc, Microsoft.OpenApi |
| Tests | xUnit, Moq, Refit, FluentAssertions | xunit, Moq, Refit, FluentAssertions |
