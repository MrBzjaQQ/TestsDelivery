# Question Management Service - API Reference

## Base URL

```
http://localhost:8082/api/v1
```

All requests require JWT token in Authorization header:
```
Authorization: Bearer <token>
```

## Responses

All API responses follow the `ResponseResultModel<T>` pattern:

```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Question created successfully",
  "data": { /* response data */ }
}
```

Error responses return ProblemDetails (RFC 9457):

```json
{
  "type": "QuestionNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Question with id '123' not found",
  "instance": "GET: /api/v1/questions/123",
  "traceId": "00-abc123..."
}
```

## Questions API

### Create Question

Creates a new question.

**Endpoint:** `POST /api/v1/questions`

**Request Body:**
```json
{
  "text": "What is the capital of France?",
  "category": "Geography",
  "difficulty": "Easy",
  "questionBankId": "b8a7c6d5-e4f3-2a1b-0987-654321fedcba",
  "options": [
    {
      "text": "London",
      "isCorrect": false
    },
    {
      "text": "Paris",
      "isCorrect": true
    },
    {
      "text": "Berlin",
      "isCorrect": false
    }
  ]
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Question created successfully",
  "data": {
    "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
    "text": "What is the capital of France?",
    "category": "Geography",
    "difficulty": "Easy",
    "createdDate": "2024-12-01T10:30:00Z"
  }
}
```

**Error Responses:**
- `400` - Invalid request data
- `401` - Unauthorized
- `403` - Forbidden
- `404` - QuestionBank not found

### Get Question by ID

Retrieves a specific question.

**Endpoint:** `GET /api/v1/questions/{id}`

**Path Parameters:**
- `id` (string, required): Question ID

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Question retrieved successfully",
  "data": {
    "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
    "text": "What is the capital of France?",
    "category": "Geography",
    "difficulty": "Easy",
    "options": [
      { "text": "London", "isCorrect": false },
      { "text": "Paris", "isCorrect": true },
      { "text": "Berlin", "isCorrect": false }
    ],
    "createdDate": "2024-12-01T10:30:00Z"
  }
}
```

**Error Responses:**
- `404` - Question not found

### Update Question

Updates an existing question.

**Endpoint:** `PUT /api/v1/questions/{id}`

**Path Parameters:**
- `id` (string, required): Question ID

**Request Body:** (same as create)

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Question updated successfully",
  "data": { /* updated question */ }
}
```

**Error Responses:**
- `404` - Question not found

### Delete Question

Deletes a question.

**Endpoint:** `DELETE /api/v1/questions/{id}`

**Success Response (204):** No content

**Error Responses:**
- `404` - Question not found

### Filter Questions

Filters questions by category and difficulty.

**Endpoint:** `GET /api/v1/questions?category={category}&difficulty={difficulty}&questionBankId={id}`

**Query Parameters:**
- `category` (string, optional): Filter by category
- `difficulty` (string, optional): Filter by difficulty (Easy, Medium, Hard)
- `questionBankId` (string, optional): Filter by question bank

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "3 questions found",
  "data": {
    "questions": [
      { /* question 1 */ },
      { /* question 2 */ },
      { /* question 3 */ }
    ],
    "totalCount": 3
  }
}
```

## Question Banks API

### Create Question Bank

**Endpoint:** `POST /api/v1/question-banks`

**Request Body:**
```json
{
  "name": "Mathematics Questions",
  "description": "Bank of math questions for 10th grade",
  "ownerId": "user-id-here"
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "message": "Question bank created successfully",
  "data": {
    "id": "b8a7c6d5-e4f3-2a1b-0987-654321fedcba",
    "name": "Mathematics Questions",
    "createdAt": "2024-12-01T10:30:00Z"
  }
}
```

### Get Question Bank by ID

**Endpoint:** `GET /api/v1/question-banks/{id}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Question bank retrieved successfully",
  "data": {
    "id": "b8a7c6d5-e4f3-2a1b-0987-654321fedcba",
    "name": "Mathematics Questions",
    "description": "Bank of math questions for 10th grade",
    "questionsCount": 50,
    "createdAt": "2024-12-01T10:30:00Z"
  }
}
```

### Get Questions from Bank

**Endpoint:** `GET /api/v1/question-banks/{id}/questions`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "5 questions retrieved",
  "data": {
    "questions": [ /* array of questions */ ],
    "totalCount": 5
  }
}
```

