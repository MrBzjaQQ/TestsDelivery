# MassTransit Events Flow - TestsDelivery

## Architecture Overview

All inter-service communication in TestsDelivery is built around **event-driven architecture** using **MassTransit with RabbitMQ**. Services publish events when significant state changes occur, and other services subscribe to relevant events to trigger their business logic.

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│                 │     │                 │     │                 │
│   Identity      │────>│   Question      │────>│   Student       │
│   Service       │     │   Management    │     │   Management    │
│                 │     │   Service       │     │   Service       │
└─────────────────┘     └─────────────────┘     └─────────────────┘
        │                       │                       │
        │                       │                       │
        ▼                       ▼                       ▼
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│  RabbitMQ       │     │  RabbitMQ       │     │  RabbitMQ       │
│  exchanges      │     │  queues         │     │  consumers      │
│  consumers      │     │  topics         │     │  handlers       │
└─────────────────┘     └─────────────────┘     └─────────────────┘
        │                       │                       │
        ▼                       ▼                       ▼
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│                 │     │                 │     │                 │
│   Notification  │     │   Test Checking │     │   File Storage  │
│   Service       │     │   Service       │     │   Service       │
│                 │     │                 │     │                 │
└─────────────────┘     └─────────────────┘     └─────────────────┘
        │
        ▼
