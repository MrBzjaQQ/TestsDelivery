# Student Test Journey - TestsDelivery

## Complete Student Workflow

This document describes the complete journey a student takes when participating in tests through the TestsDelivery system.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                          STUDENT JOURNEY                                    │
└─────────────────────────────────────────────────────────────────────────────┘

1. EMAIL INVITATION
   ↓
2. PORTAL LOGIN
   ↓
3. VIEW AVAILABLE TESTS
   ↓
4. START TEST
   ↓
5. ANSWER QUESTIONS
   ↓
6. SUBMIT TEST
   ↓
7. PROCESSING
   ↓
8. VIEW RESULTS
   ↓
9. (OPTIONAL) RETAKE TEST
```

---

## 1. Email Invitation

### Trigger
Teacher creates a new test for a course/group and assigns it to students.

### Process Flow

```
Teacher Creates Test (Dashboard)
    ↓
POST /api/v1/tests (QuestionManagementService)
    ↓
TestCreatedEvent published to RabbitMQ
    ↓
StudentManagementService.TestCreatedConsumer
    ├─> Queries students in assigned groups
    └─> For each student:
        ├─> POST /api/v1/students/{id}/tests/{id}/assign
        └─> Publishes TestAssignedEvent
            ↓
NotificationService.TestAssignedConsumer
    ├─> Renders "test-created" template
    ├─> Sends email with test link
    └─> EmailSentEvent published
```

### Email Content

```
Subject: 📝 New Test Available: Mathematics Midterm

Hello John Doe,

A new test has been assigned to you!

Title: Mathematics Midterm
Course: 10th Grade Mathematics
Duration: 60 minutes
Passing Score: 70%
Max Attempts: 3

You can take the test until December 31, 2024, 11:59 PM.

🚀 Start Test Now: https://portal.testsdelivery.com/tests/123

If you have any questions, please contact your teacher.

Best regards,
TestsDelivery Team
```

---

## 2. Portal Login

### Student Action
Student clicks "Take Test" link in email and navigates to portal.

### Authentication Flow

```
Student clicks "Take Test" link
    ↓
GET /tests/{testId} (BffPortalService)
    ↓
Validates JWT token
    ├─> Valid → Continue to test
    └─> Invalid/Expired → Redirect to login
            ↓
        POST /api/v1/auth/login (IdentityService)
            ↓
        Returns access + refresh tokens
            ↓
        Redirect to test
```

### JWT Token Structure

```json
{
  "sub": "user-guid-here",
  "email": "student@university.com",
  "role": "Student",
  "firstName": "John",
  "lastName": "Doe",
  "iat": 1701426600,
  "exp": 1701427200
}
```

---

## 3. View Available Tests

### Student Dashboard

After login, student sees their dashboard with available tests.

### API Call

```
GET /api/v1/portal/tests/available (BffPortalService)
    ↓
Query StudentManagementService
    ├─> GET /api/v1/students/{id}/tests
    │   └─> Returns all test assignments
    │
    ├─> Filter by status:
    │   ├─> Available (not started, not expired)
    │   ├─> InProgress (started but not submitted)
    │   └─> Completed (submitted, results available)
    │
    └─> Filter by time:
        ├─> Upcoming (availableFrom in future)
        ├─> Available (now between availableFrom and availableUntil)
        └─> Expired (availableUntil passed)
