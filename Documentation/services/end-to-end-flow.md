# End-to-End Flow - TestsDelivery

## Complete System Workflow

This document describes the complete end-to-end flow of the TestsDelivery system, covering all scenarios from test creation to result delivery.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        END-TO-END FLOWS                                     │
└─────────────────────────────────────────────────────────────────────────────┘

1. TEACHER CREATE TEST FLOW
2. STUDENT TEST TAKING FLOW  
3. AUTOMATIC TEST CHECKING FLOW
4. RESULT NOTIFICATION FLOW
5. BATCH PROCESSING FLOW
```

---

## 1. Teacher Create Test Flow

### High-Level Flow

```
Teacher Dashboard
    ↓
Create Test Button
    ↓
Question Selection
    ├─> Select from Question Bank
    ├─> Add New Question
    └─> Import Question Template
    ↓
Test Configuration
    ├─> Title & Description
    ├─> Duration (minutes)
    ├─> Passing Score (%)
    ├─> Max Attempts
    ├─> Available Period
    └─> Assign to Groups/Students
    ↓
Test Created
    ↓
RabbitMQ Event Published
    ↓>
Multiple Services notified:
    ├─> StudentManagementService
    ├─> NotificationService  
    └─> BffPortalService
```

### Detailed step-by-step

#### Step 1: Teacher accesses test creation

```
Teacher clicks "Create Test"
    ↓
GET /api/v1/templates (GET all available templates)
    ↓
Returns templates list:
[
  {
    "id": "math-standard",
    "name": "Math Standard Exam",
    "description": "Standard math exam with 20 questions",
    "defaultDuration": 60,
    "defaultPassingScore": 70,
    "questionsCount": 20
  },
  {
    "id": "history-midterm",
    "name": "History Midterm",
    "description": "History midterm exam",
    "defaultDuration": 90,
    "defaultPassingScore": 65,
    "questionsCount": 30
  }
]
```

#### Step 2: Select questions

```
Teacher selects question bank:
    ↓
GET /api/v1/question-banks?ownerId={teacherId}
    ↓
Returns question banks:
[
  {
    "id": "math-bank-1",
    "name": "10th Grade Math Questions",
    "description": "All math questions for 10th grade",
    "questionsCount": 150,
    "categories": ["Algebra", "Geometry", "Trigonometry"]
  }
]
    ↓
Teacher selects questions from bank:
    ↓
POST /api/v1/tests
    ↓
Request:
{
  "title": "Math Midterm Exam",
  "description": "Midterm exam for 10th grade mathematics",
  "questionBankId": "math-bank-1",
  "durationMinutes": 60,
  "passingScore": 70,
  "maxAttempts": 3,
  "assignToGroups": ["group-10a", "group-10b"],
  "availableFrom": "2024-12-01T00:00:00Z",
  "availableUntil": "2024-12-31T23:59:59Z"
}
```

#### Step 3: Test Creation in QuestionManagementService

```csharp
// QuestionManagementService.TestService.CreateTest()
public async Task<Test> CreateTestAsync(CreateTestRequest request, Guid teacherId)
{
    // 1. Get question bank
    var bank = await _questionBankRepository.GetByIdAsync(
        request.QuestionBankId);
    
    // 2. Generate test questions
    var questions = await GenerateQuestionsAsync(
        bank.Id, 
        request.QuestionCount ?? bank.DefaultQuestionCount);
    
    // 3. Create test entity
    var test = new Test
    {
        Title = request.Title,
        Description = request.Description,
        QuestionBankId = request.QuestionBankId,
        DurationMinutes = request.DurationMinutes,
        PassingScore = request.PassingScore,
        MaxAttempts = request.MaxAttempts,
        AvailableFrom = request.AvailableFrom,
        AvailableUntil = request.AvailableUntil,
        CreatedBy = teacherId,
        Questions = questions,
        Status = TestStatus.Draft
    };
    
    // 4. Save to database
    await _testRepository.AddAsync(test);
    
    // 5. Publish event
    await _eventPublisher.Publish(new TestCreatedEvent
    {
        TestId = test.Id,
        Title = test.Title,
        Description = test.Description,
        QuestionBankId = test.QuestionBankId,
        DurationMinutes = test.DurationMinutes,
        PassingScore = test.PassingScore,
        MaxAttempts = test.MaxAttempts,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = teacherId
    });
    
    return test;
}
```

#### Step 4: Event Handling by Downstream Services

```csharp
// StudentManagementService.TestCreatedConsumer
public class TestCreatedConsumer : IConsumer<TestCreatedEvent>
{
    public async Task Consume(ConsumeContext<TestCreatedEvent> context)
    {
        var testCreatedEvent = context.Message;
        
        // Find all students in assigned groups
        var students = await _studentRepository.GetStudentsByGroupsAsync(
            testCreatedEvent.AssignToGroups);
        
        foreach (var student in students)
        {
            // Create test assignment for each student
            var assignment = new TestAssignment
            {
                TestId = testCreatedEvent.TestId,
                StudentId = student.Id,
                Status = TestStatus.Assigned,
                Deadline = testCreatedEvent.AvailableUntil,
                AttemptsAllowed = testCreatedEvent.MaxAttempts,
                AttemptsUsed = 0,
                AssignedAt = DateTime.UtcNow
            };
            
            await _testAssignmentRepository.AddAsync(assignment);
            
            // Publish TestAssignedEvent
            await _eventPublisher.Publish(new TestAssignedEvent
            {
                TestAssignmentId = assignment.Id,
                TestId = assignment.TestId,
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                StudentEmail = student.Email,
                TestTitle = testCreatedEvent.Title,
                AssignedAt = assignment.AssignedAt,
                Deadline = assignment.Deadline,
                AttemptsAllowed = assignment.AttemptsAllowed
            });
        }
    }
}

