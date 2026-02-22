# Student Management Service - API Reference

## Base URL

```
http://localhost:8083/api/v1
```

## Responses

All responses use `ResponseResultModel<T>` pattern.

## Students API

### Register Student

**Endpoint:** `POST /api/v1/students`

**Request Body:**
```json
{
  "userId": "user-id-here",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@student.com",
  "groupId": "group-id-here",
  "enrollmentDate": "2024-01-15T00:00:00Z"
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Student registered successfully",
  "data": {
    "id": "s1t2u3d4-e5n6s7t8-i9o1n2g3-s4t5u6d7e8",
    "userId": "user-id-here",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@student.com",
    "groupId": "group-id-here",
    "status": "Active",
    "enrollmentDate": "2024-01-15T00:00:00Z"
  }
}
```

### Get Student by ID

**Endpoint:** `GET /api/v1/students/{id}`

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Student retrieved successfully",
  "data": {
    "id": "s1t2u3d4-e5n6s7t8-i9o1n2g3-s4t5u6d7e8",
    "userId": "user-id-here",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@student.com",
    "groupId": "group-id-here",
    "phoneNumber": "+1234567890",
    "profile": {
      "avatarUrl": "http://fileservice/avatar.jpg",
      "bio": "Computer Science student"
    },
    "status": "Active",
    "enrollmentDate": "2024-01-15T00:00:00Z"
  }
}
```

### Update Student Profile

**Endpoint:** `PUT /api/v1/students/{id}`

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Smith",
  "phoneNumber": "+1234567890",
  "profile": {
    "bio": "Updated bio",
    "avatarUrl": "http://fileservice/new-avatar.jpg"
  }
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Student profile updated successfully",
  "data": { /* updated student */ }
}
```

### Get Students by Group

**Endpoint:** `GET /api/v1/students?groupId={groupId}&status={active}`

**Query Parameters:**
- `groupId` (string, required): Group ID
- `status` (string, optional): `active`, `inactive`, `graduated`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "5 students found",
  "data": {
    "students": [ /* array of students */ ],
    "totalCount": 5
  }
}
```

## Test Assignments API

### Assign Test to Student

**Endpoint:** `POST /api/v1/students/{studentId}/tests/{testId}/assign`

**Success Response (201):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Test assigned to student",
  "data": {
    "id": "a1s2s3s4-i5s6n7g8-a9t0e1s2-s3s4t5s6",
    "studentId": "student-id-here",
    "testId": "test-id-here",
    "assignedAt": "2024-12-01T10:30:00Z",
    "deadline": "2024-12-08T23:59:59Z",
    "status": "Assigned",
    "attemptsAllowed": 3,
    "attemptsUsed": 0
  }
}
```

### Get Student's Tests

**Endpoint:** `GET /api/v1/students/{id}/tests`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "3 tests retrieved",
  "data": {
    "assignments": [
      {
        "id": "a1s2s3s4-i5s6n7g8-a9t0e1s2-s3s4t5s6",
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "assignedAt": "2024-12-01T10:30:00Z",
        "deadline": "2024-12-08T23:59:59Z",
        "status": "InProgress",
        "attemptsUsed": 1,
        "attemptsAllowed": 3
      },
      { /* another test */ }
    ],
    "totalCount": 3
  }
}
```

### Get Active Tests for Student

**Endpoint:** `GET /api/v1/students/{id}/tests/active`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "1 active test",
  "data": {
    "assignments": [ /* active tests only */ ],
    "totalCount": 1
  }
}
```

## Test Progress & Results

### Get Student's Progress

**Endpoint:** `GET /api/v1/students/{id}/progress`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Progress report generated",
  "data": {
    "studentId": "student-id-here",
    "totalTests": 10,
    "completedTests": 7,
    "inProgressTests": 2,
    "assignedTests": 1,
    "averageScore": 85.5,
    "completedTests": [
      {
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "score": 85,
        "maxScore": 100,
        "isPassed": true,
        "completedAt": "2024-12-01T10:30:00Z"
      },
      { /* another completed test */ }
    ]
  }
}
```

### Get Test Results for Student

**Endpoint:** `GET /api/v1/students/{studentId}/tests/{testId}/results`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test results retrieved",
  "data": {
    "testId": "test-id-here",
    "testTitle": "Math Midterm",
    "attempts": [
      {
        "attemptNumber": 1,
        "score": 85,
        "maxScore": 100,
        "isPassed": true,
        "submittedAt": "2024-12-01T10:30:00Z",
        "answers": [
          {
            "questionId": "question-id-1",
            "selectedOptionId": "option-id-1",
            "isCorrect": true,
            "pointsEarned": 10
          },
          { /* another answer */ }
        ]
      }
    ]
  }
}
```

