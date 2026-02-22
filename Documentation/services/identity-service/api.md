# Identity Service - API Reference

## Base URL

```
http://localhost:8081/api/v1
```

## Responses

All API responses follow the `ResponseResultModel<T>` pattern.

## Auth API

### Register

Registers a new user.

**Endpoint:** `POST /api/v1/auth/register`

**Request Body:**
```json
{
  "email": "student@university.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Student"
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "User registered successfully. Please verify your email.",
  "data": {
    "userId": "u1s2t3u4-d5e6n7t8-i9o1n2g3-s4t5u6d7e8",
    "email": "student@university.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student",
    "emailVerified": false,
    "createdAt": "2024-12-01T10:30:00Z"
  }
}
```

**Validation:**
- Email must be valid format
- Password: min 8 chars, 1 uppercase, 1 lowercase, 1 number, 1 symbol
- Email must be unique

### Login

Authenticates user and returns tokens.

**Endpoint:** `POST /api/v1/auth/login`

**Request Body:**
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
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "Login successful",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "c29tZS1yZWZyZXNoLXRva2Vu...",
    "expiresIn": 60,
    "user": {
      "id": "user-id-here",
      "email": "student@university.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "Student",
      "emailVerified": true
    }
  }
}
```

**Error Responses:**
- `400` - Invalid credentials
- `401` - Unauthorized
- `403` - Email not verified

### Refresh Token

Refreshes expired access token.

**Endpoint:** `POST /api/v1/auth/refresh`

**Request Body:**
```json
{
  "refreshToken": "c29tZS1yZWZyZXNoLXRva2Vu..."
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Tokens refreshed",
  "data": {
    "accessToken": "new-eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "new-c29tZS1yZWZyZXNoLXRva2Vu...",
    "expiresIn": 60
  }
}
```

### Logout

Invalidates refresh token.

**Endpoint:** `POST /api/v1/auth/logout`

**Headers:**
```
Authorization: Bearer <accessToken>
```

**Success Response (204):** No content

### Forgot Password

Requests password reset.

**Endpoint:** `POST /api/v1/auth/forgot-password`

**Request Body:**
```json
{
  "email": "student@university.com"
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Password reset link sent to email",
  "data": null
}
```

### Reset Password

Resets user password.

**Endpoint:** `POST /api/v1/auth/reset-password`

**Request Body:**
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
  "message": "Password reset successful",
  "data": null
}
```

## Users API

### Get User by ID

**Endpoint:** `GET /api/v1/users/{id}`

**Headers:**
```
Authorization: Bearer <token>
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "User retrieved",
  "data": {
    "id": "user-id-here",
    "email": "student@university.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Student",
    "emailVerified": true,
    "createdAt": "2024-12-01T10:30:00Z"
  }
}
```

### Assign Role

**Endpoint:** `POST /api/v1/users/{id}/roles`

**Request Body:**
```json
{
  "role": "Teacher"  // Student, Teacher, Admin
}
```

**Success Response (200):**
```json
{
  "isError": false,
  "message": "Role assigned successfully",
  "data": {
    "userId": "user-id-here",
    "role": "Teacher",
    "assignedAt": "2024-12-01T10:30:00Z"
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `201` | Created - User registered |
| `204` | No Content - Logout succeeded |
| `400` | Bad Request - Invalid data |
| `401` | Unauthorized - Invalid/missing token |
| `403` | Forbidden - Email not verified |
| `404` | Not Found - User not found |
| `409` | Conflict - Duplicate email |
| `500` | Internal Server Error |

## Error Models

### InvalidCredentialsException

```json
{
  "type": "InvalidCredentialsException",
  "title": "An error occurred while processing your request",
  "status": 400,
  "detail": "Invalid email or password",
  "instance": "POST: /api/v1/auth/login",
  "traceId": "00-abc123..."
}
```

### DuplicateEmailException

```json
{
  "type": "DuplicateEmailException",
  "title": "An error occurred while processing your request",
  "status": 409,
  "detail": "Email 'student@university.com' is already registered",
  "instance": "POST: /api/v1/auth/register",
  "traceId": "00-abc123..."
}
```

### EmailNotVerifiedException

```json
{
  "type": "EmailNotVerifiedException",
  "title": "An error occurred while processing your request",
  "status": 403,
  "detail": "Please verify your email address first",
  "instance": "POST: /api/v1/auth/login",
  "traceId": "00-abc123..."
}
```

## JWT Token Structure

### Access Token Payload

```json
{
  "sub": "user-id",
  "email": "student@university.com",
  "role": "Student",
  "scope": "read write",
  "iat": 1701426600,
  "exp": 1701427200
}
```

### Refresh Token

- Stored in database
- 24-hour expiration
- One-time use
- Invalidated after refresh

## Roles

| Role | Permissions |
|------|-------------|
| `Student` | Take tests, view results, update profile |
| `Teacher` | Create tests, view student results, manage courses |
| `Admin` | Full system access, user management, system configuration |

## Example: Auth Flow

### 1. Register

```bash
POST /api/v1/auth/register
{
  "email": "student@university.com",
  "password": "SecurePassword123!",
  "role": "Student"
}
```

### 2. Verify Email

```bash
POST /api/v1/auth/verify-email
{
  "token": "verification-token-from-email"
}
```

### 3. Login

```bash
POST /api/v1/auth/login
{
  "email": "student@university.com",
  "password": "SecurePassword123!"
}
```

### 4. Get Tokens

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "c29tZS1yZWZyZXNoLXRva2Vu...",
  "expiresIn": 60
}
```

### 5. Use Access Token

```bash
GET /api/v1/students/profile
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```