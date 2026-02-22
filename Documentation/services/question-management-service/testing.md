# Question Management Service - Testing

## Test Structure

```
QuestionManagementService.Tests/
├── Unit/                              # Unit tests (no dependencies)
│   ├── Services/
│   │   ├── QuestionServiceTests.cs
│   │   ├── QuestionBankServiceTests.cs
│   │   └── TestServiceTests.cs
│   ├── Specifications/
│   │   ├── CategoryEqualsSpecificationTests.cs
│   │   ├── DifficultyEqualsSpecificationTests.cs
│   │   └── TestNotExpiredSpecificationTests.cs
│   └── Controllers/
│       └── QuestionsControllerTests.cs
│
└── Integration/                       # Integration tests (with dependencies)
    ├── TestInfrastructure/            # TestContainers setup
    │   ├── DatabaseFixture.cs
    │   ├── DbTestsBase.cs
    │   └── TestDataSeeder.cs
    ├── Controllers/
    │   ├── QuestionsControllerTests.cs
    │   └── TestsControllerTests.cs
    ├── Repositories/
    │   └── QuestionRepositoryTests.cs
    ├── MassTransit/
    │   └── TestCreatedConsumerTests.cs
    └── Services/
        └── TestGenerationServiceTests.cs
```

## Unit Tests

### QuestionServiceTests

```csharp
public class QuestionServiceTests
{
    private readonly Mock<IQuestionRepository> _mockRepository;
    private readonly Mock<IQuestionBankRepository> _mockBankRepository;
    private readonly QuestionService _service;

    public QuestionServiceTests()
    {
        _mockRepository = new Mock<IQuestionRepository>();
        _mockBankRepository = new Mock<IQuestionBankRepository>();
        _service = new QuestionService(_mockRepository.Object, _mockBankRepository.Object);
    }

    [Fact]
    public async Task CreateQuestion_Should_ReturnSuccess_When_Valid()
    {
        // Arrange
        var request = new CreateQuestionRequest
        {
            Text = "Test question",
            Category = "Math",
            Difficulty = DifficultyLevel.Medium,
            Options = new[] { new AnswerOptionDto { Text = "A", IsCorrect = true } }
        };

        var bank = new QuestionBank { Id = Guid.NewGuid(), Name = "Math Bank" };
        _mockBankRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(bank);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Question>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateQuestion(request, bank.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Question>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateQuestion_Should_ThrowQuestionBankNotFoundException_When_Bank_Not_Exists()
    {
        // Arrange
        var request = new CreateQuestionRequest { /* ... */ };
        var bankId = Guid.NewGuid();

        _mockBankRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((QuestionBank)null);

        // Act & Assert
        await Assert.ThrowsAsync<QuestionBankNotFoundException>(() =>
            _service.CreateQuestion(request, bankId, CancellationToken.None));
    }
}
```

### Specification Tests

```csharp
public class CategoryEqualsSpecificationTests
{
    [Theory]
    [InlineData("Math", "Math", true)]
    [InlineData("Math", "Physics", false)]
    public void IsSatisfied_Should_ReturnExpectedResult(string category, string itemCategory, bool expected)
    {
        // Arrange
        var specification = new CategoryEqualsSpecification(new Category(category));
        var question = new Question { Category = new Category(itemCategory) };

        // Act
        var result = specification.IsSatisfied(question);

        // Assert
        result.Should().Be(expected);
    }
}
```

### Controller Tests

```csharp
public class QuestionsControllerTests
{
    private readonly Mock<IQuestionService> _mockService;
    private readonly QuestionsController _controller;

    public QuestionsControllerTests()
    {
        _mockService = new Mock<IQuestionService>();
        _controller = new QuestionsController(_mockService.Object);
    }

    [Fact]
    public async Task CreateQuestion_Should_ReturnCreatedResult()
    {
        // Arrange
        var request = new CreateQuestionRequest { /* ... */ };
        var response = new CreateQuestionResponse { Id = Guid.NewGuid() };

        _mockService.Setup(s => s.CreateQuestion(It.IsAny<CreateQuestionRequest>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateQuestion(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.IsError.Should().BeFalse();
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
    protected readonly QuestionDbContext DbContext;
    protected readonly UnitOfWork UnitOfWork;

    public DbTestsBase(DatabaseFixture fixture)
    {
        Fixture = fixture;
        
        var options = new DbContextOptionsBuilder<QuestionDbContext>()
            .UseNpgsql(Fixture.DbContainer.GetConnectionString())
            .Options;

        DbContext = new QuestionDbContext(options);
        UnitOfWork = new UnitOfWork(DbContext);
    }

    protected async Task SeedTestDataAsync()
    {
        var seeder = new TestDataSeeder(DbContext);
        await seeder.SeedAsync();
    }
}
```

### TestDataSeeder

```csharp
public class TestDataSeeder
{
    private readonly QuestionDbContext _context;

    public TestDataSeeder(QuestionDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var questionBank = new QuestionBank
        {
            Name = "Test Math Bank",
            Description = "Bank for testing",
            OwnerId = Guid.NewGuid()
        };

        _context.QuestionBanks.Add(questionBank);
        await _context.SaveChangesAsync();

        var question = new Question
        {
            Text = "What is 2+2?",
            Category = new Category("Math"),
            Difficulty = DifficultyLevel.Easy,
            QuestionBankId = questionBank.Id
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
    }
}
```

### Repository Integration Tests