// NotificationService.TestCreatedConsumer
public class TestCreatedConsumer : IConsumer<TestAssignedEvent>
{
    public async Task Consume(ConsumeContext<TestAssignedEvent> context)
    {
        var testAssignedEvent = context.Message;
        
        // Render email template
        var emailBody = _emailTemplateRenderer.Render(
            "test-created",
            new
            {
                studentName = testAssignedEvent.StudentName,
                testTitle = testAssignedEvent.TestTitle,
                testLink = $"https://portal.com/tests/{testAssignedEvent.TestId}",
                deadline = testAssignedEvent.Deadline,
                duration = "60 minutes"
            });
        
        // Send email
        await _smtpClient.SendEmailAsync(
            to: testAssignedEvent.StudentEmail,
            subject: $"New Test Available: {testAssignedEvent.TestTitle}",
            body: emailBody);
        
        // Publish EmailSentEvent
        await _eventPublisher.Publish(new EmailSentEvent
        {
            EmailId = Guid.NewGuid(),
            To = testAssignedEvent.StudentEmail,
            Subject = $"New Test: {testAssignedEvent.TestTitle}",
            TemplateName = "test-created",
            SentAt = DateTime.UtcNow,
            Success = true
        });
    }
}
```

---

## 2. Student Test Taking Flow

### Flow Overview

```
Student receives email notification
    ↓
Student clicks "Take Test" link
    ↓
Student logs into portal (if not already)
    ↓
Student navigates to "Available Tests"
    ↓
Student clicks "Start Test"
    ↓
Test timer starts
    ↓
Student answers questions
    ↓
Student submits test
    ↓
Test checking process begins
    ↓
 Student receives results
```

### Detailed Flow with API Calls

#### Step 1: Student opens test from email

```
GET /tests/{testId} (BffPortalService)
    ↓
Validates JWT token
    ↓
POST /api/v1/auth/login (if needed)
    ↓
Returns access token
    ↓
GET /api/v1/portal/tests/{testId} (BffPortalService)
    ↓
GET /api/v1/students/{studentId}/tests/{testId} (StudentManagementService)
    ↓
Returns test details with questions:
{
  "testId": "test-guid",
  "title": "Math Midterm",
  "description": "Midterm exam for 10th grade mathematics",
  "durationMinutes": 60,
  "passingScore": 70,
  "maxAttempts": 3,
  "attemptsUsed": 0,
  "status": "Available",
  "questions": [
    {
      "questionId": "q1",
      "text": "What is the capital of France?",
      "category": "Geography",
      "difficulty": "Easy",
      "options": [
        {"optionId": "opt1", "text": "London"},
        {"optionId": "opt2", "text": "Paris"},
        {"optionId": "opt3", "text": "Berlin"}
      ]
    }
  ]
}
```

#### Step 2: Student starts test

```
POST /api/v1/students/{studentId}/tests/{testId}/start
    ↓
