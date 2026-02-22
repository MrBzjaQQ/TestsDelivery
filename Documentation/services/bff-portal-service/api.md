# Bff Portal Service - API Reference

## Base URL

```
http://localhost:8080/api/v1
```

All requests require JWT token in Authorization header (except Auth endpoints).

## Auth API

> **Важно:** BffPortalService проксирует все запросы аутентификации в IdentityService (порт 8081). Angular UI работает только с BffPortalService как единой точкой входа.

### Register

Регистрация нового пользователя.

**Endpoint:** `POST /api/v1/auth/register`

**Request:**
```json
{
  "email": "student@university.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Student"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "User registered successfully",
  "data": {
    "userId": "user-guid-here",
    "email": "student@university.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student",
    "emailVerified": false,
    "createdAt": "2024-12-01T10:30:00Z"
  }
}
```

### Login

Аутентификация пользователя.

**Endpoint:** `POST /api/v1/auth/login`

**Request:**
```json
{
  "email": "student@university.com",
  "password": "SecurePassword123!"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Login successful",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "c29tZS1yZWZyZXNo...",
    "expiresIn": 3600,
    "user": {
      "id": "user-guid-here",
      "email": "student@university.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "Student",
      "emailVerified": true
    }
  }
}
```

### Refresh Token

Обновление access token.

**Endpoint:** `POST /api/v1/auth/refresh`

**Request:**
```json
{
  "refreshToken": "c29tZS1yZWZyZXNoLXRva2Vu..."
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Token refreshed successfully",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "bmV3LXJlZnJlc2gtdG9rZW4...",
    "expiresIn": 3600
  }
}
```

### Logout

Выход из системы. Инвалидирует refresh token.

**Endpoint:** `POST /api/v1/auth/logout`

**Headers:**
```
Authorization: Bearer <accessToken>
```

**Success Response (204):** No Content

### Forgot Password

Запрос на сброс пароля. Отправляет email с ссылкой для сброса.

**Endpoint:** `POST /api/v1/auth/forgot-password`

**Request:**
```json
{
  "email": "student@university.com"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Password reset email sent"
}
```

### Reset Password

Сброс пароля по токену из email.

**Endpoint:** `POST /api/v1/auth/reset-password`

**Request:**
```json
{
  "token": "reset-token-from-email",
  "newPassword": "NewSecurePassword123!"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Password reset successfully"
}
```

### Типы токенов

| Тип | Описание | Срок действия |
|-----|----------|---------------|
| Access Token | Используется для авторизации запросов | 60 минут |
| Refresh Token | Используется для обновления access token | 24 часа |

### Роли и права доступа

| Роль | Права |
|------|-------|
| Student | Доступ к прохождению тестов, просмотру результатов и профилю |
| Teacher | Все права Student + создание тестов, просмотр статистики групп |
| Admin | Полные права на управление системой |

## Portal API

### Get Student Profile

Gets student profile with test results.

**Endpoint:** `GET /api/v1/portal/students/profile`

**Headers:**
```
Authorization: Bearer <token>
```

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Student profile retrieved",
  "data": {
    "studentId": "student-id-here",
    "userId": "user-id-here",
    "firstName": "John",
    "lastName": "Doe",
    "email": "student@university.com",
    "group": {
      "id": "group-id",
      "name": "CS-2024",
      "startYear": 2024,
      "endYear": 2028
    },
    "profile": {
      "avatarUrl": "http://fileservice/avatar.jpg",
      "bio": "Computer Science student"
    },
    "statistics": {
      "totalTests": 10,
      "completedTests": 7,
      "inProgressTests": 2,
      "averageScore": 85.5
    },
    "recentTests": [
      {
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "status": "Completed",
        "score": 85,
        "maxScore": 100,
        "percentages": 85.0,
        "isPassed": true,
        "completedAt": "2024-12-01T10:30:00Z"
      },
      { /* another test */ }
    ]
  }
}
```

### Get Available Tests

Gets tests available for current user.

**Endpoint:** `GET /api/v1/portal/tests/available`

**Query Parameters:**
- `groupId` (string, optional): Filter by group
- `status` (string, optional): `available`, `expired`, `completed`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "5 tests found",
  "data": {
    "tests": [
      {
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "description": "Midterm exam for 10th grade mathematics",
        "durationMinutes": 60,
        "passingScore": 70,
        "maxAttempts": 3,
        "attemptsUsed": 1,
        "status": "Available",
        "availableFrom": "2024-12-01T00:00:00Z",
        "availableUntil": "2024-12-31T23:59:59Z",
        "questionsCount": 20
      },
      { /* another test */ }
    ],
    "totalCount": 5
  }
}
```

