# Test Checking Service - Testing

## Test Structure

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
│
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

## Unit Tests

### TestCheckServiceTests

```csharp
public class TestCheckServiceTests
{
    private readonly Mock<ITestResultRepository> _mockResultRepository;
    private readonly Mock<IRubricCalculator> _mockRubricCalculator;
    private readonly TestCheckService _service;

    public TestCheckServiceTests()
    {
        _mockResultRepository = new Mock<ITestResultRepository>();
        _mockRubricCalculator = new Mock<IRubricCalculator>();
        _service = new TestCheckService(_mockResultRepository.Object, _mockRubricCalculator.Object);
    }

    [Fact]
    public async Task CheckTest_Should_CalculateScoreAndPass()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto>
        {
            new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid(), IsCorrect = true },
            new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid(), IsCorrect = true }
        };

        _mockRubricCalculator.Setup(c => c.CalculateScoreAsync(It.IsAny<Guid>(), answers, CancellationToken.None))
            .ReturnsAsync(new ScoringResult
            {
                TotalScore = 17,
                MaxScore = 20,
                Percentage = 85,
                IsPassed = true
            });

        _mockResultRepository.Setup(r => r.AddAsync(It.IsAny<TestResult>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CheckTestAsync(testId, studentId, answers, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.IsPassed.Should().BeTrue();
        result.Data.Percentage.Should().Be(85);
    }

    [Fact]
    public async Task CheckTest_Should_CalculateScoreAndFail()
    {
        // Arrange
        var answers = new List<AnswerDto>
        {
            new AnswerDto { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid(), IsCorrect = false }
        };

        _mockRubricCalculator.Setup(c => c.CalculateScoreAsync(It.IsAny<Guid>(), answers, CancellationToken.None))
            .ReturnsAsync(new ScoringResult
            {
                TotalScore = 5,
                MaxScore = 20,
                Percentage = 25,
                IsPassed = false
            });

        // Act
        var result = await _service.CheckTestAsync(Guid.NewGuid(), Guid.NewGuid(), answers, CancellationToken.None);

        // Assert
        result.Data.IsPassed.Should().BeFalse();
        result.Data.Percentage.Should().Be(25);
    }
}
```

### AnswerEvaluatorTests

```csharp
public class AnswerEvaluatorTests
{
    [Theory]
    [InlineData(true, 10)]
    [InlineData(false, 0)]
    public void EvaluateAnswer_Should_ReturnExpectedPoints(bool isCorrect, int expectedPoints)
    {
        // Arrange
        var evaluator = new AnswerEvaluator();
        var question = new Question { Points = 10 };
        var answer = new AnswerDto { SelectedOptionId = isCorrect ? "correct" : "wrong" };

        // Act
        var result = evaluator.EvaluateAnswer(question, answer);

        // Assert
        result.PointsEarned.Should().Be(expectedPoints);
        result.IsCorrect.Should().Be(isCorrect);
    }
}
```

## Integration Tests

### DatabaseFixture

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = CreateDbContainer();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }

    private static PostgreSqlContainer CreateDbContainer()
    {
        return new PostgreSqlBuilder()
            .WithImage("postgres:18.1")
            .Build();
    }
}
```

### DbTestsBase

```csharp
[DatabaseCollection]
public class DbTestsBase : IClassFixture<DatabaseFixture>
{
    protected readonly DatabaseFixture Fixture;
    protected readonly TestCheckingDbContext DbContext;

    public DbTestsBase(DatabaseFixture fixture)
    {
        Fixture = fixture;
        
        var options = new DbContextOptionsBuilder<TestCheckingDbContext>()
            .UseNpgsql(Fixture.DbContainer.GetConnectionString())
            .Options;

        DbContext = new TestCheckingDbContext(options);
    }

    protected async Task SeedTestDataAsync()
    {
        var seeder = new TestDataSeeder(DbContext);
        await seeder.SeedAsync();
    }
}
```

### TestCheckControllerTests (Integration)

```csharp
[Collection("Database")]
public class TestCheckControllerTests : DbTestsBase
{
    private readonly TestCheckController _controller;

    public TestCheckControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _controller = new TestCheckController(
            new TestCheckService(
                new TestResultRepository(DbContext),
                new RubricCalculator(DbContext)));
    }

    [Fact]
    public async Task CheckTest_Should_CheckTest_With_Database()
    {
        // Arrange
        await SeedTestDataAsync();

        var testId = await _context.Tests.FirstAsync().Result.Id;
        var studentId = Guid.NewGuid();
        var answers = new List<AnswerDto> { /* ... */ };

        // Act
        var result = await _controller.CheckTest(testId, new CheckTestRequest { StudentId = studentId, Answers = answers }, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.IsPassed.Should().BeTrue();
    }
}
```

### AnswerEvaluatorIntegrationTests

```csharp
[Collection("Database")]
public class AnswerEvaluatorIntegrationTests : DbTestsBase
{
    private readonly AnswerEvaluator _evaluator;

    public AnswerEvaluatorIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _evaluator = new AnswerEvaluator(new QuestionRepository(DbContext));
    }

    [Fact]
    public async Task EvaluateQuestion_Should_ReturnCorrectPoints()
    {
        // Arrange
        await SeedTestDataAsync();

        var question = await _context.Questions.FirstAsync();
        var answer = new AnswerDto
        {
            SelectedOptionId = question.CorrectOptionId
        };

        // Act
        var result = await _evaluator.EvaluateQuestionAsync(question.Id, answer, CancellationToken.None);

        // Assert
        result.IsCorrect.Should().BeTrue();
        result.PointsEarned.Should().Be(question.Points);
    }
}
```

## MassTransit Consumer Tests

```csharp
public class TestSubmittedConsumerTests
{
    private readonly Mock<ITestCheckService> _mockTestCheckService;
    private readonly TestSubmittedConsumer _consumer;

    public TestSubmittedConsumerTests()
    {
        _mockTestCheckService = new Mock<ITestCheckService>();
        _consumer = new TestSubmittedConsumer(_mockTestCheckService.Object);
    }

    [Fact]
    public async Task Consume_Should_CheckTest_And_Publish_Result()
    {
        // Arrange
        var testSubmittedEvent = new TestSubmittedEvent
        {
            TestId = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            Answers = new List<AnswerDto>()
        };

        var context = new Mock<ConsumeContext<TestSubmittedEvent>>();
        context.Setup(c => c.Message).Returns(testSubmittedEvent);
        context.Setup(c => c.PublishAsync(It.IsAny<TestResultReceivedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockTestCheckService.Setup(s => s.CheckTestAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<AnswerDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResponseResultModel<TestScoringResult> { Data = new TestScoringResult { IsPassed = true } });

        // Act
        await _consumer.Consume(context.Object);

        // Assert
        _mockTestCheckService.Verify(s => s.CheckTestAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<List<AnswerDto>>(), It.IsAny<CancellationToken>()), Times.Once);
        context.Verify(c => c.PublishAsync(It.IsAny<TestResultReceivedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

## Running Tests

```bash
# All tests
dotnet test

# Unit tests only
dotnet test --filter "FullyQualifiedName~Unit"

# Integration tests only
dotnet test --filter "FullyQualifiedName~Integration"
```