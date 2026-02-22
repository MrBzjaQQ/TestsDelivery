# Angular 21 UI - API Reference

## Базовый URL

```
http://localhost:8080/api/v1
```

Все запросы требуют JWT-токен в заголовке Authorization:
```
Authorization: Bearer <token>
```

> **Важно:** Все запросы к Angular UI направляются через **BffPortalService** (порт 8080), который выступает единой точкой входа.
> - **AuthController** (`/api/v1/auth/*`) - предоставляет endpoints для аутентификации и проксирует их в IdentityService
> - **PortalController** (`/api/v1/portal/*`) - предоставляет endpoints для работы с тестами и студентами

## Формат ответов

### Успешный ответ

```typescript
interface ApiResponse<T> {
  isError: false;
  timestamp: string;
  message: string;
  data: T;
}
```

### Ошибочный ответ

```typescript
interface ApiError {
  type: string;
  title: string;
  status: number;
  detail: string;
  instance: string;
  traceId: string;
}
```

---

## Auth API

> **Примечание:** Эти endpoints предоставляются **BffPortalService** (`/api/v1/auth/*`) и проксируются к IdentityService.

### Login

Аутентификация пользователя.

**Endpoint:** `POST /api/v1/auth/login`

**Request:**
```typescript
interface LoginRequest {
  email: string;
  password: string;
}
```

**Response:**
```typescript
interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: UserDto;
}

interface UserDto {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'Student' | 'Teacher' | 'Admin';
  emailVerified: boolean;
}
```

### Register

Регистрация нового пользователя.

**Endpoint:** `POST /api/v1/auth/register`

**Request:**
```typescript
interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: 'Student' | 'Teacher';
}
```

**Response:**
```typescript
interface RegisterResponse {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  emailVerified: boolean;
  createdAt: string;
}
```

### Refresh Token

Обновление токена доступа.

**Endpoint:** `POST /api/v1/auth/refresh`

**Request:**
```typescript
interface RefreshTokenRequest {
  refreshToken: string;
}
```

**Response:**
```typescript
interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}
```

### Logout

Выход из системы.

**Endpoint:** `POST /api/v1/auth/logout`

**Response:** `204 No Content`

### Forgot Password

Запрос на сброс пароля.

**Endpoint:** `POST /api/v1/auth/forgot-password`

**Request:**
```typescript
interface ForgotPasswordRequest {
  email: string;
}
```

### Reset Password

Сброс пароля.

**Endpoint:** `POST /api/v1/auth/reset-password`

**Request:**
```typescript
interface ResetPasswordRequest {
  token: string;
  newPassword: string;
}
```

---

## Portal API

### Get Student Profile

Получение профиля студента с результатами тестов.

**Endpoint:** `GET /api/v1/portal/students/profile`

**Response:**
```typescript
interface StudentProfileDto {
  studentId: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  group: GroupDto | null;
  profile: {
    avatarUrl: string | null;
    bio: string | null;
  };
  statistics: StudentStatisticsDto;
  recentTests: RecentTestDto[];
}

interface GroupDto {
  id: string;
  name: string;
  startYear: number;
  endYear: number;
}

interface StudentStatisticsDto {
  totalTests: number;
  completedTests: number;
  inProgressTests: number;
  averageScore: number;
}

interface RecentTestDto {
  testId: string;
  testTitle: string;
  status: 'Completed' | 'InProgress' | 'Available';
  score: number;
  maxScore: number;
  percentages: number;
  isPassed: boolean;
  completedAt: string | null;
}
```

### Get Available Tests

Получение списка доступных тестов.

**Endpoint:** `GET /api/v1/portal/tests/available`

**Query Parameters:**
- `groupId` (string, optional): Фильтр по группе
- `status` (string, optional): `available`, `expired`, `completed`

