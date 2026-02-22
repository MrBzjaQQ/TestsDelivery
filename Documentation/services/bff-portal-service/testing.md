# Bff Portal Service - Testing

## Test Structure

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
    │   └── HttpClientFixture.cs
    └── Services/
        └── StudentPortalServiceIntegrationTests.cs
```

## Unit Tests

### StudentPortalServiceTests

```csharp
public class StudentPortalServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly StudentPortalService _service;

    [Fact]
    public async Task GetStudentProfileAsync_Should_ReturnProfile()
    {
        // Arrange
        var studentProfile = new StudentProfileDto
        {
            StudentId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        };

        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient { BaseAddress = new Uri("http://localhost") });

        _mockTokenService.Setup(t => t.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake-token");

        _service = new StudentPortalService(_mockHttpClientFactory.Object, _mockTokenService.Object, _mockCacheService.Object);

        // Act
        var result = await _service.GetStudentProfileAsync(studentProfile.StudentId, "token", CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetStudentProfileAsync_Should_ThrowServiceUnavailableException()
    {
        // Arrange
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient { BaseAddress = new Uri("http://localhost") });

        _mockTokenService.Setup(t => t.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync("token");

        _service = new StudentPortalService(_mockHttpClientFactory.Object, _mockTokenService.Object, _mockCacheService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ServiceUnavailableException>(() =>
            _service.GetStudentProfileAsync(Guid.NewGuid(), "token", CancellationToken.None));
    }
}
```

## Integration Tests

### DatabaseFixture

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18.1")
        .Build();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync() => await DbContainer.StartAsync();
    public async Task DisposeAsync() => await DbContainer.DisposeAsync();
}
```

### HttpClientFixture

```csharp
// Use MockHttpMessageHandler for HTTP client tests
public class HttpClientFixture
{
    public MockHttpMessageHandler CreateMockHandler()
    {
        var mockHandler = new MockHttpMessageHandler();
        
        mockHandler.When("http://student-service/api/v1/students/*")
            .Respond(HttpStatusCode.OK, "application/json", @"{ ""studentId"": ""123"", ""firstName"": ""John"" }");
        
        mockHandler.When("http://test-checking-service/api/v1/students/*")
            .Respond(HttpStatusCode.OK, "application/json", @"{ ""results"": [] }");
        
        return mockHandler;
    }
}
```

## Running Tests

```bash
dotnet test
dotnet test --filter "FullyQualifiedName~Unit"
dotnet test --filter "FullyQualifiedName~Integration"
```