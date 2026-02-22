# Angular 21 UI - База данных

## Обзор

Angular UI является фронтенд-приложением и **не имеет собственной базы данных**. Приложение взаимодействует с **BffPortalService**, который в свою очередь обращается к микросервисам и базам данных.

## Архитектура данных

```
Angular UI (Frontend)
       │
       │ HTTP/REST
       ▼
BffPortalService (Backend-for-Frontend)
       │
       ├──► IdentityService ──────► PostgreSQL
       ├──► QuestionManagementService ──► PostgreSQL
       ├──► StudentManagementService ──► PostgreSQL
       ├──► TestCheckingService ──► PostgreSQL
       ├──► FileStorageService ──► PostgreSQL + S3/MinIO
       └──► NotificationService ──► PostgreSQL
```

## Модели данных (Frontend DTOs)

### User Models

```typescript
// user.model.ts
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  emailVerified: boolean;
  createdAt: string;
  updatedAt?: string;
}

export enum UserRole {
  Student = 'Student',
  Teacher = 'Teacher',
  Admin = 'Admin'
}

export interface StudentProfile extends User {
  group: Group | null;
  profile: {
    avatarUrl: string | null;
    bio: string | null;
  };
  statistics: StudentStatistics;
  recentTests: RecentTest[];
}

export interface Group {
  id: string;
  name: string;
  startYear: number;
  endYear: number;
}

export interface StudentStatistics {
  totalTests: number;
  completedTests: number;
  inProgressTests: number;
  averageScore: number;
}
```

### Test Models

```typescript
// test.model.ts
export interface AvailableTest {
  testId: string;
  testTitle: string;
  description: string;
  durationMinutes: number;
  passingScore: number;
  maxAttempts: number;
  attemptsUsed: number;
  status: TestStatus;
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

export enum TestStatus {
  Available = 'Available',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Expired = 'Expired'
}

export interface TestQuestions {
  testId: string;
  testTitle: string;
  testDescription: string;
  durationMinutes: number;
  passingScore: number;
  questions: TestQuestion[];
  totalQuestions: number;
  totalPoints: number;
}

export interface TestQuestion {
  questionId: string;
  text: string;
  category: string;
  difficulty: QuestionDifficulty;
  order: number;
  options?: QuestionOption[];
  imageFileId?: string;
  answerType?: 'text';
  maxLength?: number;
}

export interface QuestionOption {
  optionId: string;
  text: string;
  order: number;
}

export enum QuestionDifficulty {
  Easy = 'Easy',
  Medium = 'Medium',
  Hard = 'Hard'
}
```

### Result Models

```typescript
// result.model.ts
export interface TestResults {
  testId: string;
  testTitle: string;
  score: number;
  maxScore: number;
  percentage: number;
  isPassed: boolean;
  passedThreshold: number;
  attemptNumber: number;
  completedAt: string;
  answers: AnswerResult[];
  canRetake: boolean;
  attemptsRemaining: number;
}

export interface AnswerResult {
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

### Analytics Models

```typescript
// analytics.model.ts
export interface GroupAnalytics {
  groupId: string;
  groupName: string;
  totalStudents: number;
  activeStudents: number;
  completedTests: number;
  averageScore: number;
  passRate: number;
  topPerformers: TopPerformer[];
  groupProgress: {
    testsCompleted: number;
    testsInProgress: number;
    totalTestsAssigned: number;
  };
}

export interface TopPerformer {
  studentId: string;
  studentName: string;
  averageScore: number;
}
```

### Question Bank Models

```typescript
// question-bank.model.ts
export interface QuestionBank {
  id: string;
  name: string;
  description: string;
  ownerId: string;
  questionsCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface Question {
  id: string;
  text: string;
  category: string;
  difficulty: QuestionDifficulty;
  questionBankId: string;
  options: QuestionOptionWithCorrect[];
  imageFileId?: string;
  answerType: AnswerType;
  maxLength?: number;
  points: number;
  createdAt: string;
  updatedAt: string;
}

export interface QuestionOptionWithCorrect extends QuestionOption {
  isCorrect: boolean;
}

export enum AnswerType {
  SingleChoice = 'single',
  MultipleChoice = 'multiple',
  Text = 'text'
}
```

---

## Local Storage

Приложение использует **Local Storage** для хранения некоторых данных на клиенте:

### Token Storage

```typescript
// token.service.ts
@Injectable({ providedIn: 'root' })
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'td_access_token';
  private readonly REFRESH_TOKEN_KEY = 'td_refresh_token';
  private readonly TOKEN_EXPIRY_KEY = 'td_token_expiry';

