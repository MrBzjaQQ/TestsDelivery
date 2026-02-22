# Question Management Service - Coding Rules

## Code Style

### C# Coding Conventions

#### Naming

| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `QuestionService`, `QuestionController` |
| Interfaces | IPascalCase | `IQuestionService`, `IGenericRepository` |
| Methods | PascalCase | `CreateQuestion`, `GetQuestions` |
| Properties | PascalCase | `QuestionText`, `DifficultyLevel` |
| Private fields | camelCase with _ | `_logger`, `_context` |
| Public fields | PascalCase | `public string Title` |
| Constants | PascalCase | `MaxQuestionsCount` |
| Enum values | PascalCase | `Easy`, `Medium`, `Hard` |

#### File Naming

| File Type | Convention | Example |
|-----------|-----------|---------|
| Class | ClassName.cs | `QuestionService.cs` |
| Controller | ControllerNameController.cs | `QuestionsController.cs` |
| Specification | NameSpecification.cs | `CategoryEqualsSpecification.cs` |
| Repository | EntityNameRepository.cs | `QuestionRepository.cs` |

### Formatting

```csharp
// ✅ Correct
public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(
        IQuestionRepository questionRepository,
        ILogger<QuestionService> logger)
    {
        _questionRepository = questionRepository;
        _logger = logger;
    }

    public async Task<ResponseResultModel<CreateQuestionResponse>> 
        CreateQuestion(CreateQuestionRequest request, Guid questionBankId, CancellationToken ct)
    {
        // Implementation
    }
}

// ❌ Incorrect
public class QuestionService : IQuestionService {
    
    private IQuestionRepository _questionRepository;
    private ILogger<QuestionService> _logger;

    public QuestionService(IQuestionRepository questionRepository,
                            ILogger<QuestionService> logger) {
        
        _questionRepository = questionRepository;
        _logger = logger;
    }

    public async Task<ResponseResultModel<CreateQuestionResponse>> 
        CreateQuestion(CreateQuestionRequest request, Guid questionBankId, CancellationToken ct) {
        
        // Implementation
    }
}
```

### Using Directives

```csharp
// 1. Standard .NET
using System;
using System.Threading;
using System.Threading.Tasks;

// 2. ASP.NET Core
using Microsoft.AspNetCore.Mvc;

// 3. Application-specific
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.Services.Interfaces;

// 4. Third-party
using MassTransit;
using Microsoft.EntityFrameworkCore;

// No namespace (auto-generated)
namespace QuestionManagementService.Application.Services;
```

## Documentation

### XML Comments

```csharp
/// <summary>
/// Creates a new question in the system.
/// </summary>
/// <param name="request">The request containing question data.</param>
/// <param name="questionBankId">The ID of the question bank.</param>
/// <param name="cancellationToken">The cancellation token.</param>
/// <returns>The created question response.</returns>
/// <exception cref="QuestionBankNotFoundException">Thrown when question bank is not found.</exception>
/// <exception cref="ValidationException">Thrown when request data is invalid.</exception>
public async Task<ResponseResultModel<CreateQuestionResponse>> 
    CreateQuestion(CreateQuestionRequest request, Guid questionBankId, CancellationToken cancellationToken)
```

### Comment Style

```csharp
// Single-line comments for simple explanations
// Multi-line comments
// can span multiple lines
// for detailed explanations

/*
 * Block comments for longer explanations
 * or when documenting algorithms
 */

/// <summary>
/// XML documentation for public API
/// </summary>
```

## Exception Handling

### Custom Exceptions

```csharp
public class QuestionNotFoundException : NotFoundException
{
    public QuestionNotFoundException(Guid id) 
        : base($"Question with ID '{id}' was not found.")
    {
        QuestionId = id;
    }

    public Guid QuestionId { get; }
}

public class QuestionBankNotFoundException : NotFoundException
{
    public QuestionBankNotFoundException(Guid id) 
        : base($"Question bank with ID '{id}' was not found.")
    {
        QuestionBankId = id;
    }

    public Guid QuestionBankId { get; }
}
```