**Response:**
```typescript
interface AvailableTestsResponse {
  tests: AvailableTestDto[];
  totalCount: number;
}

interface AvailableTestDto {
  testId: string;
  testTitle: string;
  description: string;
  durationMinutes: number;
  passingScore: number;
  maxAttempts: number;
  attemptsUsed: number;
  status: 'Available' | 'InProgress' | 'Completed' | 'Expired';
  availableFrom: string;
  availableUntil: string;
  questionsCount: number;
  canTake: boolean;
  score?: number;
  maxScore?: number;
  percentage?: number;
  isPassed?: boolean;
  completedAt?: string;
}
```

### Start Test

Начало теста.

**Endpoint:** `POST /api/v1/portal/tests/{testId}/start`

**Response:**
```typescript
interface StartTestResponse {
  testId: string;
  studentId: string;
  status: 'InProgress';
  startedAt: string;
  deadline: string;
  attemptsUsed: number;
  questionsCount: number;
}
```

### Get Test Questions

Получение вопросов теста.

**Endpoint:** `GET /api/v1/portal/tests/{testId}/questions`

**Response:**
```typescript
interface TestQuestionsResponse {
  testId: string;
  testTitle: string;
  testDescription: string;
  durationMinutes: number;
  passingScore: number;
  questions: TestQuestionDto[];
  totalQuestions: number;
  totalPoints: number;
}

interface TestQuestionDto {
  questionId: string;
  text: string;
  category: string;
  difficulty: 'Easy' | 'Medium' | 'Hard';
  order: number;
  options?: QuestionOptionDto[];
  imageFileId?: string;
  answerType?: 'text';
  maxLength?: number;
}

interface QuestionOptionDto {
  optionId: string;
  text: string;
  order: number;
}
```

### Submit Test Answers

Отправка ответов на тест.

**Endpoint:** `POST /api/v1/portal/tests/{testId}/submit`

**Request:**
```typescript
interface SubmitTestRequest {
  answers: AnswerSubmissionDto[];
}

interface AnswerSubmissionDto {
  questionId: string;
  selectedOptionIds: string[];
  textAnswer: string | null;
}
```

**Response:**
```typescript
interface SubmitTestResponse {
  testAssignmentId: string;
  testId: string;
  studentId: string;
  status: 'Submitted';
  submittedAt: string;
  attemptNumber: number;
  answersCount: number;
}
```

### Get Test Results

Получение результатов теста.

**Endpoint:** `GET /api/v1/portal/tests/{testId}/results`

**Response:**
```typescript
interface TestResultsResponse {
  testId: string;
  testTitle: string;
  score: number;
  maxScore: number;
  percentage: number;
  isPassed: boolean;
  passedThreshold: number;
  attemptNumber: number;
  completedAt: string;
  answers: AnswerResultDto[];
  canRetake: boolean;
  attemptsRemaining: number;
}

interface AnswerResultDto {
  questionId: string;
  questionText: string;
  selectedOptionIds: string[];
  isCorrect: boolean;
  pointsEarned: number;
  correctOptionIds?: string[];
  textAnswer?: string;
  feedback?: string;
}
```

### Get Group Analytics

Получение аналитики по группе.

**Endpoint:** `GET /api/v1/portal/groups/{id}/analytics`

**Response:**
```typescript
interface GroupAnalyticsDto {
  groupId: string;
  groupName: string;
  totalStudents: number;
  activeStudents: number;
  completedTests: number;
  averageScore: number;
  passRate: number;
  topPerformers: TopPerformerDto[];
  groupProgress: {
    testsCompleted: number;
    testsInProgress: number;
    totalTestsAssigned: number;
  };
}

interface TopPerformerDto {
  studentId: string;
  studentName: string;
  averageScore: number;
}
```

---

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK — Запрос выполнен успешно |
| `201` | Created — Ресурс создан |
| `204` | No Content — DELETE выполнен успешно |
| `400` | Bad Request — Некорректные данные |
| `401` | Unauthorized — Отсутствует/неверен токен |
| `403` | Forbidden — Недостаточно прав |
| `404` | Not Found — Ресурс не найден |
| `409` | Conflict — Ресурс уже существует |
| `500` | Internal Server Error |