```csharp
[Collection("Database")]
public class QuestionRepositoryTests : DbTestsBase
{
    public QuestionRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetQuery_Should_ReturnQuestions_When_FilteredByCategory()
    {
        // Arrange
        await SeedTestDataAsync();

        var repository = new QuestionRepository(DbContext);
        var category = new Category("Math");

        // Act
        var query = repository.GetQuery(q => q.Category == category);
        var questions = await query.ToListAsync();

        // Assert
        questions.Should().NotBeEmpty();
        questions.Should().AllSatisfy(q => q.Category.Name.Should().Be("Math"));
    }

    [Fact]
    public async Task AddAsync_Should_AddQuestion_And_Save()
    {
        // Arrange
        await SeedTestDataAsync();

        var repository = new QuestionRepository(DbContext);
        var questionBank = await _context.QuestionBanks.FirstAsync();

        var question = new Question
        {
            Text = "New test question",
            Category = new Category("Science"),
            Difficulty = DifficultyLevel.Hard,
            QuestionBankId = questionBank.Id
        };

        // Act
        await repository.AddAsync(question);

        // Assert
        var savedQuestion = await _context.Questions.FindAsync(question.Id);
        savedQuestion.Should().NotBeNull();
        savedQuestion!.Text.Should().Be("New test question");
    }
}
```

### Controller Integration Tests

```csharp
[Collection("Database")]
public class QuestionsControllerTests : DbTestsBase
{
    private readonly QuestionsController _controller;

    public QuestionsControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _controller = new QuestionsController(new QuestionService(
            new QuestionRepository(DbContext),
            new QuestionBankRepository(DbContext)));
    }

    [Fact]
    public async Task CreateQuestion_Should_ReturnSuccess_With_Database()
    {
        // Arrange
        await SeedTestDataAsync();

        var request = new CreateQuestionRequest
        {
            Text = "API Test Question",
            Category = "Math",
            Difficulty = "Medium",
            Options = new[] { new AnswerOptionDto { Text = "A", IsCorrect = true } }
        };

        var questionBank = await _context.QuestionBanks.FirstAsync();

        // Act
        var result = await _controller.CreateQuestion(request, questionBank.Id, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.Id.Should().NotBeEmpty();
    }
}
```

### MassTransit Consumer Tests

```csharp
public class TestCreatedConsumerTests
{
    private readonly Mock<INotificationService> _mockNotificationService;
    private readonly TestCreatedConsumer _consumer;

    public TestCreatedConsumerTests()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _consumer = new TestCreatedConsumer(_mockNotificationService.Object);
    }

    [Fact]
    public async Task Consume_Should_SendNotification()
    {
        // Arrange
        var testCreatedEvent = new TestCreatedEvent
        {
            TestId = Guid.NewGuid(),
            Title = "Test Title",
            CreatedBy = "user-id"
        };

        var context = new Mock<ConsumeContext<TestCreatedEvent>>();
        context.Setup(c => c.Message).Returns(testCreatedEvent);

        // Act
        await _consumer.Consume(context.Object);

        // Assert
        _mockNotificationService.Verify(
            s => s.SendTestCreatedNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>(), CancellationToken.None),
            Times.Once);
    }
}
```

## Test Collections

```csharp
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
```

## Running Tests

### Unit Tests Only

```bash
dotnet test --filter "FullyQualifiedName~Unit"
```

### Integration Tests Only

```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

### All Tests

```bash
dotnet test
```

### With Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=coverage/ /p:CoverletOutputFormat=lcov
```

## Test Patterns

### arrange-act-assert (AAA)

```csharp
[Fact]
public void Method_Should_Behavior()
{
    // Arrange
    var service = CreateService();
    var input = CreateInput();

    // Act
    var result = service.Method(input);

    // Assert
    result.Should().Be(expected);
}
```

### Given-When-Then

```csharp
[Fact]
public void Method_GivenCondition_WhenCalled_ThenResult()
{
    // Given
    var service = CreateService();

    // When
    var result = service.Method();

    // Then
    result.Should().Be(expected);
}
```

## Test Data Builders

```csharp
public class QuestionBuilder
{
    private Question _question = new Question();

    public QuestionBuilder WithDefault()
    {
        _question.Text = "Test question";
        _question.Category = new Category("Math");
        _question.Difficulty = DifficultyLevel.Medium;
        return this;
    }

    public QuestionBuilder WithText(string text)
    {
        _question.Text = text;
        return this;
    }

    public QuestionBuilder WithCategory(string category)
    {
        _question.Category = new Category(category);
        return this;
    }

    public Question Build()
    {
        return _question;
    }
}
```

## Mocking Patterns

### Moq Setup

```csharp
// Setup return value
mock.Setup(m => m.Method(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(response);

// Setup exception
mock.Setup(m => m.Method(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
    .ThrowsAsync(new Exception("Test error"));

// Verify call
mock.Verify(m => m.Method(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
```

## Test Configuration

### xunit.runner.json

```json
{
  "parallelizeTestCollections": false,
  "shadowCopy": false,
  "diagnosticMessages": true
}
```

## CI/CD Testing Pipeline

```yaml
# .gitlab-ci.yml
test:
  stage: test
  script:
    - dotnet restore
    - dotnet build
    - dotnet test --no-build
  artifacts:
    reports:
      junit: test-results.xml
```

## Performance Testing

```csharp
[ Benchmark ]
public async Task GetQuestionById()
{
    var question = await _repository.GetByIdAsync(testQuestionId, CancellationToken.None);
}

// Run with BenchmarkDotNet
// dotnet run -c Release --filter *
```

## Test Coverage Requirements

| Component | Minimum Coverage |
|-----------|------------------|
| Services | 80% |
| Repositories | 70% |
| Controllers | 60% |
| Specifications | 90% |

## Debugging Tests

### VS Code

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Run Unit Tests",
      "type": "dotnet",
      "request": "launch",
      "project": "QuestionManagementService.Tests.Unit",
      "args": [ "--filter", "FullyQualifiedName~Unit" ]
    }
  ]
}
```