```

### Dashboard Response

```json
{
  "isError": false,
  "message": "5 tests available",
  "data": {
    "tests": [
      {
        "testId": "test-1-guid",
        "testTitle": "Mathematics Midterm",
        "description": "Midterm exam for 10th grade mathematics",
        "durationMinutes": 60,
        "passingScore": 70,
        "maxAttempts": 3,
        "attemptsUsed": 0,
        "status": "Available",
        "availableFrom": "2024-12-01T00:00:00Z",
        "availableUntil": "2024-12-31T23:59:59Z",
        "questionsCount": 20,
        "canTake": true
      },
      {
        "testId": "test-2-guid",
        "testTitle": "History Final",
        "description": "Final exam for world history",
        "durationMinutes": 90,
        "passingScore": 65,
        "maxAttempts": 2,
        "attemptsUsed": 1,
        "status": "Completed",
        "score": 85,
        "maxScore": 100,
        "isPassed": true,
        "completedAt": "2024-11-15T10:30:00Z"
      }
    ],
    "totalCount": 5
  }
}
```

---

## 4. Start Test

### Student Action
Student clicks "Start Test" button on available test card.

### Process Flow

```
Student clicks "Start Test"
    ↓
POST /api/v1/portal/tests/{testId}/start (BffPortalService)
    ↓
POST /api/v1/students/{studentId}/tests/{testId}/start (StudentManagementService)
    ↓
Creates TestAssignment record:
    ├─> Status: InProgress
    ├─> StartedAt: Now
    ├─> Deadline: StartedAt + Duration
    ├─> AttemptNumber: Increment from previous attempts
    └─> Questions loaded from Test
    ↓
TestStartedEvent published to RabbitMQ
    ↓
Returns test data with questions:
{
  "testId": "guid",
  "title": "Mathematics Midterm",
  "description": "Midterm exam for 10th grade mathematics",
  "durationMinutes": 60,
  "passingScore": 70,
  "maxAttempts": 3,
  "attemptsUsed": 1,
  "startedAt": "2024-12-01T10:30:00Z",
  "deadline": "2024-12-01T11:30:00Z",
  "questions": [
    {
      "questionId": "q1-guid",
      "text": "What is the capital of France?",
      "category": "Geography",
      "difficulty": "Easy",
      "options": [
        {"optionId": "opt1", "text": "London", "order": 1},
        {"optionId": "opt2", "text": "Paris", "order": 2},
        {"optionId": "opt3", "text": "Berlin", "order": 3}
      ],
      "imageFileId": null
    },
    // More questions...
  ]
}
```

### Angular UI Timer

```typescript
// Angular component timer logic
@Component({
  selector: 'app-test-taking',
  template: `
    <div class="timer">
      <mat-icon>access_time</mat-icon>
      <span>{{ timeLeft | date: 'mm:ss' }}</span>
      of {{ test.durationMinutes }} min
    </div>
    
    <!-- Questions UI -->
    <app-question-card *ngFor="let q of test.questions" [question]="q">
    </app-question-card>
    
    <button mat-button (click)="submitTest()" [disabled]="timeLeft === 0">
      Submit Test
    </button>
  `
})
export class TestTakingComponent implements OnInit, OnDestroy {
  timeLeft: number;
  private countdownInterval: any;
  
  ngOnInit() {
    // Start countdown timer
    this.timeLeft = this.test.durationMinutes * 60;
    this.startCountdown();
  }
  