Creates TestAssignment:
    ├─> StudentId: {studentId}
    ├─> TestId: {testId}
    ├─> Status: InProgress
    ├─> StartedAt: Now
    ├─> Deadline: Now + 60 minutes
    ├─> AttemptNumber: 1
    └─> Questions: Loaded from test
    ↓
TestStartedEvent published
    ↓
Returns test session:
{
  "testAssignmentId": "assignment-guid",
  "status": "InProgress",
  "startedAt": "2024-12-01T10:30:00Z",
  "deadline": "2024-12-01T11:30:00Z",
  "attemptNumber": 1,
  "questionsCount": 20
}
```

#### Step 3: Student answers questions

```
Student navigates through questions, selecting answers
    ↓
Answers are saved locally (Angular state)
    ↓
Student can:
    ├─> Skip questions
    ├─> Mark for review
    └─> Change answers before submit
```

#### Step 4: Student submits test

```
POST /api/v1/students/{studentId}/tests/{testId}/submit
    ↓
Request:
{
  "answers": [
    {
      "questionId": "q1",
      "selectedOptionIds": ["opt2"],
      "textAnswer": null
    },
    {
      "questionId": "q2",
      "selectedOptionIds": ["opt1", "opt3"],
      "textAnswer": null
    },
    {
      "questionId": "q3",
      "selectedOptionIds": [],
      "textAnswer": "The capital of France is Paris"
    }
  ]
}
    ↓
Validates:
    ├─> All questions answered
    ├─> Answer format correct
    └─> Time not expired
    ↓
Updates TestAssignment:
    ├─> Status: Submitted
    ├─> SubmittedAt: Now
    └─> AttemptsUsed: 1
    ↓
Publishes TestSubmittedEvent
    ↓
Returns submission confirmation:
{
  "isError": false,
  "message": "Test submitted successfully",
  "data": {
    "testAssignmentId": "assignment-guid",
    "testId": "test-guid",
    "status": "Submitted",
    "submittedAt": "2024-12-01T11:25:00Z",
    "attemptNumber": 1,
    "answersCount": 20
  }
}
```

---

## 3. Automatic Test Checking Flow

### Flow Overview

```
TestSubmittedEvent received
    ↓
Load test with correct answers
    ↓
Compare submitted answers with correct answers
    ↓
Calculate score for each question
    ↓
Compute total score and percentage
    ↓
Determine pass/fail status
    ↓
Save test result to database
    ↓
Publish TestResultReceivedEvent
    ↓>
Multiple services notified:
    ├─> NotificationService (send email)
    ├─> StudentManagementService (update status)
    └─> BffPortalService (update statistics)
```

### Detailed Implementation

```csharp
// TestCheckingService TestSubmittedConsumer
public class TestSubmittedConsumer : IConsumer<TestSubmittedEvent>
{
    public async Task Consume(ConsumeContext<TestSubmittedEvent> context)
    {
        var testSubmittedEvent = context.Message;
        
        // Load test with questions and correct answers
        var test = await _testRepository.GetByIdAsync(testSubmittedEvent.TestId);
        
        // Score the test
        var scoringResult = await ScoreTestAsync(
            test,
            testSubmittedEvent.Answers);
        
        // Create test result
        var testResult = new TestResult
        {
            TestAssignmentId = testSubmittedEvent.TestAssignmentId,
            TestId = testSubmittedEvent.TestId,
            StudentId = testSubmittedEvent.StudentId,
            Score = scoringResult.EarnedPoints,
            MaxScore = scoringResult.TotalPoints,
            Percentage = scoringResult.Percentage,
            IsPassed = scoringResult.IsPassed,
            PassedThreshold = test.PassingScore,
            AttemptNumber = testSubmittedEvent.AttemptNumber,
            CompletedAt = DateTime.UtcNow,
            AnswerResults = scoringResult.AnswerResults.Select(ar => new AnswerResult
            {
                QuestionId = ar.QuestionId,
                SelectedOptionIds = ar.SelectedOptionIds,
                TextAnswer = ar.TextAnswer,
                IsCorrect = ar.IsCorrect,
                PointsEarned = ar.PointsEarned
            }).ToList()
        };
        
        // Save to database
        await _testResultRepository.AddAsync(testResult);
        
        // Update test assignment status
        await _testAssignmentRepository.UpdateAsync(
            testSubmittedEvent.TestAssignmentId,
            ta => ta.Status = TestStatus.Completed);
        
        // Publish result event
        await _eventPublisher.Publish(new TestResultReceivedEvent
        {
            TestId = testSubmittedEvent.TestId,
            StudentId = testSubmittedEvent.StudentId,
            StudentName = testSubmittedEvent.StudentName,
            StudentEmail = testSubmittedEvent.StudentEmail,
            Score = testResult.Score,
            MaxScore = testResult.MaxScore,
            Percentage = testResult.Percentage,
            IsPassed = testResult.IsPassed,
            PassedThreshold = testResult.PassedThreshold,
            PassedDate = testResult.CompletedAt,
            AttemptNumber = testResult.AttemptNumber
        });
    }
    