### Exception Throwing

```csharp
// ✅ Correct
public async Task<Question> GetByIdAsync(Guid id, CancellationToken ct)
{
    var question = await _context.Questions.FindAsync(id, ct);
    
    if (question == null)
    {
        throw new QuestionNotFoundException(id);
    }
    
    return question;
}

// ❌ Incorrect
public async Task<Question> GetByIdAsync(Guid id, CancellationToken ct)
{
    var question = await _context.Questions.FindAsync(id, ct);
    
    if (question == null)
    {
        throw new Exception($"Question {id} not found"); // Don't use generic Exception
    }
    
    return question;
}
```

## API Response Pattern

### ResponseResultModel<T>

```csharp
// ✅ Success response
return new ResponseResultModel<CreateQuestionResponse>
{
    IsError = false,
    Message = "Question created successfully",
    Data = result
};

// ✅ Error response (handled by exception filter)
throw new QuestionNotFoundException(questionId);

// ✅ Error response with validation
return new ResponseResultModel<CreateQuestionResponse>
{
    IsError = true,
    Message = "Invalid question data",
    Data = null
};
```

## Repository Pattern

### Generic Repository

```csharp
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly QuestionDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(QuestionDbContext context)
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

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        return await _dbSet.AnyAsync(predicate, token);
    }
}
```

### Unit of Work

```csharp
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Question> QuestionRepository { get; }
    IGenericRepository<QuestionBank> QuestionBankRepository { get; }
    IGenericRepository<Test> TestRepository { get; }
    
    Task SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly IQuestionDbContext _context;

    public UnitOfWork(IQuestionDbContext context)
    {
        _context = context;
        QuestionRepository = new QuestionRepository(context);
        QuestionBankRepository = new QuestionBankRepository(context);
        TestRepository = new TestRepository(context);
    }

    public IGenericRepository<Question> QuestionRepository { get; private set; }
    public IGenericRepository<QuestionBank> QuestionBankRepository { get; private set; }
    public IGenericRepository<Test> TestRepository { get; private set; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

## Specification Pattern

```csharp
public interface ISpecification<in T>
{
    bool IsSatisfied(T item);
}

public abstract class ExpressionSpecification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> Expression { get; }
    
    public bool IsSatisfied(T item)
    {
        var predicate = Expression.Compile();
        return predicate(item);
    }
}

public class CategoryEqualsSpecification : ExpressionSpecification<Question>
{
    private readonly Category _category;

    public CategoryEqualsSpecification(Category category)
    {
        _category = category;
    }

    public override Expression<Func<Question, bool>> Expression =>
        q => q.Category == _category;
}
```

## MassTransit Messages

### Command Messages

```csharp
// ✅ Commands (naming: Verb + Entity)
public interface ICreateTestCommand
{
    Guid Id { get; }
    string Title { get; }
}