### Get Group Analytics

Gets analytics for a group.

**Endpoint:** `GET /api/v1/portal/groups/{id}/analytics`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Group analytics retrieved",
  "data": {
    "groupId": "group-id-here",
    "groupName": "CS-2024",
    "totalStudents": 30,
    "activeStudents": 28,
    "completedTests": 150,
    "averageScore": 78.5,
    "passRate": 83.3,
    "topPerformers": [
      {
        "studentId": "student-id-1",
        "studentName": "John Doe",
        "averageScore": 92.5
      },
      {
        "studentId": "student-id-2",
        "studentName": "Jane Smith",
        "averageScore": 90.0
      }
    ],
    "groupProgress": {
      "testsCompleted": 150,
      "testsInProgress": 25,
      "totalTestsAssigned": 175
    }
  }
}
```

### Start Test

Starts a test for student.

**Endpoint:** `POST /api/v1/portal/tests/{id}/start`

**Headers:**
```
Authorization: Bearer <token>
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test started",
  "data": {
    "testId": "test-id-here",
    "studentId": "student-id-here",
    "status": "InProgress",
    "startedAt": "2024-12-01T10:30:00Z",
    "deadline": "2024-12-01T11:30:00Z",
    "attemptsUsed": 1,
    "questionsCount": 20
  }
}
```

### Get Test Questions

Gets questions for a specific test. Used when student opens a test.

**Endpoint:** `GET /api/v1/portal/tests/{testId}/questions`

**Headers:**
```
Authorization: Bearer <token>
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test questions retrieved",
  "data": {
    "testId": "test-id-here",
    "testTitle": "Math Midterm",
    "testDescription": "Midterm exam for 10th grade mathematics",
    "durationMinutes": 60,
    "passingScore": 70,
    "questions": [
      {
        "questionId": "q1-guid",
        "text": "What is the capital of France?",
        "category": "Geography",
        "difficulty": "Easy",
        "order": 1,
        "options": [
          {
            "optionId": "opt1",
            "text": "London",
            "order": 1
          },
          {
            "optionId": "opt2",
            "text": "Paris",
            "order": 2
          },
          {
            "optionId": "opt3",
            "text": "Berlin",
            "order": 3
          }
        ],
        "imageFileId": null
      },
      {
        "questionId": "q2-guid",
        "text": "Which of these are in Europe? (Select all that apply)",
        "category": "Geography",
        "difficulty": "Medium",
        "order": 2,
        "options": [
          {
            "optionId": "opt4",
            "text": "France",
            "order": 1
          },
          {
            "optionId": "opt5",
            "text": "USA",
            "order": 2
          },
          {
            "optionId": "opt6",
            "text": "Germany",
            "order": 3
          }
        ],
        "imageFileId": null
      },
      {
        "questionId": "q3-guid",
        "text": "Explain the water cycle in your own words.",
        "category": "Science",
        "difficulty": "Hard",
        "order": 3,
        "answerType": "text",
        "maxLength": 1000
      }
    ],
    "totalQuestions": 3,
    "totalPoints": 100
  }
}
```

### Submit Test Answers

Submits answers for a test.

**Endpoint:** `POST /api/v1/portal/tests/{testId}/submit`

**Headers:**
```
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "answers": [
    {
      "questionId": "q1-guid",
      "selectedOptionIds": ["opt2"],
      "textAnswer": null
    },
    {
      "questionId": "q2-guid",
      "selectedOptionIds": ["opt4", "opt6"],
      "textAnswer": null
    },
    {
      "questionId": "q3-guid",
      "selectedOptionIds": [],
      "textAnswer": "The water cycle involves evaporation, condensation, precipitation, and collection."
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
    "testAssignmentId": "assignment-guid",
    "testId": "test-id-here",
    "studentId": "student-id-here",
    "status": "Submitted",
    "submittedAt": "2024-12-01T10:30:00Z",
    "attemptNumber": 1,
    "answersCount": 3
  }
}
```

### Get Test Results

Gets results for a test.

**Endpoint:** `GET /api/v1/portal/tests/{testId}/results`

**Headers:**
```
Authorization: Bearer <token>
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test results retrieved",
  "data": {
    "testId": "test-id-here",
    "testTitle": "Math Midterm",
    "score": 85,
    "maxScore": 100,
    "percentage": 85.0,
    "isPassed": true,
    "passedThreshold": 70,
    "attemptNumber": 1,
    "completedAt": "2024-12-01T10:35:00Z",
    "answers": [
      {
        "questionId": "q1-guid",
        "questionText": "What is the capital of France?",
        "selectedOptionIds": ["opt2"],
        "isCorrect": true,
        "pointsEarned": 10,
        "correctOptionIds": ["opt2"]
      },
      {
        "questionId": "q2-guid",
        "questionText": "Which of these are in Europe?",
        "selectedOptionIds": ["opt4", "opt6"],
        "isCorrect": true,
        "pointsEarned": 15,
        "correctOptionIds": ["opt4", "opt6"]
      },
      {
        "questionId": "q3-guid",
        "questionText": "Explain the water cycle...",
        "textAnswer": "The water cycle involves...",
        "isCorrect": true,
        "pointsEarned": 25,
        "feedback": "Good explanation, mentions all key stages"
      }
    ],
    "canRetake": true,
    "attemptsRemaining": 2
  }
}
```

### Get Available Tests

Gets tests available for current user.

**Endpoint:** `GET /api/v1/portal/tests/available`

**Query Parameters:**
- `groupId` (string, optional): Filter by group
- `status` (string, optional): `available`, `expired`, `completed`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "5 tests found",
  "data": {
    "tests": [
      {
        "testId": "test-id-1",
        "testTitle": "Math Midterm",
        "description": "Midterm exam for 10th grade mathematics",
        "durationMinutes": 60,
        "passingScore": 70,
        "maxAttempts": 3,
        "attemptsUsed": 1,
        "status": "Available",
        "availableFrom": "2024-12-01T00:00:00Z",
        "availableUntil": "2024-12-31T23:59:59Z",
        "questionsCount": 20,
        "canTake": true
      },
      {
        "testId": "test-id-2",
        "testTitle": "History Final",
        "description": "Final exam for world history",
        "durationMinutes": 90,
        "passingScore": 65,
        "maxAttempts": 2,
        "attemptsUsed": 0,
        "status": "Available",
        "availableFrom": "2024-12-15T00:00:00Z",
        "availableUntil": "2024-12-31T23:59:59Z",
        "questionsCount": 30,
        "canTake": true
      },
      {
        "testId": "test-id-3",
        "testTitle": "Completed Test",
        "description": "Already completed",
        "score": 85,
        "maxScore": 100,
        "percentage": 85.0,
        "isPassed": true,
        "completedAt": "2024-11-15T10:30:00Z",
        "status": "Completed",
        "canTake": false
      }
    ],
    "totalCount": 3
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `400` | Bad Request - Invalid request |
| `401` | Unauthorized - Missing/invalid token |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - Resource not found |
| `500` | Internal Server Error |

## Error Models

### PortalDataNotFoundException

```json
{
  "type": "PortalDataNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Student profile not found",
  "instance": "GET: /api/v1/portal/students/profile",
  "traceId": "00-abc123..."
}
```

### ServiceUnavailableException

```json
{
  "type": "ServiceUnavailableException",
  "title": "An error occurred while processing your request",
  "status": 503,
  "detail": "Student service is temporarily unavailable",
  "instance": "GET: /api/v1/portal/students/profile",
  "traceId": "00-abc123..."
}
```

## Caching Strategy

### Cache Keys

| Cache Key | Expiration | Content |
|-----------|-----------|---------|
| `student-profile:{studentId}` | 5 minutes | Student profile + stats |
| `available-tests:{userId}` | 10 minutes | Available tests list |
| `group-analytics:{groupId}` | 15 minutes | Group statistics |
| `test-questions:{testId}` | 1 hour | Test questions (rarely changed) |

### Cache Invalidation

```csharp
// On test submission
_cache.Remove($"student-profile:{studentId}");
_cache.Remove($"available-tests:{studentId}");
```

## Example: Portal Flow

### 1. Student Login

> Примечание: Аутентификация выполняется через IdentityService. Подробнее см. раздел "Аутентификация и авторизация".

### 2. Get Student Profile

```bash
GET /api/v1/portal/students/profile
Authorization: Bearer <access-token>

# Returns profile with tests + results
```

### 3. Get Available Tests

```bash
GET /api/v1/portal/tests/available
Authorization: Bearer <access-token>

# Returns tests available for student
```

### 4. Start Test

```bash
POST /api/v1/portal/tests/{testId}/start
Authorization: Bearer <access-token>

# Returns test details + questions
```

### 5. Get Test Questions

```bash
GET /api/v1/portal/tests/{testId}/questions
Authorization: Bearer <access-token>

# Returns questions with options
```

### 6. Submit Test Answers

```bash
POST /api/v1/portal/tests/{testId}/submit
Authorization: Bearer <access-token>
Content-Type: application/json

{
  "answers": [...]
}

# Returns submission confirmation
```

### 7. View Results

```bash
GET /api/v1/portal/tests/{testId}/results
Authorization: Bearer <access-token>

# Returns test results with detailed breakdown
```