┌─────────────────┐
│   BFF Portal    │
│   Service       │
│                 │
└─────────────────┘
```

## Event Types by Service

### 1. Identity Service Events

#### UserRegisteredEvent
Published when a new user is registered.

```csharp
public interface UserRegisteredEvent
{
    Guid UserId { get; }
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    string Role { get; }
    DateTime RegisteredAt { get; }
}
```

**Consumer:** NotificationService (sends welcome email)

---

#### UserEmailVerifiedEvent
Published when user email is verified.

```csharp
public interface UserEmailVerifiedEvent
{
    Guid UserId { get; }
    string Email { get; }
    DateTime VerifiedAt { get; }
}
```

---

#### UserPasswordResetEvent
Published when password reset is requested.

```csharp
public interface UserPasswordResetEvent
{
    Guid UserId { get; }
    string Email { get; }
    string ResetToken { get; }
    DateTime ExpiresAt { get; }
}
```

---

### 2. Question Management Service Events

#### TestCreatedEvent
Published when a new test is created or copied.

```csharp
public interface TestCreatedEvent
{
    Guid TestId { get; }
    string Title { get; }
    string Description { get; }
    Guid QuestionBankId { get; }
    Guid? TemplateId { get; }
    int DurationMinutes { get; }
    int PassingScore { get; }
    int MaxAttempts { get; }
    DateTime CreatedAt { get; }
    Guid CreatedBy { get; }
}
```

**Consumers:**
- **StudentManagementService**: Notifies students about new tests
- **NotificationService**: Sends email notifications to students

---

#### TestTemplateCreatedEvent
Published when a new test template is created.

```csharp
public interface TestTemplateCreatedEvent
{
    Guid TemplateId { get; }
    string Name { get; }
    string Description { get; }
    int DefaultDuration { get; }
    int DefaultPassingScore { get; }
    TemplateConfiguration Configuration { get; }
    DateTime CreatedAt { get; }
}
```

---

#### QuestionBankCreatedEvent
Published when a new question bank is created.

```csharp
public interface QuestionBankCreatedEvent
{
    Guid QuestionBankId { get; }
    string Name { get; }
    string Description { get; }
    Guid OwnerId { get; }
    DateTime CreatedAt { get; }
}
```

---

#### QuestionCreatedEvent
Published when a new question is added to a bank.

```csharp
public interface QuestionCreatedEvent
{
    Guid QuestionId { get; }
    Guid QuestionBankId { get; }
    string Text { get; }
    string Category { get; }
    string Difficulty { get; }
    List<AnswerOption> Options { get; }
    Guid? ImageFileId { get; }
    DateTime CreatedAt { get; }
}
```

---

### 3. Student Management Service Events

#### TestAssignedEvent
Published when a test is assigned to a student.

```csharp
public interface TestAssignedEvent
{
    Guid TestAssignmentId { get; }
    Guid TestId { get; }
    Guid StudentId { get; }
    string StudentName { get; }
    string StudentEmail { get; }
    string TestTitle { get; }
    DateTime AssignedAt { get; }
    DateTime Deadline { get; }
    int AttemptsAllowed { get; }
}
```

**Consumers:**
- **NotificationService**: Sends email notification with test link
- **BffPortalService**: Updates available tests cache

---

#### TestStartedEvent
Published when student starts a test.

```csharp
public interface TestStartedEvent
{
    Guid TestAssignmentId { get; }
    Guid TestId { get; }
    Guid StudentId { get; }
    string StudentName { get; }
    DateTime StartedAt { get; }
    DateTime Deadline { get; }
    int AttemptNumber { get; }
}
```

---

#### TestSubmittedEvent
Published when student submits answers.

```csharp
public interface TestSubmittedEvent
{
    Guid TestAssignmentId { get; }
    Guid TestId { get; }
    Guid StudentId { get; }
    string StudentName { get; }
    List<AnswerSubmission> Answers { get; }
    DateTime SubmittedAt { get; }
    int AttemptNumber { get; }
}
```

**Consumers:**
- **TestCheckingService**: Scores the test
- **BffPortalService**: Updates student progress

---

#### StudentProfileUpdatedEvent
Published when student profile is updated.

```csharp
public interface StudentProfileUpdatedEvent
{
    Guid StudentId { get; }
    Guid UserId { get; }
    string FirstName { get; }
    string LastName { get; }
    string? PhoneNumber { get; }
    string? AvatarUrl { get; }
    string? Bio { get; }
    DateTime UpdatedAt { get; }
}
```

---

### 4. Test Checking Service Events

#### TestResultReceivedEvent
Published when test is scored and result is calculated.

```csharp
public interface TestResultReceivedEvent
{
    Guid TestId { get; }
    Guid StudentId { get; }
    string StudentName { get; }
    string StudentEmail { get; }
    int Score { get; }
    int MaxScore { get; }
    double Percentage { get; }
    bool IsPassed { get; }
    int PassedThreshold { get; }
    DateTime PassedDate { get; }
    int AttemptNumber { get; }
}
```

**Consumers:**
- **NotificationService**: Sends result notification
- **StudentManagementService**: Updates test status
- **BffPortalService**: Updates student statistics

---

#### BatchTestResultsEvent
Published when multiple tests are checked.

```csharp
public interface BatchTestResultsEvent
{
    List<TestResultData> Results { get; }
    DateTime CheckedAt { get; }
}
```

---

### 5. Notification Service Events

#### EmailSentEvent
Published after email is queued/sent.

```csharp
public interface EmailSentEvent
{
    Guid EmailId { get; }
    string To { get; }
    string Subject { get; }
    string TemplateName { get; }
    DateTime SentAt { get; }
    bool Success { get; }
    string? ErrorMessage { get; }
}
```

---

### 6. File Storage Service Events

#### FileUploadedEvent
Published after file is uploaded successfully.

```csharp
public interface FileUploadedEvent
{
    Guid FileId { get; }
    string FileName { get; }
    string ContentType { get; }
    long Size { get; }
    Guid OwnerId { get; }
    string FileType { get; }
    DateTime UploadedAt { get; }
}
```

---

## Event Naming Conventions

### Event Names (PascalCase, Past Tense)
```
TestCreatedEvent
StudentRegisteredEvent
TestSubmittedEvent
TestResultReceivedEvent
```

### Command Names (PascalCase, Verb + Entity)
```
CreateTestCommand
AssignTestToStudentCommand
CheckTestCommand
```

### Consumer Names (Verb + Entity + Consumer)
```
TestCreatedConsumer
StudentRegisteredConsumer
TestSubmittedConsumer
```

---

## MassTransit Configuration

### Identity Service Configuration

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<TestCreatedConsumer>()
        .Endpoint(e => e.Name = "identity-test-created");
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(settings.Host, settings.Port, settings.VirtualHost, h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});
```

### Student Management Service Configuration

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<TestAssignedConsumer>()
        .Endpoint(e => e.Name = "student-test-assigned");
    
    x.AddConsumer(TestStartedConsumer>()
        .Endpoint(e => e.Name = "student-test-started");
    
    cfg.ConfigureEndpoints(context);
});
```

### Notification Service Configuration

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<UserRegisteredConsumer>()
        .Endpoint(e => e.Name = "notification-user-registered");
    
    x.AddConsumer<TestCreatedConsumer>()
        .Endpoint(e => e.Name = "notification-test-created");
    
    x.AddConsumer<TestResultReceivedConsumer>()
        .Endpoint(e => e.Name = "notification-test-result");
    
    cfg.ConfigureEndpoints(context);
});
```