  startCountdown() {
    this.countdownInterval = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.autoSubmit();
      }
    }, 1000);
  }
  
  ngOnDestroy() {
    clearInterval(this.countdownInterval);
  }
}
```

---

## 5. Answer Questions

### Student Interaction

Student answers questions, can:
- Select options (radio buttons/multiple checkboxes)
- Type open-ended answers
- Mark questions for review
- Navigate between questions
- See progress bar

### Question Types Supported

#### Multiple Choice (Single Answer)
```json
{
  "questionId": "q1-guid",
  "text": "What is 2 + 2?",
  "options": [
    {"optionId": "opt1", "text": "3", "order": 1},
    {"optionId": "opt2", "text": "4", "order": 2},
    {"optionId": "opt3", "text": "5", "order": 3}
  ]
}
```

#### Multiple Choice (Multiple Answers)
```json
{
  "questionId": "q2-guid",
  "text": "Which are Europe countries? (Select all that apply)",
  "options": [
    {"optionId": "opt1", "text": "France", "isCorrect": true},
    {"optionId": "opt2", "text": "USA", "isCorrect": false},
    {"optionId": "opt3", "text": "Germany", "isCorrect": true},
    {"optionId": "opt4", "text": "Canada", "isCorrect": false}
  ]
}
```

#### Open-Ended Answer
```json
{
  "questionId": "q3-guid",
  "text": "Explain the water cycle in your own words.",
  "answerType": "text",
  "maxLength": 1000
}
```

#### Image-Based Question
```json
{
  "questionId": "q4-guid",
  "text": "Identify the landmark in the image below:",
  "imageFileId": "image-guid",
  "options": [
    {"optionId": "opt1", "text": "Eiffel Tower", "order": 1},
    {"optionId": "opt2", "text": "Big Ben", "order": 2}
  ]
}
```

### UI Components

```typescript
// Angular component for question display
@Component({
  selector: 'app-question-card',
  template: `
    <mat-card>
      <mat-card-title>
        {{ question.order }}. {{ question.text }}
      </mat-card-title>
      
      <mat-card-content>
        <!-- Show image if present -->
        <img *ngIf="question.imageFileId" 
             [src]="getImageUrl(question.imageFileId)"
             [alt]="'Question image'">
        
        <!-- Single choice -->
        <mat-radio-group *ngIf="!question.isMultipleChoice"
                         [(ngModel)]="selectedOptionId">
          <mat-radio-button *ngFor="let opt of question.options"
                            [value]="opt.optionId">
            {{ opt.text }}
          </mat-radio-button>
        </mat-radio-group>
        
        <!-- Multiple choice -->
        <mat-checkbox *ngFor="let opt of question.options"
                      [checked]="selectedOptions.includes(opt.optionId)"
                      (change)="toggleOption(opt.optionId, $event.checked)">
          {{ opt.text }}
        </mat-checkbox>
        
        <!-- Text answer -->
        <mat-form-field *ngIf="question.answerType === 'text'">
          <textarea matInput 
                    [(ngModel)]="textAnswer"
                    [maxlength]="question.maxLength">
          </textarea>
          <mat-hint>{{ textAnswer?.length || 0 }}/{{ question.maxLength }}</mat-hint>
        </mat-form-field>
      </mat-card-content>
      
      <mat-card-actions>
        <button mat-button color="primary" (click)="markForReview()">
          Mark for Review
        </button>
      </mat-card-actions>
    </mat-card>
  `
})
export class QuestionCardComponent {
  @Input() question!: TestQuestion;
  selectedOptionId?: string;
  selectedOptions: string[] = [];
  textAnswer?: string;
  
  toggleOption(optionId: string, checked: boolean) {
    if (checked) {
      this.selectedOptions.push(optionId);
    } else {
      this.selectedOptions = this.selectedOptions.filter(id => id !== optionId);
    }
  }
}
```

---

## 6. Submit Test

### Student Action
Student reviews answers and clicks "Submit Test".

### Validation

Before submitting, system validates:
1. All required questions answered
2. Answer format (select options, text length)
3. Time not expired
4. Max attempts not exceeded

### Process Flow

```
Student clicks "Submit Test"
    ↓
Validate answers (Angular client-side)
    ├─> Check all questions have answers
    ├─> Validate answer format
    └─> Check time not expired
    
If validation passes:
    ↓
POST /api/v1/students/{studentId}/tests/{testId}/submit (StudentManagementService)
    ↓
Creates Submission record:
    ├─> Answers: Array of {questionId, selectedOptionIds, textAnswer}
    ├─> SubmittedAt: Now
    ├─> Status: Submitted
    └─> Update TestAssignment:
        ├─> Status: Submitted
        └─> AttemptsUsed: Increment
    ↓
Publishes TestSubmittedEvent to RabbitMQ
    ↓