---

## Сервисы Angular

### AuthService

> **Важно:** AuthService теперь использует BffPortalService (`/api/v1/auth/*`) вместо IdentityService напрямую.

```typescript
@Injectable({ providedIn: 'root' })
export class AuthService {
  readonly currentUser = signal<UserDto | null>(null);
  readonly isAuthenticated = signal(false);
  
  login(request: LoginRequest): Observable<LoginResponse>;
  register(request: RegisterRequest): Observable<RegisterResponse>;
  logout(): void;
  refreshToken(): Observable<RefreshTokenResponse>;
  forgotPassword(email: string): Observable<{ isError: boolean; message: string }>;
  resetPassword(token: string, newPassword: string): Observable<{ isError: boolean; message: string }>;
}
```

### StudentApiService

```typescript
@Injectable({ providedIn: 'root' })
export class StudentApiService {
  readonly profile = signal<StudentProfileDto | null>(null);
  readonly availableTests = signal<AvailableTestDto[]>([]);
  readonly isLoading = signal(false);
  
  loadProfile(): Observable<StudentProfileDto>;
  loadAvailableTests(filters?: TestFilters): Observable<AvailableTestsResponse>;
  startTest(testId: string): Observable<StartTestResponse>;
  getTestQuestions(testId: string): Observable<TestQuestionsResponse>;
  submitTest(testId: string, answers: AnswerSubmissionDto[]): Observable<SubmitTestResponse>;
  getTestResults(testId: string): Observable<TestResultsResponse>;
}
```

### TeacherApiService

```typescript
@Injectable({ providedIn: 'root' })
export class TeacherApiService {
  getQuestionBanks(): Observable<QuestionBankDto[]>;
  createQuestionBank(request: CreateQuestionBankRequest): Observable<QuestionBankDto>;
  getQuestions(bankId: string): Observable<QuestionDto[]>;
  createQuestion(request: CreateQuestionRequest): Observable<QuestionDto>;
  getTests(): Observable<TestDto[]>;
  createTest(request: CreateTestRequest): Observable<TestDto>;
  getStudentResults(groupId?: string): Observable<StudentResultDto[]>;
  getGroupAnalytics(groupId: string): Observable<GroupAnalyticsDto>;
}
```

### AdminApiService

```typescript
@Injectable({ providedIn: 'root' })
export class AdminApiService {
  getUsers(filters?: UserFilters): Observable<UserDto[]>;
  createUser(request: CreateUserRequest): Observable<UserDto>;
  updateUser(id: string, request: UpdateUserRequest): Observable<UserDto>;
  deleteUser(id: string): Observable<void>;
  assignRole(userId: string, role: string): Observable<void>;
  getSystemHealth(): Observable<SystemHealthDto>;
  getAuditLogs(filters?: LogFilters): Observable<AuditLogDto[]>;
}
```

---

## Обработка ошибок

### Типы ошибок

```typescript
interface ApiError {
  type: string;
  title: string;
  status: number;
  detail: string;
  instance: string;
  traceId: string;
  violations?: ValidationError[];
}

interface ValidationError {
  field: string;
  message: string;
  code: string;
}
```

### Классификация ошибок

| Type | Description |
|------|-------------|
| `InvalidCredentialsException` | Неверный email или пароль |
| `DuplicateEmailException` | Email уже зарегистрирован |
| `EmailNotVerifiedException` | Email не подтверждён |
| `TestNotFoundException` | Тест не найден |
| `TestNotAvailableException` | Тест недоступен |
| `MaxAttemptsExceededException` | Превышено количество попыток |
| `TestTimeExpiredException` | Время теста истекло |
| `PortalDataNotFoundException` | Данные не найдены |
| `ServiceUnavailableException` | Сервис временно недоступен |