  get accessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  get refreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  setTokens(accessToken: string, refreshToken: string, expiresIn: number): void {
    const expiryDate = new Date().getTime() + expiresIn * 1000;
    
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiryDate.toString());
  }

  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
  }

  isTokenExpired(): boolean {
    const expiry = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    if (!expiry) return true;
    return Date.now() > parseInt(expiry, 10);
  }
}
```

### User Preferences Storage

```typescript
// preferences.service.ts
@Injectable({ providedIn: 'root' })
export class PreferencesService {
  private readonly PREFERENCES_KEY = 'td_preferences';

  interface UserPreferences {
    theme: 'light' | 'dark' | 'system';
    language: 'en' | 'ru';
    notifications: {
      email: boolean;
      push: boolean;
    };
    testTaking: {
      showTimer: boolean;
      autoSubmit: boolean;
      shuffleQuestions: boolean;
    };
  }

  getPreferences(): UserPreferences {
    const stored = localStorage.getItem(this.PREFERENCES_KEY);
    return stored ? JSON.parse(stored) : this.getDefaultPreferences();
  }

  setPreferences(prefs: Partial<UserPreferences>): void {
    const current = this.getPreferences();
    const updated = { ...current, ...prefs };
    localStorage.setItem(this.PREFERENCES_KEY, JSON.stringify(updated));
  }

  private getDefaultPreferences(): UserPreferences {
    return {
      theme: 'system',
      language: 'ru',
      notifications: {
        email: true,
        push: true
      },
      testTaking: {
        showTimer: true,
        autoSubmit: true,
        shuffleQuestions: false
      }
    };
  }
}
```

### Draft Answers Storage

```typescript
// draft-answers.service.ts
@Injectable({ providedIn: 'root' })
export class DraftAnswersService {
  private readonly DRAFT_KEY = 'td_draft_answers';

  saveDraft(testId: string, answers: DraftAnswer[]): void {
    const drafts = this.getDrafts();
    drafts[testId] = {
      answers,
      savedAt: new Date().toISOString()
    };
    localStorage.setItem(this.DRAFT_KEY, JSON.stringify(drafts));
  }

  getDraft(testId: string): DraftAnswer[] | null {
    const drafts = this.getDrafts();
    return drafts[testId]?.answers ?? null;
  }

  clearDraft(testId: string): void {
    const drafts = this.getDrafts();
    delete drafts[testId];
    localStorage.setItem(this.DRAFT_KEY, JSON.stringify(drafts));
  }

  private getDrafts(): Record<string, { answers: DraftAnswer[]; savedAt: string }> {
    const stored = localStorage.getItem(this.DRAFT_KEY);
    return stored ? JSON.parse(stored) : {};
  }
}

interface DraftAnswer {
  questionId: string;
  selectedOptionIds: string[];
  textAnswer: string | null;
}
```

---

## Session Storage

Используется для данных, которые должны быть доступны только в рамках одной сессии:

```typescript
// session-storage.service.ts
@Injectable({ providedIn: 'root' })
export class SessionStorageService {
  private readonly TEST_SESSION_KEY = 'td_test_session';

  saveTestSession(session: TestSession): void {
    sessionStorage.setItem(this.TEST_SESSION_KEY, JSON.stringify(session));
  }

  getTestSession(): TestSession | null {
    const stored = sessionStorage.getItem(this.TEST_SESSION_KEY);
    return stored ? JSON.parse(stored) : null;
  }

  clearTestSession(): void {
    sessionStorage.removeItem(this.TEST_SESSION_KEY);
  }
}

interface TestSession {
  testId: string;
  startedAt: string;
  deadline: string;
  currentQuestionIndex: number;
}
```

---

## Индексная база данных (IndexedDB)

Для более сложных операций с данными на клиенте используется IndexedDB:

```typescript
// indexed-db.service.ts
@Injectable({ providedIn: 'root' })
export class IndexedDbService {
  private dbName = 'TestsDeliveryDB';
  private version = 1;
  private db: IDBDatabase | null = null;