// ✅ Events (naming: Entity + PastTense)
public interface TestCreatedEvent
{
    Guid TestId { get; }
    string Title { get; }
    DateTimeOffset CreatedAt { get; }
}
```

### Consumer Implementation

```csharp
public class TestCreatedConsumer : IConsumer<TestCreatedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<TestCreatedConsumer> _logger;

    public TestCreatedConsumer(
        INotificationService notificationService,
        ILogger<TestCreatedConsumer> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestCreatedEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Processing test created event: {TestId}", @event.TestId);
        
        await _notificationService.SendTestCreatedNotificationAsync(
            @event.TestId, 
            @event.Title,
            context.CancellationToken);
    }
}
```

## Controller Conventions

### Route Attributes

```csharp
[ApiController]
[Route("api/v1/[controller]")] // Routes to /api/v1/questions
public class QuestionsController : ControllerBase
{
    // Actions will be at:
    // GET /api/v1/questions
    // POST /api/v1/questions
    // GET /api/v1/questions/{id}
}
```

### Action Results

```csharp
// ✅ Correct
[HttpGet("{id}")]
[ProducesResponseType(typeof(ResponseResultModel<QuestionDto>), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ResponseResultModel<QuestionDto>> GetById(Guid id, CancellationToken ct)
{
    try
    {
        var result = await _service.GetByIdAsync(id, ct);
        return new ResponseResultModel<QuestionDto>
        {
            IsError = false,
            Message = "Question retrieved successfully",
            Data = result
        };
    }
    catch (QuestionNotFoundException)
    {
        throw; // Handled by exception filter
    }
}

// ❌ Incorrect
[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    var question = await _context.Questions.FindAsync(id);
    if (question == null) return NotFound();
    return Ok(question);
}
```

## Test Naming

### Unit Test Naming

```csharp
[Fact]
public void Method_Scenario_ExpectedBehavior()
{
    // Arrange
    // Act
    // Assert
}

// Examples:
[Fact]
public void CreateQuestion_WhenValidRequest_ReturnsSuccess()
[Fact]
public void GetQuestion_WhenNotFound_ThrowsQuestionNotFoundException()
[Fact]
public void UpdateQuestion_WhenQuestionExists_UpdatesSuccessfully()
```

## Commit Message Convention

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style (formatting, semicolons)
- `refactor`: Code refactoring
- `test`: Adding tests
- `chore`: Maintenance tasks

### Examples

```
feat(question): add question creation endpoint
fix(test): handle null answer options
docs(api): update question API documentation
refactor(repository): implement generic repository pattern
test(service): add unit tests for QuestionService
fix(question): prevent duplicate question in bank
```

## Git Branching Strategy

```
main (production)
  │
  ├─── develop (staging)
  │       │
  │       ├─── feature/question-management (development)
  │       ├─── feature/student-service
  │       └─── hotfix/db-connection (urgent)
  │
  └─── release-v1.0 (releaseCandidate)
```

### Branch Naming

| Type | Pattern | Example |
|------|---------|---------|
| Feature | `feature/<description>` | `feature/question-bank-api` |
| Fix | `fix/<description>` | `fix/question-not-found` |
| Hotfix | `hotfix/<description>` | `hotfix/db-timeout` |
| Release | `release/<version>` | `release-v1.0` |

## Dependency Injection

### Registration Order

```csharp
public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        // 1. Add controllers
        builder.Services.AddControllers();
        
        // 2. Add OpenAPI/Swagger
        builder.Services.AddOpenApi();
        
        // 3. Add database
        builder.Services.AddDatabase(settings.ConnectionString);
        
        // 4. Add application services
        builder.Services.AddApplicationServices();
        
        // 5. Add infrastructure services
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
        
        // 6. Add exception handling
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        // 7. Add factories
        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        
        // 8. Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

### Lifetime Scopes

| Lifetime | Usage | Example |
|----------|-------|---------|
| `Singleton` | One instance per application | Factories, settings |
| `Scoped` | One instance per request | Services, controllers |
| `Transient` | New instance every time | Repositories, clients |

## Performance Guidelines

### Database

```csharp
// ✅ Use AsNoTracking for read-only queries
var questions = await _context.Questions
    .AsNoTracking()
    .Where(q => q.Difficulty == difficulty)
    .ToListAsync(ct);

// ✅ Use compiled queries for frequently executed queries
private static readonly Func<QuestionDbContext, Guid, Task<Question?>> 
    _getQuestionById = EF.CompileAsyncQuery(
        (QuestionDbContext ctx, Guid id) => 
            ctx.Questions.FirstOrDefaultAsync(q => q.Id == id));

// ❌ Avoid N+1 queries
var questions = await _context.Questions.ToListAsync(ct);
foreach (var q in questions)
{
    var options = await _context.AnswerOptions
        .Where(ao => ao.QuestionId == q.Id)
        .ToListAsync(ct); // N+1 problem
}
```

### Caching

```csharp
public async Task<List<Question>> GetQuestionsByCategoryAsync(Category category, CancellationToken ct)
{
    var cacheKey = $"questions:{category.Name}";
    
    var cached = await _cache.GetStringAsync(cacheKey, ct);
    if (!string.IsNullOrEmpty(cached))
    {
        return JsonSerializer.Deserialize<List<Question>>(cached);
    }
    
    var questions = await _repository.GetQuestionsByCategory(category, ct);
    
    await _cache.SetStringAsync(cacheKey, 
        JsonSerializer.Serialize(questions), 
        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) },
        ct);
    
    return questions;
}
```

## Security Best Practices

### Input Validation

```csharp
// ✅ Use data annotations
public class CreateQuestionRequest
{
    [Required]
    [MaxLength(2000)]
    public string Text { get; set; }
    
    [Required]
    [Range(1, 3)]
    public int Difficulty { get; set; }
}

// ✅ Validate in controller/action filters
[HttpPost]
public async Task<IActionResult> Create([FromBody, ValidateModel] CreateQuestionRequest request)
{
    // Implementation
}
```

### SQL Injection Prevention

```csharp
// ✅ Use EF Core parameterization
var questions = await _context.Questions
    .Where(q => q.Category == categoryParameter)
    .ToListAsync(ct);

// ❌ Never use string concatenation
var questions = await _context.Questions
    .FromSqlRaw($"SELECT * FROM questions WHERE category = '{categoryInput}'") // SQL Injection risk!
    .ToListAsync(ct);
```

## Monitoring & Observability

### Logging

```csharp
// ✅ Structure logging with correlation
_logger.LogInformation("Processing question creation for bank {QuestionBankId}", questionBankId);

// ✅ Log exceptions with context
try
{
    // Operation
}
catch (Exception ex)
{
    _logger.LogError(ex, "Failed to create question for bank {QuestionBankId}", questionBankId);
    throw;
}

// ✅ Log at appropriate levels
_logger.LogTrace("Detailed trace information");
_logger.LogDebug("Debug information");
_logger.LogInformation("General information");
_logger.LogWarning("Warning condition");
_logger.LogError(exception, "Error occurred");
_logger.LogCritical(exception, "Critical error");
```

### Metrics

```csharp
// ✅ Use Prometheus metrics
_counter = _meter.CreateCounter<int>("questions_created_total");

public async Task CreateQuestionAsync(...)
{
    // ...
    _counter.Add(1, new KeyValuePair<string, object?>("category", category));
}
```

## Code Quality Tools

### .editorconfig

```ini
# .editorconfig
root = true

[*.{cs,csx}]
indent_style = space
indent_size = 4
max_line_length = 120
charset = utf-8

# Naming rules
dotnet_naming_rule classes_should_be_pascal_case.symbols = class
dotnet_naming_style PascalCase = C:
dotnet_naming_rule interfaces_should_be_i_pascal_case.symbols = interface
dotnet_naming_style ICapital = I:C:
```

### StyleCop Analysis

```xml
<!-- StyleCopAnalyzers settings -->
<Rule AnalyzerId="StyleCop.Analyzers" RuleId="SA1633">
  <Severity>warning</Severity>
</Rule>
```

## CI/CD Integration

### Code Analysis

```bash
# Run analyzers
dotnet build --warn-as-error

# Run tests
dotnet test --filter "TestCategory!=Integration"

# Code coverage
dotnet test /p:CollectCoverage=true
```

### Pre-commit Hooks

```bash
#!/bin/bash
# .git/hooks/pre-commit

# Run code analysis
dotnet build

# Run tests
dotnet test --filter "TestCategory=Unit"

# Check formatter
dotnet format --verify

if [ $? -ne 0 ]; then
    echo "Pre-commit checks failed!"
    exit 1
fi
```
