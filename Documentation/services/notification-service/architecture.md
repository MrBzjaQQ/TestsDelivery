# Notification Service - Architecture

## Структура проекта

### Слой Domain

```
NotificationService.Domain/
├── Entities/
│   ├── Notification.cs                # Уведомление
│   ├── EmailMessage.cs                # Email сообщение
│   └── NotificationTemplate.cs        # Шаблон письма
├── ValueObjects/
│   ├── NotificationType.cs            # Тип (Email, Push)
│   ├── NotificationStatus.cs          # Статус (Pending, Sent, Failed)
│   └── EmailAddress.cs                # Email
└── Exceptions/
    ├── NotificationNotFoundException.cs
    └── EmailSendFailedException.cs
```

### Слой Application

```
NotificationService.Application/
├── Services/
│   ├── EmailNotificationService.cs
│   ├── NotificationQueueService.cs
│   └── TemplateService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── SendEmailRequest.cs
│   │   ├── TestCreatedNotificationRequest.cs
│   │   └── TestResultNotificationRequest.cs
│   └── Responses/
│       ├── SendEmailResponse.cs
│       └── NotificationStatusDto.cs
├── Specifications/
│   ├── NotificationSpecs/
│   │   ├── PendingNotificationSpecification.cs
│   │   └── EmailValidSpecification.cs
│   └── TemplateSpecs/
│       ├── TemplateExistsSpecification.cs
│       └── RequiredTokensSpecification.cs
├── Contracts/
│   ├── IEmailNotificationService.cs
│   └── INotificationQueueService.cs
└── Exceptions/
    └── TemplateRenderException.cs
```

### Слой Infrastructure

```
NotificationService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── NotificationDbContext.cs
│   │   └── INotificationDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_NotificationTables.cs
│   │   └── NotificationDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   └── NotificationRepository.cs
├── MassTransit/
│   ├── Consumer/
│   │   ├── TestCreatedConsumer.cs
│   │   ├── TestResultConsumer.cs
│   │   └── UserRegisteredConsumer.cs
│   └── MessageTypes/
│       ├── TestCreatedEvent.cs
│       ├── TestResultReceivedEvent.cs
│       └── UserRegisteredEvent.cs
└── Email/
    ├── EmailSender.cs
    ├── HtmlTemplateRenderer.cs
    └── SmtpClientFactory.cs
```

### Слой WebApi

```
NotificationService.WebApi/
├── Controllers/
│   ├── NotificationsController.cs
│   └── TemplatesController.cs
├── Settings/
│   ├── AppSettings.cs
│   └── SmtpSettings.cs
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

### Layer Flow

```
RabbitMQ Message
    ↓
MassTransit Consumer
    ↓
Application (Service)
    ↓
Infrastructure (EmailSender)
    ↓
SMTP Server
```

## Dependency Injection

```csharp
public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        
        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();
        builder.Services.AddEmailSender(settings.Smtp);
        
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### Email Notification Service

```csharp
public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailSender _emailSender;
    private readonly ITemplateService _templateService;
    private readonly ILogger<EmailNotificationService> _logger;

    public async Task SendEmailAsync(SendEmailRequest request, CancellationToken ct)
    {
        // Render template
        var htmlBody = await _templateService.RenderAsync(
            request.TemplateName,
            request.TemplateData,
            ct);

        // Create email
        var emailMessage = new EmailMessage
        {
            To = request.To,
            From = _settings.DefaultFrom,
            Subject = request.Subject,
            HtmlBody = htmlBody,
            IsHtml = true
        };

        // Send email
        await _emailSender.SendAsync(emailMessage, ct);

        // Log
        _logger.LogInformation("Email sent to {To} with subject {Subject}", request.To, request.Subject);
    }
}
```

### MassTransit Consumer

```csharp
public class TestCreatedConsumer : IConsumer<TestCreatedEvent>
{
    private readonly IEmailNotificationService _emailService;
    private readonly ILogger<TestCreatedConsumer> _logger;

    public async Task Consume(ConsumeContext<TestCreatedEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Processing test created notification for {TestTitle}", @event.Title);
        
        // Render template
        var templateData = new
        {
            TestTitle = @event.Title,
            StudentName = @event.StudentName,
            TestLink = $"https://portal.com/tests/{@event.TestId}"
        };

        var sendRequest = new SendEmailRequest
        {
            To = @event.StudentEmail,
            Subject = $"New Test Available: {@event.Title}",
            TemplateName = "test-created",
            TemplateData = templateData
        };

        await _emailService.SendEmailAsync(sendRequest, context.CancellationToken);
    }
}
```

### Email Sender

```csharp
public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<EmailSender> _logger;

    public async Task SendAsync(EmailMessage message, CancellationToken ct)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
        mimeMessage.To.Add(new MailboxAddress("", message.To));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart(message.IsHtml ? "html" : "plain")
        {
            Text = message.HtmlBody ?? message.PlainTextBody
        };

        try
        {
            await _smtpClient.SendAsync(mimeMessage, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", message.To);
            throw new EmailSendFailedException(message.To, ex);
        }
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs, Specifications |
| **Infrastructure** | Data access | DbContext, Repositories, Email, MassTransit |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests** | Tests | Unit and Integration tests |