    private async Task<TestScoringResult> ScoreTestAsync(Test test, 
        List<AnswerSubmission> submittedAnswers)
    {
        int totalPoints = test.Questions.Sum(q => q.Points);
        int earnedPoints = 0;
        var answerResults = new List<ScoringAnswerResult>();
        
        foreach (var question in test.Questions)
        {
            var submittedAnswer = submittedAnswers
                .First(a => a.QuestionId == question.Id);
            
            bool isCorrect = false;
            int pointsEarned = 0;
            
            // Score based on question type
            switch (question.AnswerType)
            {
                case AnswerType.MultipleChoiceSingle:
                    isCorrect = submittedAnswer.SelectedOptionIds.Count == 1 &&
                                submittedAnswer.SelectedOptionIds[0] == question.CorrectOptionId;
                    pointsEarned = isCorrect ? question.Points : 0;
                    break;
                
                case AnswerType.MultipleChoiceMultiple:
                    var correctOptionIds = new HashSet<Guid>(
                        question.Options.Where(o => o.IsCorrect).Select(o => o.Id));
                    var submittedOptionIds = new HashSet<Guid>(
                        submittedAnswer.SelectedOptionIds);
                    isCorrect = correctOptionIds.SetEquals(submittedOptionIds);
                    pointsEarned = isCorrect ? question.Points : 0;
                    break;
                
                case AnswerType.Text:
                    var similarity = CalculateTextSimilarity(
                        submittedAnswer.TextAnswer, 
                        question.CorrectAnswer);
                    isCorrect = similarity >= 0.7; // 70% threshold
                    pointsEarned = isCorrect ? question.Points : 0;
                    break;
            }
            
            earnedPoints += pointsEarned;
            
            answerResults.Add(new ScoringAnswerResult
            {
                QuestionId = question.Id,
                IsCorrect = isCorrect,
                PointsEarned = pointsEarned,
                SelectedOptionIds = submittedAnswer.SelectedOptionIds,
                TextAnswer = submittedAnswer.TextAnswer
            });
        }
        
        var percentage = (double)earnedPoints / totalPoints * 100;
        var isPassed = percentage >= test.PassingScore;
        
        return new TestScoringResult
        {
            TestId = test.Id,
            TotalPoints = totalPoints,
            EarnedPoints = earnedPoints,
            Percentage = percentage,
            IsPassed = isPassed,
            PassedThreshold = test.PassingScore,
            AnswerResults = answerResults
        };
    }
    
    private double CalculateTextSimilarity(string submitted, string correct)
    {
        // Use Levenshtein distance or keyword matching
        var submittedWords = submitted.ToLower().Split(' ').ToHashSet();
        var correctWords = correct.ToLower().Split(' ').ToHashSet();
        
        var commonWords = submittedWords.Intersect(correctWords).Count();
        var totalWords = submittedWords.Union(correctWords).Count();
        
        return totalWords > 0 
            ? (double)commonWords / totalWords 
            : 0.0;
    }
}
```

---

## 4. Result Notification Flow

### Flow Overview

```
TestResultReceivedEvent published
    ↓
NotificationService receives event
    ↓
Email template rendering
    ↓
Email sent via SMTP
    ↓