### Submit Test Answers

**Endpoint:** `POST /api/v1/students/{studentId}/tests/{testId}/submit`

**Request Body:**
```json
{
  "answers": [
    {
      "questionId": "question-id-1",
      "selectedOptionId": "option-id-1"
    },
    {
      "questionId": "question-id-2",
      "textAnswer": "Paris is the capital of France"
    }
  ]
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test submitted successfully",
  "data": {
    "attemptNumber": 1,
    "status": "Submitted",
    "submittedAt": "2024-12-01T10:30:00Z"
  }
}
```

## Groups API

### Get Group Statistics

**Endpoint:** `GET /api/v1/groups/{id}/statistics`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Group statistics retrieved",
  "data": {
    "groupId": "group-id-here",
    "groupName": "CS-2024",
    "totalStudents": 30,
    "activeStudents": 28,
    "completedTests": 150,
    "averageScore": 78.5,
    "topPerformers": [
      {
        "studentId": "student-id-1",
        "studentName": "John Doe",
        "averageScore": 92.5
      },
      { /* another student */ }
    ],
    "testsCompletedByStudents": [
      {
        "studentId": "student-id-1",
        "testCount": 12,
        "averageScore": 92.5
      },
      { /* another student */ }
    ]
  }
}
```

### Get Group Students

**Endpoint:** `GET /api/v1/groups/{id}/students`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "30 students in group",
  "data": {
    "students": [ /* array of students */ ],
    "totalCount": 30
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK |
| `201` | Created |
| `204` | No Content |
| `400` | Bad Request |
| `401` | Unauthorized |
| `403` | Forbidden |
| `404` | Not Found |
| `409` | Conflict |
| `422` | Unprocessable Entity |
| `500` | Internal Server Error |

## Error Models

### StudentNotFoundException

```json
{
  "type": "StudentNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Student with id '123' not found",
  "instance": "GET: /api/v1/students/123",
  "traceId": "00-abc123..."
}
```

### TestAssignmentNotFoundException

```json
{
  "type": "TestAssignmentNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Test assignment not found",
  "instance": "POST: /api/v1/students/123/tests/456/submit",
  "traceId": "00-abc123..."
}
```

### TestNotEligibleException

```json
{
  "type": "TestNotEligibleException",
  "title": "An error occurred while processing your request",
  "status": 422,
  "detail": "Test deadline has passed or maximum attempts reached",
  "instance": "POST: /api/v1/students/123/tests/456/submit",
  "traceId": "00-abc123...",
  "violations": [
    {
      "field": "testId",
      "message": "Test deadline: 2024-12-08 23:59:59",
      "code": "DeadlineExpired"
    }
  ]
}
```

## Example: Student Test Flow

### 1. Register Student
```bash
POST /api/v1/students
{
  "userId": "user-123",
  "firstName": "Alice",
  "lastName": "Johnson",
  "email": "alice@student.com",
  "groupId": "group-456"
}

# Response: 201 Created
```

### 2. Assign Test to Student
```bash
POST /api/v1/students/alice-id/tests/math-test-id/assign

# Response: 201 Created
{
  "testId": "math-test-id",
  "deadline": "2024-12-15T23:59:59Z",
  "attemptsAllowed": 3,
  "status": "Assigned"
}
```

### 3. Submit Test
```bash
POST /api/v1/students/alice-id/tests/math-test-id/submit
{
  "answers": [
    { "questionId": "q1", "selectedOptionId": "opt1" },
    { "questionId": "q2", "textAnswer": "Answer text" }
  ]
}

# Response: 200 OK
{
  "attemptNumber": 1,
  "status": "Submitted"
}
```

### 4. Get Results (after checking)
```bash
GET /api/v1/students/alice-id/tests/math-test-id/results

# Response: 200 OK
{
  "score": 85,
  "maxScore": 100,
  "isPassed": true,
  "attemptNumber": 1
}
```
