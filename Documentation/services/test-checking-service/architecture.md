# Test Checking Service - Architecture

## Структура проекта

### Слой Domain

```
TestCheckingService.Domain/
├── Entities/
│   ├── TestResult.cs                  # Результат теста
│   ├── AnswerResult.cs                # Результат ответа на вопрос
│   └── Rubric.cs                      # Шкала оценок
├── ValueObjects/
│   ├── Score.cs                       # Оценка
│   ├── Percentage.cs                  # Процент выполнения
│   └── TestStatus.cs                  # Статус (Pending, Checked, Passed, Failed)
└── Exceptions/
    ├── TestResultNotFoundException.cs
    └── TestNotEligibleException.cs
```

### Слой Application

```
TestCheckingService.Application/
├── Services/
│   ├── TestCheckService.cs
│   ├── ResultService.cs
│   └── RubricService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── CheckTestRequest.cs
│   │   ├── CheckTestBatchRequest.cs
│   │   └── AnswerDto.cs
│   └── Responses/
│       ├── TestResultDto.cs
│       ├── AnswerResultDto.cs
│       └── ScoringReportDto.cs
├── Specifications/
│   ├── TestResultSpecs/
│   │   ├── TestNotExpiredSpecification.cs
│   │   └── StudentEligibleSpecification.cs
│   └── RubricSpecs/
│       ├── PassThresholdSpecification.cs
│       └── GradeCalculationSpecification.cs
├── Contracts/
│   ├── ITestCheckService.cs
│   └── IResultService.cs
└── Exceptions/
    └── ScoringException.cs
```

### Слой Infrastructure

```
TestCheckingService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── TestCheckingDbContext.cs
│   │   �└── ITestCheckingDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_TestResultTables.cs
│   │   └── TestCheckingDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   ├── TestResultRepository.cs
│   └── AnswerResultRepository.cs
├── MassTransit/
│   ├── Consumer/
│   │   ├── TestSubmittedConsumer.cs
│   │   └── QuestionAnsweredConsumer.cs
│   └── MessageTypes/
│       ├── TestSubmittedEvent.cs
│       └── TestResultReceivedEvent.cs
└── Scoring/
    ├── ScoringEngine.cs
    ├── RubricCalculator.cs
    └── AnswerEvaluator.cs
```

### Слой WebApi

```
TestCheckingService.WebApi/
├── Controllers/
│   ├── TestCheckController.cs
│   ├── ResultsController.cs
│   └── ScoringController.cs
├── Settings/
│   ├── AppSettings.cs
│   └── ScoringSettings.cs
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
TestCheckingService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── TestCheckServiceTests.cs
│   │   ├── ResultServiceTests.cs
│   │   └── AnswerEvaluatorTests.cs
│   ├── Specifications/
│   │   └── StudentEligibleSpecificationTests.cs
│   └── Controllers/
│       └── TestCheckControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   ├── DbTestsBase.cs
    │   └── TestDataSeeder.cs
    ├── Controllers/
    │   └── TestCheckControllerTests.cs
    └── Services/
        └── AnswerEvaluatorIntegrationTests.cs
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
Infrastructure (Repository/Scoring Engine)
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
        
        // Scoring
        builder.Services.AddScoringEngine();
        
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

### Scoring Engine

```csharp
public class ScoringEngine : IScoringEngine
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IRubricCalculator _rubricCalculator;

    public ScoringEngine(
        IQuestionRepository questionRepository,
        IRubricCalculator rubricCalculator)
    {
        _questionRepository = questionRepository;
        _rubricCalculator = rubricCalculator;
    }

    public async Task<TestScoringResult> ScoreTestAsync(
        Guid testId,
        List<AnswerDto> studentAnswers,
        CancellationToken ct)
    {
        var testQuestions = await _questionRepository.GetTestQuestionsAsync(testId, ct);
        
        var answerResults = new List<AnswerResult>();
        var totalScore = 0;
        var maxScore = 0;

        foreach (var answerDto in studentAnswers)
        {
            var question = testQuestions.FirstOrDefault(q => q.Id == answerDto.QuestionId);
            if (question == null) continue;

            var answerResult = EvaluateAnswer(question, answerDto);
            answerResults.Add(answerResult);

            totalScore += answerResult.PointsEarned;
            maxScore += question.MaxScore;
        }

        var percentage = (double)totalScore / maxScore * 100;
        var isPassed = percentage >= test.PassThreshold;

        return new TestScoringResult
        {
            TestId = testId,
            TotalScore = totalScore,
            MaxScore = maxScore,
            Percentage = percentage,
            IsPassed = isPassed,
            AnswerResults = answerResults
        };
    }

    private AnswerResult EvaluateAnswer(Question question, AnswerDto answerDto)
    {
        var isCorrect = answerDto.SelectedOptionId == question.CorrectOptionId;
        
        return new AnswerResult
        {
            QuestionId = question.Id,
            SelectedOptionId = answerDto.SelectedOptionId,
            IsCorrect = isCorrect,
            PointsEarned = isCorrect ? question.Points : 0
        };
    }
}
```

### MassTransit Consumer

```csharp
public class TestSubmittedConsumer : IConsumer<TestSubmittedEvent>
{
    private readonly ITestCheckService _testCheckService;
    private readonly ILogger<TestSubmittedConsumer> _logger;

    public TestSubmittedConsumer(
        ITestCheckService testCheckService,
        ILogger<TestSubmittedConsumer> logger)
    {
        _testCheckService = testCheckService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestSubmittedEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Processing test submission for test {TestId}, student {StudentId}",
            @event.TestId, @event.StudentId);
        
        var result = await _testCheckService.CheckTestAsync(
            @event.TestId,
            @event.StudentId,
            @event.Answers,
            context.CancellationToken);
        
        // Publish result event
        await context.Publish(new TestResultReceivedEvent
        {
            TestId = @event.TestId,
            StudentId = @event.StudentId,
            Score = result.TotalScore,
            MaxScore = result.MaxScore,
            IsPassed = result.IsPassed,
            Percentage = result.Percentage
        }, context.CancellationToken);
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs, Specifications, Scoring |
| **Infrastructure** | Data access | DbContext, Repositories, MassTransit, Scoring Engine |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests/Unit** | Unit tests | Service, Specification, Controller tests |
| **Tests/Integration** | Integration tests | Database, API, MassTransit tests |