Result stored in notification log
```

### Email Notification Implementation

```csharp
// NotificationService TestResultReceivedConsumer
public class TestResultReceivedConsumer : IConsumer<TestResultReceivedEvent>
{
    public async Task Consume(ConsumeContext<TestResultReceivedEvent> context)
    {
        var testResultEvent = context.Message;
        
        // Determine if passed or failed
        var status = testResultEvent.IsPassed ? "passed" : "failed";
        var statusColor = testResultEvent.IsPassed ? "green" : "red";
        
        // Render email template
        var emailBody = _emailTemplateRenderer.Render("test-result", new
        {
            studentName = testResultEvent.StudentName,
            testTitle = await GetTestTitleAsync(testResultEvent.TestId),
            score = testResultEvent.Score,
            maxScore = testResultEvent.MaxScore,
            percentage = Math.Round(testResultEvent.Percentage, 1),
            status = status,
            statusColor = statusColor,
            passedDate = testResultEvent.PassedDate
        });
        
        // Send email
        await _smtpClient.SendEmailAsync(
            to: testResultEvent.StudentEmail,
            subject: $"Test Result: {testResultEvent.TestTitle} - {status.ToUpper()}",
            body: emailBody);
        
        // Log email sent
        await _emailLogRepository.AddAsync(new EmailLog
        {
            EmailId = Guid.NewGuid(),
            To = testResultEvent.StudentEmail,
            Subject = $"Test Result: {testResultEvent.TestTitle}",
            TemplateName = "test-result",
            Status = EmailStatus.Sent,
            SentAt = DateTime.UtcNow,
            EventId = testResultEvent.TestId.ToString()
        });
    }
}

// Email template (test-result.hbs)
/*
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Test Result</title>
</head>
<body>
    <h1>Test Result Available</h1>
    <p>Dear {{studentName}},</p>
    <p>Your test result is ready:</p>
    <div style="background-color: #f8f9fa; padding: 20px; border-radius: 5px;">
        <h2>{{testTitle}}</h2>
        <p>Score: <strong>{{score}} / {{maxScore}}</strong> ({{percentage}}%)</p>
        <p>Status: <strong style="color: {{statusColor}};">{{status}}</strong></p>
    </div>
    <p>Best regards,<br>TestsDelivery Team</p>
</body>
</html>
*/
```

---

## 5. Batch Processing Flow

### Flow Overview

```
Teacher assigns test to entire class
    ↓
System detects multiple students
    ↓
Create test assignments in batch
    ↓
Publish TestAssignedEvent for each student
    ↓>
Concurrent processing:
    ├─> NotificationService (email batch)
    ├─> BffPortalService (cache updates)
    └─> StudentManagementService (statistics)
    ↓
All students receive notifications
```

### Batch Processing Implementation

```csharp
// StudentManagementService - Batch Test Assignment
public class BatchTestAssignmentService
{
    public async Task BatchAssignTestsAsync(
        Guid testId,
        List<Guid> studentIds,
        DateTime deadline,
        int maxAttempts)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        
        // Create assignments in batch
        var assignments = studentIds.Select(studentId => new TestAssignment
        {
            TestId = testId,
            StudentId = studentId,
            Status = TestStatus.Assigned,
            Deadline = deadline,
            AttemptsAllowed = maxAttempts,
            AttemptsUsed = 0,
            AssignedAt = DateTime.UtcNow
        }).ToList();
        
        await unitOfWork.TestAssignmentRepository.AddBatchAsync(assignments);
        await unitOfWork.SaveChangesAsync();
        
        // Publish events asynchronously
        foreach (var assignment in assignments)
        {
            // Get student details
            var student = await unitOfWork.StudentRepository.GetByIdAsync(
                assignment.StudentId);
            
            var test = await unitOfWork.TestRepository.GetByIdAsync(testId);
            
            var @event = new TestAssignedEvent
            {
                TestAssignmentId = assignment.Id,
                TestId = assignment.TestId,
                StudentId = assignment.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                StudentEmail = student.Email,
                TestTitle = test.Title,
                AssignedAt = assignment.AssignedAt,
                Deadline = assignment.Deadline,
                AttemptsAllowed = assignment.AttemptsAllowed
            };
            
            // Publish event
            await _eventPublisher.Publish(@event);
        }
    }
}
```

---

## Complete End-to-End Example: Full Test Lifecycle

```
[Day 1 - 9:00 AM] Teacher creates Math Midterm test
    ├─> Creates test with 20 questions from question bank
    ├─> Sets duration: 60 minutes, passing score: 70%, max attempts: 3
    ├─> Assigns to class groups
    └─> Publishes TestCreatedEvent

[Day 1 - 9:05 AM] 30 students receive email notifications
    ├─> StudentManagementService processes TestCreatedEvent
    ├─> Creates test assignments for all 30 students
    ├─> Publishes 30 TestAssignedEvents
    └─> NotificationService sends emails to all students

[Day 1 - 9:15 AM] Student John Doe logs into portal
    ├─> Sees "Math Midterm" in available tests
    ├─> Clicks "Start Test"
    └─> Timer starts (60 minutes)