Returns submission confirmation:
{
  "isError": false,
  "message": "Test submitted successfully",
  "data": {
    "testAssignmentId": "guid",
    "testId": "test-guid",
    "studentId": "student-guid",
    "status": "Submitted",
    "submittedAt": "2024-12-01T11:25:00Z",
    "attemptNumber": 1,
    "answersCount": 20
  }
}
```

### Angular Submit Implementation

```typescript
submitTest() {
  // Final validation
  const unanswered = this.questions.filter(q => 
    !q.options?.some(opt => 
      q.isMultipleChoice 
        ? this.selectedOptions[q.questionId]?.includes(opt.optionId)
        : this.selectedOptions[q.questionId] === opt.optionId
    )
  );
  
  if (unanswered.length > 0) {
    this.showConfirmationDialog(
      'Unanswered Questions',
      `You have ${unanswered.length} unanswered question(s). Are you sure you want to submit?`,
      'Yes, Submit',
      'Continue Editing'
    ).subscribe(confirmed => {
      if (confirmed) this.finalizeSubmission();
    });
    return;
  }
  
  this.finalizeSubmission();
}

finalizeSubmission() {
  // Build submission data
  const answers = this.questions.map(q => {
    if (q.answerType === 'text') {
      return {
        questionId: q.questionId,
        selectedOptionIds: [],
        textAnswer: this.textAnswers[q.questionId]
      };
    } else {
      return {
        questionId: q.questionId,
        selectedOptionIds: this.selectedOptions[q.questionId] || [],
        textAnswer: null
      };
    }
  });
  
  // Submit to backend
  this.testService.submitTest(this.testId, this.studentId, answers)
    .subscribe({
      next: (response) => {
        this.snackBar.open('Test submitted successfully!', 'Close', {
          duration: 3000,
          panelClass: ['snackbar-success']
        });
        this.router.navigate(['/portal/results']);
      },
      error: (error) => {
        this.snackBar.open('Failed to submit test. Please try again.', 'Close', {
          duration: 5000,
          panelClass: ['snackbar-error']
        });
        console.error('Test submission error:', error);
      }
    });
}
```

---

## 7. Processing

### Background Processing

After submission, backend processes the test:

```
TestSubmittedEvent received by TestCheckingService
    ↓
Batch Questions & Answers
    ├─> Load test with correct answers
    ├─> Load submitted answers
    └─> Compare answers
    
Scoring Algorithm
    ├─> Correct answer: Full points
    ├─> Incorrect answer: 0 points
    └─> Partial credit: For partially correct answers
    
Calculate Final Score
    ├─> Sum of earned points
    ├─> Calculate percentage
    └─> Determine pass/fail based on threshold
    
Save Result
    ├─> Insert into TestResults table
    ├─> Update TestAssignment status
    └─> Publish TestResultReceivedEvent
    
Notifications
    ├─> Email to student with result
    ├─> Update dashboard statistics
    └─> Notify teacher (if configured)
