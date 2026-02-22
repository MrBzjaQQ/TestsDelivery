# Student Management Service - Testing

## Unit Tests

```csharp
public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly Mock<IStudyGroupRepository> _mockGroupRepository;
    private readonly StudentService _service;

    [Fact]
    public async Task RegisterStudent_Should_ReturnSuccess()
    {
        // Arrange
        var request = new RegisterStudentRequest { /* ... */ };
        
        // Act
        var result = await _service.RegisterStudent(request, CancellationToken.None);
        
        // Assert
        result.IsError.Should().BeFalse();
    }
}
```

## Integration Tests

```csharp
public class StudentsControllerTests : DbTestsBase
{
    [Fact]
    public async Task RegisterStudent_Should_CreateStudent()
    {
        // Arrange
        var request = new RegisterStudentRequest { /* ... */ };
        
        // Act
        var result = await _controller.RegisterStudent(request, CancellationToken.None);
        
        // Assert
        result.IsError.Should().BeFalse();
    }
}
```

## Test Containers

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18.1")
        .Build();
    
    public async Task InitializeAsync() => await _dbContainer.StartAsync();
    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}
```
