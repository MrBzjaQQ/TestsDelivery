# Notification Service - Testing

## Test Structure

```
NotificationService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── EmailNotificationServiceTests.cs
│   │   └── TemplateServiceTests.cs
│   └── Controllers/
│       └── NotificationsControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   └── SmtpFixture.cs
    └── Services/
        └── EmailNotificationServiceIntegrationTests.cs
```

## Unit Tests

### EmailNotificationServiceTests

```csharp
public class EmailNotificationServiceTests
{
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly Mock<ITemplateService> _mockTemplateService;
    private readonly EmailNotificationService _service;

    [Fact]
    public async Task SendEmail_Should_CallEmailSender()
    {
        // Arrange
        var request = new SendEmailRequest
        {
            To = "test@example.com",
            Subject = "Test Subject",
            TemplateName = "test-created",
            TemplateData = new { testTitle = "Math" }
        };

        _mockTemplateService.Setup(s => s.RenderAsync("test-created", It.IsAny<object>(), CancellationToken.None))
            .ReturnsAsync("<html>Test Email</html>");

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        _service = new EmailNotificationService(_mockEmailSender.Object, _mockTemplateService.Object);

        // Act
        await _service.SendEmailAsync(request, CancellationToken.None);

        // Assert
        _mockEmailSender.Verify(s => s.SendAsync(It.IsAny<EmailMessage>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SendEmail_Should_ThrowEmailSendFailedException()
    {
        // Arrange
        _mockTemplateService.Setup(s => s.RenderAsync(It.IsAny<string>(), It.IsAny<object>(), CancellationToken.None))
            .ReturnsAsync("<html>Test</html>");

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<EmailMessage>(), CancellationToken.None))
            .ThrowsAsync(new SmtpException("Connection failed"));

        _service = new EmailNotificationService(_mockEmailSender.Object, _mockTemplateService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EmailSendFailedException>(() =>
            _service.SendEmailAsync(new SendEmailRequest { /* ... */ }, CancellationToken.None));
    }
}
```

### TemplateServiceTests

```csharp
public class TemplateServiceTests
{
    [Fact]
    public async Task RenderAsync_Should_Render_Template_With_Data()
    {
        // Arrange
        var service = new TemplateService("Templates/en");
        var templateData = new { testTitle = "Math Midterm", studentName = "John" };

        // Act
        var result = await service.RenderAsync("test-created", templateData, CancellationToken.None);

        // Assert
        result.Should().Contain("Math Midterm");
        result.Should().Contain("John");
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

### SmtpFixture (For Integration Tests)

```csharp
// Use MailHog or similar SMTP testing server
public class SmtpFixture : IAsyncLifetime
{
    private readonly MailHogContainer _smtpContainer = new MailHogBuilder()
        .WithImage("mailhog/mailhog:latest")
        .Build();

    public MailHogContainer SmtpContainer => _smtpContainer;

    public async Task InitializeAsync() => await SmtpContainer.StartAsync();
    public async Task DisposeAsync() => await SmtpContainer.DisposeAsync();
}
```

## MassTransit Consumer Tests

```csharp
public class TestCreatedConsumerTests
{
    private readonly Mock<IEmailNotificationService> _mockEmailService;
    private readonly TestCreatedConsumer _consumer;

    [Fact]
    public async Task Consume_Should_Send_Email()
    {
        // Arrange
        var @event = new TestCreatedEvent
        {
            TestId = Guid.NewGuid(),
            StudentEmail = "student@example.com",
            StudentName = "John Doe",
            Title = "Math Midterm"
        };

        var context = new Mock<ConsumeContext<TestCreatedEvent>>();
        context.Setup(c => c.Message).Returns(@event);

        _mockEmailService.Setup(s => s.SendEmailAsync(It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _consumer = new TestCreatedConsumer(_mockEmailService.Object);

        // Act
        await _consumer.Consume(context.Object);

        // Assert
        _mockEmailService.Verify(s => s.SendEmailAsync(It.IsAny<SendEmailRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

## Running Tests

```bash
dotnet test
dotnet test --filter "FullyQualifiedName~Unit"
dotnet test --filter "FullyQualifiedName~Integration"
```