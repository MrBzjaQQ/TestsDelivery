# Test Checking Service - API Reference

## Base URL

```
http://localhost:8084/api/v1
```

All requests require JWT token in Authorization header.

## Responses

All API responses follow the `ResponseResultModel<T>` pattern.

## Test Checking API

### Check Test

Checks a test and calculates score.

**Endpoint:** `POST /api/v1/tests/{testId}/check`

**Request Body:**
```json
{
  "studentId": "student-id-here",
  "answers": [
    {
      "questionId": "question-id-1",
      "selectedOptionId": "option-id-1",
      "textAnswer": "Answer text if open question"
    },
    {
      "questionId": "question-id-2",
      "selectedOptionId": "option-id-2",
      "textAnswer": null
    }
  ]
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Test checked successfully",
  "data": {
    "testId": "test-id-here",
    "studentId": "student-id-here",
    "score": 85,
    "maxScore": 100,
    "percentage": 85.0,
    "isPassed": true,
    "passedThreshold": 70,
    "passedDate": "2024-12-01T10:30:00Z",
    "attemptNumber": 1,
    "answerResults": [
      {
        "questionId": "question-id-1",
        "selectedOptionId": "option-id-1",
        "isCorrect": true,
        "pointsEarned": 10,
        "correctOptionId": "option-id-1"
      },
      {
        "questionId": "question-id-2",
        "selectedOptionId": "option-id-2",
        "isCorrect": false,
        "pointsEarned": 0,
        "correctOptionId": "option-id-3"
      }
    ]
  }
}
```

**Error Responses:**
- `400` - Invalid request data
- `404` - Test or student not found
- `422` - Test not eligible for checking

### Batch Check Tests

Checks multiple tests in one request.

**Endpoint:** `POST /api/v1/tests/check/batch`

**Request Body:**
```json
{
  "checks": [
    {
      "testId": "test-id-1",
      "studentId": "student-id-1",
      "answers": [ /* answers */ ]
    },
    {
      "testId": "test-id-2",
      "studentId": "student-id-2",
      "answers": [ /* answers */ ]
    }
  ]
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Batch check completed",
  "data": {
    "totalChecks": 2,
    "successfulChecks": 2,
    "failedChecks": 0,
    "results": [
      { /* result 1 */ },
      { /* result 2 */ }
    ]
  }
}
```

### Get Test Results

Gets results for a specific test.

**Endpoint:** `GET /api/v1/tests/{testId}/results`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Results retrieved",
  "data": {
    "testId": "test-id-here",
    "testTitle": "Math Midterm",
    "totalResults": 30,
    "passedResults": 25,
    "failedResults": 5,
    "averageScore": 78.5,
    "results": [
      {
        "studentId": "student-id-1",
        "studentName": "John Doe",
        "score": 85,
        "maxScore": 100,
        "percentage": 85.0,
        "isPassed": true,
        "attemptNumber": 1,
        "completedAt": "2024-12-01T10:30:00Z"
      },
      { /* another result */ }
    ]
  }
}
```

### Get Student Results

Gets all results for a student.

**Endpoint:** `GET /api/v1/students/{studentId}/results`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Student results retrieved",
  "data": {
    "studentId": "student-id-here",
    "studentName": "John Doe",
    "totalTests": 10,
    "passedTests": 8,
    "failedTests": 2,
    "averageScore": 82.5,
    "results": [
      {
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "score": 85,
        "maxScore": 100,
        "percentage": 85.0,
        "isPassed": true,
        "attemptNumber": 1,
        "completedAt": "2024-12-01T10:30:00Z"
      },
      { /* another result */ }
    ]
  }
}
```

### Get Single Result

Gets a specific test result for a student.

**Endpoint:** `GET /api/v1/tests/{testId}/results/{studentId}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Result retrieved",
  "data": {
    "testId": "test-id-here",
    "studentId": "student-id-here",
    "testTitle": "Math Midterm",
    "score": 85,
    "maxScore": 100,
    "percentage": 85.0,
    "isPassed": true,
    "attemptNumber": 1,
    "passedDate": "2024-12-01T10:30:00Z",
    "answers": [
      {
        "questionId": "question-id-1",
        "questionText": "What is 2+2?",
        "selectedOptionId": "option-id-1",
        "isSelectedCorrect": true,
        "pointsEarned": 10
      },
      { /* another answer */ }
    ]
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `201` | Created - Resource created |
| `400` | Bad Request - Invalid data |
| `401` | Unauthorized - Missing/invalid token |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - Resource not found |
| `422` | Unprocessable Entity - Test not eligible |
| `500` | Internal Server Error |

## Error Models

### TestResultNotFoundException

```json
{
  "type": "TestResultNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Test result for test '123' and student '456' not found",
  "instance": "GET: /api/v1/tests/123/results/456",
  "traceId": "00-abc123..."
}
```

### TestNotEligibleException

```json
{
  "type": "TestNotEligibleException",
  "title": "An error occurred while processing your request",
  "status": 422,
  "detail": "Test has expired or maximum attempts reached",
  "instance": "POST: /api/v1/tests/123/check",
  "traceId": "00-abc123...",
  "violations": [
    {
      "field": "testId",
      "message": "Deadline: 2024-12-08 23:59:59",
      "code": "DeadlineExpired"
    }
  ]
}
```

## MassTransit Events

### TestSubmittedEvent

Publishes when a student submits a test.

**Properties:**
```json
{
  "testId": "guid",
  "studentId": "guid",
  "studentName": "string",
  "answers": [
    {
      "questionId": "guid",
      "selectedOptionId": "guid",
      "textAnswer": "string"
    }
  ],
  "submittedAt": "datetime"
}
```

### TestResultReceivedEvent

Publishes when test result is calculated.

**Properties:**
```json
{
  "testId": "guid",
  "studentId": "guid",
  "studentName": "string",
  "score": "integer",
  "maxScore": "integer",
  "percentage": "double",
  "isPassed": "boolean",
  "passedThreshold": "integer",
  "passedDate": "datetime"
}
```

## Example: Test Checking Flow

### 1. Student Submit Test
```bash
POST /api/v1/students/123/tests/456/submit
{
  "answers": [
    { "questionId": "q1", "selectedOptionId": "opt1" },
    { "questionId": "q2", "textAnswer": "Answer text" }
  ]
}
```

### 2. TestSubmitted EventPublished
```json
{
  "testId": "456",
  "studentId": "123",
  "answers": [ /* answers */ ]
}
```

### 3. TestCheckingService Processes
```csharp
// Consumer receives event
// Scores test
// Publishes TestResultReceivedEvent
```

### 4. Result Published
```json
{
  "testId": "456",
  "studentId": "123",
  "score": 85,
  "maxScore": 100,
  "isPassed": true
}
```