---

## RabbitMQ Topology

### Exchanges

| Exchange Name | Type | Description |
|---------------|------|-------------|
| `testsdelivery.events` | topic | Main exchange for all events |
| `testsdelivery.commands` | fanout | Command distribution |
| `testsdelivery.deadletter` | direct | Dead letter queue |

### Queues

| Queue Name | Consumer | Description |
|------------|----------|-------------|
| `identity-user-registered` | IdentityService | User registration events |
| `question-test-created` | QuestionService | Test creation events |
| `question-question-created` | QuestionService | Question creation events |
| `student-test-assigned` | StudentService | Test assignment events |
| `student-test-started` | StudentService | Test start events |
| `student-test-submitted` | StudentService | Test submission events |
| `test-checking-submitted` | TestCheckingService | Test submission events |
| `notification-user-registered` | NotificationService | Welcome emails |
| `notification-test-created` | NotificationService | Test availability emails |
| `notification-test-result` | NotificationService | Result notification emails |

---

## Event Flow Examples

### Flow 1: New Test Created → Email Notification

```
1. Teacher creates test
   └─> POST /api/v1/tests
       └─> TestService.CreateTest()

2. TestCreatedEvent published
   └─> massTransit.Publish(new TestCreatedEvent {...})

3. Multiple consumers receive event
   ├─> StudentManagementService.TestCreatedConsumer
   │   ├─> Queries students in group
   │   └─> Publishes TestAssignedEvent for each student
   
   ├─> NotificationService.TestCreatedConsumer
   │   ├─> Renders email template
   │   └─> Sends email via SMTP
   
   └─> BffPortalService (caching)
       └─> Invalidates available-tests cache
```

### Flow 2: Student Takes Test

```
1. Student clicks "Start Test"
   └─> POST /api/v1/portal/tests/{id}/start
       └─> BffPortalService.StartTest()

2. StudentManagementService creates assignment
   └─> POST /api/v1/students/{id}/tests/{id}/start
       └─> Publishes TestStartedEvent

3. Student submits test
   └─> POST /api/v1/students/{id}/tests/{id}/submit
       └─> Publishes TestSubmittedEvent

4. TestCheckingService processes submission
   └─> TestSubmittedConsumer receives event
       ├─> Scores answers
       └─> Publishes TestResultReceivedEvent

5. Results notify interested services
   ├─> NotificationService: Sends result email
   ├─> StudentManagementService: Updates status
   └─> BffPortalService: Updates statistics
```

---

## Error Handling & Retries

### Retry Policies

```csharp
cfg.UseEndpointConfiguration((context, cfg) =>
{
    cfg.ConfigureEndpoints(context, (context, endpointConfig) =>
    {
        endpointConfig.UseMessageRetry(retry => retry.Incremental(
            retryCount: 3,
            initialState: TimeSpan.FromSeconds(1),
            increment: TimeSpan.FromSeconds(2)));
        
        endpointConfig.UseInMemoryOutbox();
    });
});
```

### Dead Letter Queue

```csharp
cfg.ReceiveEndpoint("dead-letter", e =>
{
    e.UseMessageRetry(retry => retry.None());
    e.UseInMemoryOutbox();
    e.Consumer<DeadLetterConsumer>();
});
```

---

## Testing MassTransit Consumers

```csharp
public class TestCreatedConsumerTests
{
    [Fact]
    public async Task Consume_TestCreatedEvent_ShouldSendNotifications()
    {
        // Arrange
        var consumer = new TestCreatedConsumer(
            notificationServiceMock.Object,
            loggerMock.Object);
        
        var testCreatedEvent = new TestCreatedEvent
        {
            TestId = Guid.NewGuid(),
            Title = "Math Test",
            Description = "Test description",
            QuestionBankId = Guid.NewGuid(),
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };
        
        var context = new TestConsumeContext(testCreatedEvent);
        
        // Act
        await consumer.Consume(context);
        
        // Assert
        notificationServiceMock.Verify(
            ns => ns.SendTestCreatedNotificationAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
```

---

## Best Practices

1. **Event Immutability**: Events should be immutable and contain all necessary data
2. **Idempotency**: Consumers should handle duplicate events gracefully
3. **Versioning**: Include version in event messages for future compatibility
4. **Data Minimization**: Only include essential data in events
5. **Error Handling**: Always handle exceptions in consumers to prevent queue blocking
6. **Logging**: Log event processing with correlation IDs