  async init(): Promise<void> {
    return new Promise((resolve, reject) => {
      const request = indexedDB.open(this.dbName, this.version);

      request.onerror = () => reject(request.error);
      request.onsuccess = () => {
        this.db = request.result;
        resolve();
      };

      request.onupgradeneeded = (event) => {
        const db = (event.target as IDBOpenDBRequest).result;

        // Хранилище кэшированных вопросов
        if (!db.objectStoreNames.contains('cachedQuestions')) {
          db.createObjectStore('cachedQuestions', { keyPath: 'questionId' });
        }

        // Хранилище оффлайн-ответов
        if (!db.objectStoreNames.contains('offlineAnswers')) {
          const store = db.createObjectStore('offlineAnswers', { 
            keyPath: 'testId', 
            autoIncrement: false 
          });
          store.createIndex('savedAt', 'savedAt', { unique: false });
        }

        // Хранилище загруженных файлов
        if (!db.objectStoreNames.contains('uploadedFiles')) {
          db.createObjectStore('uploadedFiles', { keyPath: 'id' });
        }
      };
    });
  }

  async cacheQuestions(questions: CachedQuestion[]): Promise<void> {
    if (!this.db) await this.init();
    
    const tx = this.db!.transaction('cachedQuestions', 'readwrite');
    const store = tx.objectStore('cachedQuestions');
    
    questions.forEach(q => store.put(q));
    
    return new Promise((resolve, reject) => {
      tx.oncomplete = () => resolve();
      tx.onerror = () => reject(tx.error);
    });
  }

  async saveOfflineAnswers(testId: string, answers: unknown): Promise<void> {
    if (!this.db) await this.init();
    
    const tx = this.db!.transaction('offlineAnswers', 'readwrite');
    const store = tx.objectStore('offlineAnswers');
    
    store.put({
      testId,
      answers,
      savedAt: new Date().toISOString(),
      synced: false
    });
    
    return new Promise((resolve, reject) => {
      tx.oncomplete = () => resolve();
      tx.onerror = () => reject(tx.error);
    });
  }
}
```

---

## Схема взаимодействия

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         Angular UI Application                         │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────────────────────┐ │
│  │  Local      │    │  Session    │    │  IndexedDB                  │ │
│  │  Storage    │    │  Storage    │    │                             │ │
│  │             │    │             │    │  - Cached questions         │ │
│  │ - JWT Token │    │ - Test      │    │  - Offline answers          │ │
│  │ - Prefs     │    │   Session   │    │  - Uploaded files           │ │
│  │ - Drafts    │    │             │    │                             │ │
│  └─────────────┘    └─────────────┘    └─────────────────────────────┘ │
│                                                                         │
│         │                    │                        │                │
│         ▼                    ▼                        ▼                │
│  ┌──────────────────────────────────────────────────────────────────┐   │
│  │                      State Management (Signals)                  │   │
│  │                                                                  │   │
│  │  AuthService    │  StudentService  │  TeacherService  │ etc.    │   │
│  └──────────────────────────────────────────────────────────────────┘   │
│                                    │                                   │
│                                    ▼                                   │
│  ┌──────────────────────────────────────────────────────────────────┐   │
│  │                    HTTP Interceptors                             │   │
│  │                                                                  │   │
│  │  AuthInterceptor  │  ErrorInterceptor  │  LoadingInterceptor   │   │
│  └──────────────────────────────────────────────────────────────────┘   │
│                                    │                                   │
│                                    ▼                                   │
│  ┌──────────────────────────────────────────────────────────────────┐   │
│  │                  BffPortalService (API)                           │   │
│  │                       http://localhost:8080/api/v1               │   │
│  └──────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
                    ┌───────────────────────────────────┐
                    │         Microservices              │
                    │                                   │
                    │  IdentityService                  │
                    │  QuestionManagementService        │
                    │  StudentManagementService         │
                    │  TestCheckingService              │
                    │  FileStorageService               │
                    │  NotificationService              │
                    └───────────────────────────────────┘
                                    │
                                    ▼
                    ┌───────────────────────────────────┐
                    │         PostgreSQL Databases       │
                    │                                   │
                    │  - identity_db                   │
                    │  - question_db                   │
                    │  - student_db                    │
                    │  - checking_db                   │
                    │  - file_db                        │
                    │  - notification_db               │
                    └───────────────────────────────────┘
```
