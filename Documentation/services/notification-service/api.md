# Notification Service - API Reference

## Base URL

```
http://localhost:8086/api/v1
```

## Responses

All API responses follow the `ResponseResultModel<T>` pattern.

## Notifications API

### Send Email

Sends an email notification.

**Endpoint:** `POST /api/v1/notifications/send-email`

**Request Body:**
```json
{
  "to": "student@university.com",
  "subject": "New Test Available",
  "templateName": "test-created",
  "templateData": {
    "testTitle": "Math Midterm",
    "studentName": "John Doe",
    "testLink": "https://portal.com/tests/123"
  }
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Email sent successfully",
  "data": {
    "emailId": "e1m2a3i4-l5s6t7o8-r9e0s1t2",
    "to": "student@university.com",
    "subject": "New Test Available",
    "sentAt": "2024-12-01T10:30:00Z",
    "status": "Sent"
  }
}
```

**Error Responses:**
- `400` - Invalid email or template
- `404` - Template not found
- `500` - Email send failed

### Send Test Created Notification

Sends notification about new test.

**Endpoint:** `POST /api/v1/notifications/test-created/{testId}`

**Path Parameters:**
- `testId` (string, required): Test ID

**Request Body:**
```json
{
  "studentId": "student-id-here",
  "studentEmail": "student@university.com",
  "studentName": "John Doe"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test created notification sent",
  "data": {
    "notificationId": "n1o2t3i4-f5i6n7g8-s9e0n1t2",
    "studentId": "student-id-here",
    "testId": "test-id-here",
    "sentAt": "2024-12-01T10:30:00Z"
  }
}
```

### Send Test Result Notification

Sends notification about test result.

**Endpoint:** `POST /api/v1/notifications/result-ready/{studentId}/{testId}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Test result notification sent",
  "data": {
    "studentId": "student-id-here",
    "testId": "test-id-here",
    "score": 85,
    "maxScore": 100,
    "isPassed": true,
    "sentAt": "2024-12-01T10:30:00Z"
  }
}
```

### Get Pending Notifications

Gets all pending notifications.

**Endpoint:** `GET /api/v1/notifications/pending`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "10 pending notifications",
  "data": {
    "notifications": [
      {
        "id": "n1o2t3i4-f5i6n7g8-s9e0n1t2",
        "to": "student@university.com",
        "subject": "New Test Available",
        "templateName": "test-created",
        "status": "Pending",
        "retryCount": 0,
        "createdAt": "2024-12-01T10:30:00Z"
      },
      { /* another notification */ }
    ],
    "totalCount": 10
  }
}
```

### Get Notification Status

Gets status of a specific notification.

**Endpoint:** `GET /api/v1/notifications/{id}`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Notification retrieved",
  "data": {
    "id": "n1o2t3i4-f5i6n7g8-s9e0n1t2",
    "to": "student@university.com",
    "subject": "New Test Available",
    "templateName": "test-created",
    "status": "Sent",
    "sentAt": "2024-12-01T10:30:00Z",
    "errorMessage": null
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `201` | Created - Notification queued |
| `400` | Bad Request - Invalid request |
| `401` | Unauthorized - Missing/invalid token |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - Notification/template not found |
| `500` | Internal Server Error |

## Error Models

### NotificationNotFoundException

```json
{
  "type": "NotificationNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "Notification with id '123' not found",
  "instance": "GET: /api/v1/notifications/123",
  "traceId": "00-abc123..."
}
```

### EmailSendFailedException

```json
{
  "type": "EmailSendFailedException",
  "title": "An error occurred while processing your request",
  "status": 500,
  "detail": "Failed to send email to student@university.com",
  "instance": "POST: /api/v1/notifications/send-email",
  "traceId": "00-abc123...",
  "innerError": "SMTP server unreachable"
}
```

## MassTransit Events

### TestCreatedEvent

**Properties:**
```json
{
  "testId": "guid",
  "testTitle": "string",
  "studentId": "guid",
  "studentName": "string",
  "studentEmail": "string",
  "createdBy": "string",
  "createdAt": "datetime"
}
```

### TestResultReceivedEvent

**Properties:**
```json
{
  "testId": "guid",
  "studentId": "guid",
  "studentName": "string",
  "studentEmail": "string",
  "score": "integer",
  "maxScore": "integer",
  "percentage": "double",
  "isPassed": "boolean",
  "passedDate": "datetime"
}
```

## Email Templates

### test-created.hbs

```handlebars
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>{{testTitle}}</title>
</head>
<body>
    <h1>Hello {{studentName}}!</h1>
    <p>A new test is available for you:</p>
    <h2>{{testTitle}}</h2>
    <p>You can take the test by clicking the button below:</p>
    <a href="{{testLink}}" style="background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">Take Test</a>
    <p>Best regards,<br>TestsDelivery Team</p>
</body>
</html>
```

### test-result.hbs

```handlebars
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
        <p>Status: <strong style="color: {{#if isPassed}}green{{else}}red{{/if}};">{{#if isPassed}}Passed{{else}}Failed{{/if}}</strong></p>
    </div>
    <p>Best regards,<br>TestsDelivery Team</p>
</body>
</html>
```

## Example: Notification Flow

### 1. Event Published

```json
{
  "testId": "test-id-here",
  "studentId": "student-id-here",
  "studentName": "John Doe",
  "studentEmail": "john@university.com",
  "title": "Math Midterm"
}
```

### 2. Consumer Handles Event

```csharp
// TestCreatedConsumer receives event
// Calls EmailNotificationService.SendEmailAsync()
// Renders template with test data
// Sends email via SMTP
```

### 3. Email Sent

```json
{
  "isError": false,
  "message": "Email sent successfully",
  "data": {
    "to": "john@university.com",
    "subject": "New Test Available: Math Midterm",
    "status": "Sent"
  }
}
```