## Tests API

### Create Test

**Endpoint:** `POST /api/v1/tests`

**Request Body:**
```json
{
  "title": "Math Midterm Exam",
  "description": "Midterm exam for 10th grade mathematics",
  "questionBankId": "b8a7c6d5-e4f3-2a1b-0987-654321fedcba",
  "templateId": "template-id-here",
  "durationMinutes": 60,
  "passingScore": 70,
  "maxAttempts": 3
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "message": "Test created successfully",
  "data": {
    "id": "t1u2i3o4-p5a6s7d8-f9g0h1j2-k3l4m5n6",
    "title": "Math Midterm Exam",
    "createdAt": "2024-12-01T10:30:00Z",
    "status": "Draft"
  }
}
```

### Get Test by ID

**Endpoint:** `GET /api/v1/tests/{id}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test retrieved successfully",
  "data": {
    "id": "t1u2i3o4-p5a6s7d8-f9g0h1j2-k3l4m5n6",
    "title": "Math Midterm Exam",
    "description": "Midterm exam for 10th grade mathematics",
    "questions": [ /* array of questions */ ],
    "durationMinutes": 60,
    "passingScore": 70,
    "maxAttempts": 3,
    "createdAt": "2024-12-01T10:30:00Z",
    "status": "Active"
  }
}
```

### Get Tests by Template

**Endpoint:** `GET /api/v1/tests?templateId={id}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "5 tests found",
  "data": {
    "tests": [ /* array of tests */ ],
    "totalCount": 5
  }
}
```

### Copy Test

**Endpoint:** `POST /api/v1/tests/{id}/copy`

**Success Response (201):**
```json
{
  "isError": false,
  "message": "Test copied successfully",
  "data": {
    "id": "new-test-id-here",
    "title": "Math Midterm Exam (Copy)",
    "createdAt": "2024-12-01T10:35:00Z",
    "status": "Draft"
  }
}
```

### Generate Test from Bank

**Endpoint:** `POST /api/v1/tests/{id}/generate`

**Request Body:**
```json
{
  "title": "Generated Test",
  "questionCount": 10,
  "difficultyFilter": "Medium"
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "message": "Test generated successfully",
  "data": {
    "id": "generated-test-id",
    "title": "Generated Test",
    "questions": [ /* 10 questions */ ],
    "createdAt": "2024-12-01T10:40:00Z"
  }
}
```

## Test Templates API

### Create Template

**Endpoint:** `POST /api/v1/templates`

**Request Body:**
```json
{
  "name": "Standard Math Exam",
  "description": "Standard template for math exams",
  "defaultDuration": 60,
  "defaultPassingScore": 70,
  "configuration": {
    "questionsCount": 10,
    "difficultyDistribution": {
      "Easy": 3,
      "Medium": 5,
      "Hard": 2
    }
  }
}
```

### Get Template by ID

**Endpoint:** `GET /api/v1/templates/{id}`

### Update Template

**Endpoint:** `PUT /api/v1/templates/{id}`

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `201` | Created - Resource created |
| `204` | No Content - DELETE succeeded |
| `400` | Bad Request - Invalid data |
| `401` | Unauthorized - Missing/invalid token |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - Resource not found |
| `409` | Conflict - Resource already exists |
| `500` | Internal Server Error |

## Error Models

### QuestionNotFoundException

```json
{
  "type": "QuestionNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Question with id '123' not found",
  "instance": "GET: /api/v1/questions/123",
  "traceId": "00-abc123..."
}
```

### QuestionBankNotFoundException

```json
{
  "type": "QuestionBankNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Question bank with id '456' not found",
  "instance": "GET: /api/v1/question-banks/456",
  "traceId": "00-abc123..."
}
```

### TestValidationException

```json
{
  "type": "TestValidationException",
  "title": "An error occurred while processing your request",
  "status": 400,
  "detail": "Test must have at least 5 questions",
  "instance": "POST: /api/v1/tests",
  "traceId": "00-abc123...",
  "violations": [
    {
      "field": "questions",
      "message": "At least 5 questions required",
      "code": "MinQuestionsCount"
    }
  ]
}
```