[Day 1 - 10:20 AM] John submits test (55 minutes used)
    ├─> Answers 20 questions
    ├─> Clicks "Submit Test"
    └─> Publishes TestSubmittedEvent

[Day 1 - 10:20 AM] TestCheckingService automatically scores test
    ├─> Receives TestSubmittedEvent
    ├─> Loads correct answers
    ├─> Compares submitted answers
    ├─> Calculates score: 85/100 (85%)
    ├─> Determines: PASSED (threshold: 70%)
    └─> Saves result to database

[Day 1 - 10:21 AM] TestResultReceivedEvent published
    ├─> NotificationService receives event
    ├─> Renders "test-result" email template
    ├─> Sends email to john.doe@student.com
    └─> Notification logged

[Day 1 - 10:25 AM] John views results in portal
    ├─> Sees: "Math Midterm - PASSED - 85%"
    ├─> View detailed breakdown:
    │   ├─> Correct answers: 17
    │   └─> Incorrect answers: 3
    └─> Check "Retake Test" available (2 attempts remaining)

[Day 2 - 3:00 PM] John retakes test (2nd attempt)
    ├─> Starts new test session
    └─> Submits with score: 92/100 (92%)

[Day 2 - 3:01 PM] New result calculated and notifications sent
    └─> John's average score updated: 88.5%

[Day 3 - 9:00 AM] Teacher views class statistics
    ├─> Sees 30 students assigned, 30 completed
    ├─> Average class score: 85.5%
    └─> Pass rate: 100%
```

---

## Performance Metrics

### End-to-End Timing

| Step | Average Time | Max Time | Description |
|------|-------------|----------|-------------|
| Email notification | 5-10 seconds | 30 seconds | SMTP delivery |
| Test assignment creation | 2-5 seconds | 10 seconds | Database operations |
| Test submission | 3-7 seconds | 15 seconds | Validation + database |
| Automatic scoring | 5-15 seconds | 30 seconds | Answer comparison |
| Result notification | 5-10 seconds | 30 seconds | Email + cache update |
| **Total E2E time** | **20-57 seconds** | **115 seconds** | From submit to result email |

### Scalability

- **Concurrent test takers**: 10,000+ (with proper scaling)
- **Question bank size**: 100,000+ questions
- **Test assignments per hour**: 50,000+
- **Email delivery rate**: 1,000 emails/minute

### Error Rates

| Error Type | Rate | Recovery |
|------------|------|----------|
| Database timeout | <0.1% | Retry with exponential backoff |
| Email delivery failure | 0.5% | Queue for retry (max 3) |
| Test submission failure | <0.01% | Restore session from autosave |
| Event bus timeout | <0.05% | Dead letter queue + alert |

---

## Monitoring & Observability

### Key Metrics

```promql
# Request rates
http_requests_total{service="student-management"}
http_requests_total{service="test-checking"}

# Error rates
http_request_errors_total{service="student-management"} / 
http_requests_total{service="student-management"}

# Event bus metrics
masstransit_consumed_events_total{event_type="TestSubmittedEvent"}

# Processing times
histogram_quantile(0.95, rate(test_scoring_duration_seconds_bucket[5m]))

# Email delivery success rate
smtp_sent_emails_total / smtp_attempted_emails_total
```

### Logging Correlation

```
Correlation ID: abc123-def456-ghi789

[10:20:00] TestSubmittedEvent published
    └─> CorrelationId: abc123-def456

[10:20:01] TestCheckingService received event
    └─> CorrelationId: abc123-def456
    └─> CorrelationId: abc123-def456
    └─> Test scored: 85/100

[10:20:02] TestResultReceivedEvent published
    └─> CorrelationId: abc123-def456

[10:20:05] Email sent to john.doe@student.com
    └─> CorrelationId: abc123-def456
```

---

## Best Practices

1. **Event Ordering**: Always publish events in dependency order
2. **Idempotency**: Consumers should handle duplicate events
3. **Batch Processing**: Use batch operations for scalability
4. **Dead Letter Queues**: Handle failed events properly
5. **Retry Policies**: Implement exponential backoff
6. **Caching**: Cache frequent reads (tests, questions)
7. **Async Processing**: Use async/await throughout stack
8. **Monitoring**: Track key metrics and set alerts
9. **Testing**: Test complete flows in integration tests
10. **Documentation**: Document all event contracts