```

### Scoring Example

```csharp
public class TestScoringService
{
    public TestScoringResult ScoreTest(
        Test test,
        List<AnswerSubmission> submittedAnswers)
    {
        int totalPoints = 0;
        int earnedPoints = 0;
        var answerResults = new List<AnswerResult>();
        
        foreach (var question in test.Questions)
        {
            totalPoints += question.Points;
            
            var submittedAnswer = submittedAnswers
                .First(a => a.QuestionId == question.Id);
            
            bool isCorrect = false;
            int pointsEarned = 0;
            
            if (question.AnswerType == AnswerType.MultipleChoiceSingle)
            {
                isCorrect = submittedAnswer.SelectedOptionId == 
                            question.CorrectOptionId;
                pointsEarned = isCorrect ? question.Points : 0;
            }
            else if (question.AnswerType == AnswerType.MultipleChoiceMultiple)
            {
                var correctOptionIds = question.Options
                    .Where(o => o.IsCorrect)
                    .Select(o => o.Id)
                    .ToHashSet();
                
                var submittedOptionIds = new HashSet<Guid>(
                    submittedAnswer.SelectedOptionIds);
                
                // Exact match required for multiple choice
                isCorrect = correctOptionIds.SetEquals(submittedOptionIds);
                pointsEarned = isCorrect ? question.Points : 0;
            }
            else if (question.AnswerType == AnswerType.Text)
            {
                // For text answers, use similarity or keyword matching
                var similarity = CalculateSimilarity(
                    submittedAnswer.TextAnswer,
                    question.CorrectAnswer,
                    question.MinKeywordMatch);
                
                isCorrect = similarity >= 0.7; // 70% threshold
                pointsEarned = isCorrect ? question.Points : 0;
            }
            
            earnedPoints += pointsEarned;
            
            answerResults.Add(new AnswerResult
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
}
```

---

## 8. View Results

### Student Dashboard Update

After result is ready, student sees:

```
POST /api/v1/students/{id}/tests/{testId}/results (StudentManagementService)
    ↓
GET /api/v1/portal/students/profile (BffPortalService)
    ↓
Returns updated statistics:
{
  "totalTests": 10,
  "completedTests": 7,
  "inProgressTests": 0,
  "averageScore": 85.5,
  "recentTests": [
    {
      "testId": "test-guid",
      "testTitle": "Mathematics Midterm",
      "status": "Completed",
      "score": 85,
      "maxScore": 100,
      "percentage": 85.0,
      "isPassed": true,
      "completedAt": "2024-12-01T11:35:00Z"
    }
  ]
}
```

### Detailed Results View

Student can view detailed results:

```
GET /api/v1/students/{studentId}/tests/{testId}/results
    ↓
Returns detailed breakdown:
{
  "testId": "test-guid",
  "testTitle": "Mathematics Midterm",
  "score": 85,
  "maxScore": 100,
  "percentage": 85.0,
  "isPassed": true,
  "passedThreshold": 70,
  "attemptNumber": 1,
  "completedAt": "2024-12-01T11:35:00Z",
  "answers": [
    {
      "questionId": "q1-guid",
      "questionText": "What is 2 + 2?",
      "selectedOptionIds": ["opt2"],
      "isCorrect": true,
      "pointsEarned": 5,
      "correctOptionIds": ["opt2"]
    },
    {
      "questionId": "q2-guid",
      "questionText": "Which are European countries?",
      "selectedOptionIds": ["opt1", "opt3"],
      "isCorrect": true,
      "pointsEarned": 10,
      "correctOptionIds": ["opt1", "opt3"]
    },
    {
      "questionId": "q3-guid",
      "questionText": "Explain the water cycle...",
      "textAnswer": "Water evaporates...",
      "isCorrect": false,
      "pointsEarned": 0,
      "feedback": "Missing key concepts: condensation, precipitation"
    }
  ]
}
```

### Angular Results Component

```typescript
@Component({
  selector: 'app-test-results',
  template: `
    <mat-card>
      <mat-card-title>
        <h2>{{ testTitle }}</h2>
      </mat-card-title>
      
      <mat-card-content>
        <!-- Score summary -->
        <div class="score-summary">
          <div class="score-circle">
            <span class="score-value">{{ percentage }}%</span>
            <span class="score-label">Score</span>
          </div>
          <div class="score-details">
            <p><strong>Result:</strong> 
               <span [ngClass]="{
                 'text-success': isPassed,
                 'text-danger': !isPassed
               }">
                 {{ isPassed ? 'Passed' : 'Failed' }}
               </span>
            </p>
            <p><strong>Points:</strong> {{ score }} / {{ maxScore }}</p>
            <p><strong>Passed Threshold:</strong> {{ passedThreshold }}%</p>
          </div>
        </div>
        
        <!-- Answer breakdown -->
        <div class="answers-breakdown">
          <h3>Your Answers</h3>
          
          <mat-expansion-panel *ngFor="let answer of answers">
            <mat-expansion-panel-header>
              <mat-panel-title>
                <span class="question-number">
                  {{ getAnswerStatusIcon(answer.isCorrect) }}
                  {{ answer.questionText }}
                </span>
              </mat-panel-title>
              <mat-panel-description>
                {{ answer.pointsEarned }} / {{ getQuestionPoints(answer.questionId) }} points
              </mat-panel-description>
            </mat-expansion-panel-header>
            
            <div class="answer-details">
              <div *ngIf="answer.isCorrect" class="correct-answer">
                <mat-icon>check_circle</mat-icon>
                <span>Your answer is correct!</span>
              </div>
              
              <div *ngIf="!answer.isCorrect" class="incorrect-answer">
                <mat-icon>error</mat-icon>
                <span>Your answer is incorrect.</span>
                <div *ngIf="answer.feedback" class="feedback">
                  <strong>Feedback:</strong> {{ answer.feedback }}
                </div>
              </div>
              
              <div *ngIf="answer.selectedOptionIds?.length" class="selected-options">
                <strong>Your Selection:</strong>
                <div *ngFor="let id of answer.selectedOptionIds">
                  {{ getOptionText(answer.questionId, id) }}
                </div>
              </div>
              
              <div *ngIf="answer.textAnswer" class="text-answer">
                <strong>Your Answer:</strong>
                <p>{{ answer.textAnswer }}</p>
              </div>
            </div>
          </mat-expansion-panel>
        </div>
      </mat-card-content>
      
      <mat-card-actions *ngIf="canRetake()">
        <button mat-button color="primary" (click)="retakeTest()">
         Retake Test</button>
        <button mat-button routerLink="/portal/dashboard">
          Back to Dashboard</button>
      </mat-card-actions>
    </mat-card>
  `
})
export class TestResultsComponent {
  @Input() testId!: string;
  @Input() studentId!: string;
  
  testTitle!: string;
  score!: number;
  maxScore!: number;
  percentage!: number;
  isPassed!: boolean;
  passedThreshold!: number;
  completedAt!: Date;
  answers: AnswerResult[] = [];
  canRetake: boolean = false;
  
  ngOnInit() {
    this.loadData();
  }
  
  loadData() {
    this.testService.getTestResults(this.studentId, this.testId)
      .subscribe({
        next: (response) => {
          this.testTitle = response.data.testTitle;
          this.score = response.data.score;
          this.maxScore = response.data.maxScore;
          this.percentage = response.data.percentage;
          this.isPassed = response.data.isPassed;
          this.passedThreshold = response.data.passedThreshold;
          this.completedAt = response.data.completedAt;
          this.answers = response.data.answers;
          this.canRetake = response.data.canRetake;
        }
      });
  }
  
  retakeTest() {
    this.testService.retakeTest(this.testId, this.studentId)
      .subscribe(response => {
        this.router.navigate(['/portal/test', this.testId]);
      });
  }
  
  getAnswerStatusIcon(isCorrect: boolean): string {
    return isCorrect ? 'check_circle' : 'error';
  }
  
  getOptionText(questionId: string, optionId: string): string {
    const question = this.questions.find(q => q.id === questionId);
    return question?.options.find(o => o.id === optionId)?.text || '';
  }
}
```

---

## 9. Optional: Retake Test

### Retake Conditions

Student can retake test if:
- Test allows multiple attempts
- Student hasn't exceeded max attempts
- Test is still within available period

### Process Flow

```
Student clicks "Retake Test"
    ↓
POST /api/v1/students/{id}/tests/{testId}/retake (StudentManagementService)
    ↓
Validates retake conditions:
    ├─> AttemptsUsed < MaxAttempts
    ├─> Deadline not passed
    └─> Test is active
    
If valid:
    ↓
Creates new TestAssignment:
    ├─> Status: InProgress
    ├─> StartedAt: Now
    ├─> Deadline: StartedAt + Duration
    ├─> AttemptNumber: AttemptNumber + 1
    └─> Questions: New random order (if randomized)
    ↓
Publishes TestStartedEvent
    ↓
Returns new test session with questions
```

### Angular Retake Logic

```typescript
canRetake(): boolean {
  // Check if test allows retake
  return (
    this.maxAttempts > this.attemptsUsed &&
    this.deadline > new Date() &&
    this.canRetakeFlag
  );
}

retakeTest() {
  this.dialog.open(RetakeConfirmationDialog, {
    width: '400px',
    data: {
      currentScore: this.score,
      maxScore: this.maxScore,
      attemptsUsed: this.attemptsUsed,
      maxAttempts: this.maxAttempts,
      currentPercentage: this.percentage
    }
  }).afterClosed().subscribe(confirmed => {
    if (confirmed) {
      this.testService.retakeTest(this.testId, this.studentId)
        .subscribe({
          next: (response) => {
            this.snackBar.open('Starting new test attempt...', 'Close', {
              duration: 2000
            });
            this.router.navigate(['/portal/test', this.testId]);
          },
          error: (error) => {
            this.snackBar.open('Cannot retake test at this time', 'Close', {
              duration: 3000
            });
          }
        });
    }
  });
}
```

---

## Timeline Example

```
Day 1, 9:00 AM  → Student receives email: "New test available"
Day 1, 9:15 AM  → Student logs into portal
Day 1, 9:20 AM  → Student starts test
Day 1, 10:15 AM → Student submits test (45 minutes used)
Day 1, 10:20 AM → Background scoring completes
Day 1, 10:25 AM → Results email received
Day 1, 10:30 AM → Student views results (85/100, passed)
Day 2, 14:00 PM → Student retakes test (2nd attempt)
Day 2, 14:45 PM → New result: 92/100 (improved score)
```

---

## Metrics & Statistics

### Student Progress Tracking

```csharp
public class StudentProgressService
{
    public StudentProgress GetStudentProgress(Guid studentId)
    {
        var assignments = _context.TestAssignments
            .Where(a => a.StudentId == studentId)
            .ToList();
        
        var completedTests = assignments
            .Where(a => a.Status == TestStatus.Completed)
            .ToList();
        
        var inProgressTests = assignments
            .Where(a => a.Status == TestStatus.InProgress)
            .ToList();
        
        var assignedTests = assignments
            .Where(a => a.Status == TestStatus.Assigned)
            .ToList();
        
        var averageScore = completedTests.Any()
            ? completedTests.Average(a => a.Score) ?? 0
            : 0;
        
        var passRate = completedTests.Any()
            ? (double)completedTests.Count(a => a.IsPassed) / completedTests.Count * 100
            : 0;
        
        return new StudentProgress
        {
            TotalTests = assignments.Count,
            CompletedTests = completedTests.Count,
            InProgressTests = inProgressTests.Count,
            AssignedTests = assignedTests.Count,
            AverageScore = Math.Round(averageScore, 1),
            PassRate = Math.Round(passRate, 1)
        };
    }
}
```

### Dashboard Statistics

```
Student Dashboard
├─ Total Tests Assigned: 10
├─ Completed Tests: 7
├─ In Progress: 1
├─ Available: 2
├─ Average Score: 85.5%
├─ Pass Rate: 85.7%
└─ Top Score: 95
```

---

## Best Practices for Students

1. **Read Instructions Carefully**: Check test rules, time limits, and question types
2. **Manage Time**: Keep an eye on the countdown timer
3. **Review Before Submitting**: Check all questions are answered
4. **Use Multiple Attempts**: If allowed, use retakes to improve scores
5. **Check Results Thoroughly**: Review incorrect answers for learning
6. **Contact Support**: Report any technical issues immediately
