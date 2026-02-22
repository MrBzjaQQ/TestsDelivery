# Angular 21 UI - Страницы

## Обзор страниц

Приложение разделено на 4 основных раздела по ролям:

1. **Auth** — страницы аутентификации
2. **Student** — портал студента
3. **Teacher** — панель преподавателя
4. **Admin** — панель администратора

---

## Auth Pages (Страницы аутентификации)

> **Важно:** Все API-вызовы аутентификации направляются через **BffPortalService** (порт 8080), который предоставляет `AuthController` и проксирует запросы к **IdentityService** (порт 8081).

### LoginPage

**Путь:** `/login`

**Назначение:** Вход пользователя в систему.

**Компоненты:**
- Форма входа (email, password)
- Кнопка "Войти"
- Ссылка "Забыли пароль?"
- Ссылка "Регистрация"

**API:** `POST /api/v1/auth/login`

**Логика:**
```typescript
@Component({
  selector: 'app-login-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss']
})
export class LoginPage {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(NonNullableFormBuilder);
  
  readonly loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    rememberMe: [false]
  });
  
  readonly isLoading = this.authService.isLoading;
  
  onSubmit(): void {
    if (this.loginForm.invalid) return;
    
    const { email, password } = this.loginForm.getRawValue();
    this.authService.login({ email, password }).subscribe({
      next: () => this.router.navigate(['/student/dashboard']),
      error: (error) => this.handleError(error)
    });
  }
}
```

**Шаблон:**
```html
<div class="auth-container">
  <div class="auth-card">
    <div class="auth-header">
      <img src="assets/images/logo.svg" alt="Logo" class="logo">
      <h1>Вход в систему</h1>
    </div>
    
    <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
      <app-input
        formControlName="email"
        label="Email"
        type="email"
        placeholder="Введите email"
        [errorMessages]="{ required: 'Email обязателен', email: 'Некорректный email' }">
      </app-input>
      
      <app-input
        formControlName="password"
        label="Пароль"
        type="password"
        placeholder="Введите пароль"
        [errorMessages]="{ required: 'Пароль обязателен', minlength: 'Минимум 8 символов' }">
      </app-input>
      
      <div class="form-row">
        <app-checkbox formControlName="rememberMe" label="Запомнить меня"></app-checkbox>
        <a routerLink="/auth/forgot-password" class="forgot-link">Забыли пароль?</a>
      </div>
      
      <app-button type="submit" [loading]="isLoading()" [disabled]="loginForm.invalid">
        Войти
      </app-button>
    </form>
    
    <div class="auth-footer">
      <p>Нет аккаунта? <a routerLink="/auth/register">Зарегистрироваться</a></p>
    </div>
  </div>
</div>
```

---

### RegisterPage

**Путь:** `/register`

**Назначение:** Регистрация нового пользователя.

**Компоненты:**
- Форма регистрации (firstName, lastName, email, password, confirmPassword, role)
- Кнопка "Зарегистрироваться"
- Ссылка "Уже есть аккаунт? Войти"

**API:** `POST /api/v1/auth/register`

**Форма:**
```typescript
readonly registerForm = this.fb.group({
  firstName: ['', [Validators.required, Validators.minLength(2)]],
  lastName: ['', [Validators.required, Validators.minLength(2)]],
  email: ['', [Validators.required, Validators.email]],
  password: ['', [Validators.required, Validators.minLength(8), this.passwordStrengthValidator]],
  confirmPassword: ['', [Validators.required]],
  role: ['Student', Validators.required]
}, { validators: this.passwordMatchValidator });
```

---

### ForgotPasswordPage

**Путь:** `/forgot-password`

**Назначение:** Запрос на восстановление пароля.

**Компоненты:**
- Форма с полем email
- Кнопка "Отправить ссылку"
- Сообщение об отправке письма

**API:** `POST /api/v1/auth/forgot-password`

---

### ResetPasswordPage

**Путь:** `/reset-password?token=xxx`

**Назначение:** Сброс пароля по токену из email.

**Компоненты:**
- Форма с полями newPassword, confirmPassword
- Кнопка "Сохранить пароль"

**API:** `POST /api/v1/auth/reset-password`

---

## Student Pages (Страницы студента)

### StudentDashboardPage

**Путь:** `/dashboard`

**Назначение:** Главная страница студента с обзором статистики и списка тестов.

**Компоненты:**
- Карточка профиля студента
- Статистика (всего тестов, пройдено, средний балл)
- Список недавних тестов
- Быстрый доступ к доступным тестам

**Логика:**
```typescript
@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard.page.html',
  styleUrls: ['./dashboard.page.scss']
})
export class StudentDashboardPage implements OnInit {
  private readonly studentService = inject(StudentApiService);
  
  readonly profile = this.studentService.profile;
  readonly isLoading = this.studentService.isLoading;
  
  ngOnInit(): void {
    this.studentService.loadProfile();
  }
  
  get statistics() {
    return this.profile()?.statistics;
  }
  
  get recentTests() {
    return this.profile()?.recentTests ?? [];
  }
}
```

**Шаблон:**
```html
<div class="dashboard-container">
  <!-- Header с профилем -->
  <section class="profile-section">
    <app-avatar [url]="profile()?.profile?.avatarUrl" [name]="fullName()"></app-avatar>
    <div class="profile-info">
      <h1>{{ profile()?.firstName }} {{ profile()?.lastName }}</h1>
      <p class="group-name" *ngIf="profile()?.group">{{ profile()?.group?.name }}</p>
    </div>
  </section>
  
  <!-- Статистика -->
  <section class="statistics-section">
    <app-card class="stat-card">
      <app-stat-item 
        icon="assignment" 
        label="Всего тестов" 
        [value]="statistics?.totalTests ?? 0">
      </app-stat-item>
    </app-card>
    <app-card class="stat-card">
      <app-stat-item 
        icon="check_circle" 
        label="Пройдено" 
        [value]="statistics?.completedTests ?? 0">
      </app-stat-item>
    </app-card>
    <app-card class="stat-card">
      <app-stat-item 
        icon="trending_up" 
        label="Средний балл" 
        [value]="(statistics?.averageScore ?? 0) + '%'">
      </app-stat-item>
    </app-card>
  </section>
  
  <!-- Недавние тесты -->
  <section class="recent-tests-section">
    <h2>Недавние тесты</h2>
    <div class="tests-grid">
      @for (test of recentTests; track test.testId) {
        <app-test-card [test]="test"></app-test-card>
      }
    </div>
  </section>
</div>
```

---

### TestListPage

**Путь:** `/student/tests`

**Назначение:** Список всех доступных тестов с фильтрацией.

**Компоненты:**
- Фильтры (статус, группа)
- Поиск по названию
- Список тестов в виде карточек
- Пагинация

**Фильтры:**
```typescript
readonly filters = signal<TestFilters>({
  status: 'available',
  groupId: null,
  search: ''
});

readonly filteredTests = computed(() => {
  const tests = this.studentService.availableTests();
  const { status, search } = this.filters();
  
  return tests.filter(test => {
    if (status && test.status !== status) return false;
    if (search && !test.testTitle.toLowerCase().includes(search.toLowerCase())) return false;
    return true;
  });
});
```

---

### TestTakingPage

**Путь:** `/test/:id`

**Назначение:** Страница прохождения теста.

**Компоненты:**
- Таймер обратного отсчёта
- Навигация по вопросам
- Карточка вопроса
- Кнопка "Завершить тест"
- Модальное окно подтверждения

**Логика:**
```typescript
@Component({
  selector: 'app-test-taking',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './test-taking.page.html',
  styleUrls: ['./test-taking.page.scss']
})
export class TestTakingPage implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly studentService = inject(StudentApiService);
  private readonly testTakingService = inject(TestTakingService);
  
  readonly testQuestions = this.testTakingService.testQuestions;
  readonly currentQuestionIndex = this.testTakingService.currentQuestionIndex;
  readonly timeRemaining = this.testTakingService.timeRemaining;
  readonly answers = this.testTakingService.answers;
  
  private destroyRef = inject(DestroyRef);
  
  ngOnInit(): void {
    const testId = this.route.snapshot.paramMap.get('id')!;
    this.studentService.startTest(testId).subscribe(() => {
      this.studentService.getTestQuestions(testId).subscribe();
    });
    
    this.testTakingService.startTimer();
  }
  
  ngOnDestroy(): void {
    this.testTakingService.stopTimer();
  }
  
  get currentQuestion(): TestQuestionDto | null {
    return this.testQuestions()?.questions[this.currentQuestionIndex()] ?? null;
  }
  
  selectOption(optionId: string): void {
    this.testTakingService.selectOption(this.currentQuestion!.questionId, optionId);
  }
  
  nextQuestion(): void {
    this.testTakingService.nextQuestion();
  }
  
  previousQuestion(): void {
    this.testTakingService.previousQuestion();
  }
  
  submitTest(): void {
    this.testTakingService.submitTest().subscribe({
      next: () => this.router.navigate(['/student/tests', this.testId, 'results']),
      error: (error) => this.handleError(error)
    });
  }
}
```

**Шаблон:**
```html
<div class="test-taking-container">
  <!-- Header с таймером -->
  <header class="test-header">
    <h1>{{ testQuestions()?.testTitle }}</h1>
    <app-timer [timeRemaining]="timeRemaining()" (timeUp)="submitTest()"></app-timer>
  </header>
  
  <!-- Прогресс -->
  <app-progress-bar 
    [current]="currentQuestionIndex() + 1" 
    [total]="testQuestions()?.totalQuestions ?? 0">
  </app-progress-bar>
  
  <!-- Вопрос -->
  <app-question-card
    [question]="currentQuestion"
    [selectedOptions]="answers()[currentQuestion?.questionId ?? ''] ?? []"
    (optionSelected)="selectOption($event)">
  </app-question-card>
  
  <!-- Навигация -->
  <div class="navigation">
    <app-button variant="secondary" (click)="previousQuestion()" [disabled]="currentQuestionIndex() === 0">
      Назад
    </app-button>
    <app-button (click)="nextQuestion()" [disabled]="currentQuestionIndex() === (testQuestions()?.questions.length ?? 0) - 1">
      Далее
    </app-button>
    <app-button variant="primary" (click)="submitTest()">
      Завершить тест
    </app-button>
  </div>
</div>
```

---

### TestResultsPage

**Путь:** `/results/:id`

**Назначение:** Просмотр результатов пройденного теста.

**Компоненты:**
- Общий балл с визуализацией
- Статус (пройден/не пройден)
- Детализация по вопросам
- Кнопка "Пересдать" (если доступно)

**Логика:**
```typescript
@Component({
  selector: 'app-test-results',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './test-results.page.html',
  styleUrls: ['./test-results.page.scss']
})
export class TestResultsPage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly studentService = inject(StudentApiService);
  
  readonly results = this.studentService.testResults;
  readonly isLoading = this.studentService.isLoading;
  
  ngOnInit(): void {
    const testId = this.route.snapshot.paramMap.get('id')!;
    this.studentService.getTestResults(testId);
  }
  
  retakeTest(): void {
    const testId = this.route.snapshot.paramMap.get('id')!;
    this.studentService.startTest(testId).subscribe(() => {
      this.router.navigate(['/student/tests', testId]);
    });
  }
}
```

**Шаблон:**
```html
<div class="results-container">
  @if (results(); as results) {
    <!-- Итоговый балл -->
    <section class="score-section">
      <app-score-display
        [score]="results.score"
        [maxScore]="results.maxScore"
        [percentage]="results.percentage"
        [isPassed]="results.isPassed">
      </app-score-display>
      
      <div class="score-details">
        <p class="status" [class.passed]="results.isPassed" [class.failed]="!results.isPassed">
          {{ results.isPassed ? 'Тест пройден' : 'Тест не пройден' }}
        </p>
        <p>Пороговый балл: {{ results.passedThreshold }}%</p>
        <p>Попытка: {{ results.attemptNumber }}</p>
        <p>Завершён: {{ results.completedAt | dateFormat }}</p>
      </div>
    </section>
    
    <!-- Детализация ответов -->
    <section class="answers-section">
      <h2>Ваши ответы</h2>
      <app-accordion>
        @for (answer of results.answers; track answer.questionId) {
          <app-answer-result [answer]="answer"></app-answer-result>
        }
      </app-accordion>
    </section>
    
    <!-- Действия -->
    <section class="actions-section">
      @if (results.canRetake) {
        <app-button (click)="retakeTest()">
          Пересдать тест (осталось попыток: {{ results.attemptsRemaining }})
        </app-button>
      }
      <app-button variant="secondary" routerLink="/student/dashboard">
        На главную
      </app-button>
    </section>
  }
</div>
```

---

### ProfilePage

**Путь:** `/student/profile`

**Назначение:** Просмотр и редактирование профиля студента.

**Компоненты:**
- Аватар (с возможностью загрузки)
- Форма редактирования профиля
- Смена пароля
- Статистика

---

## Teacher Pages (Страницы преподавателя)

### TeacherDashboardPage

**Путь:** `/teacher/dashboard`

**Назначение:** Главная страница преподавателя.

**Компоненты:**
- Статистика (всего тестов, студентов, средний балл)
- Список активных тестов
- Последние результаты студентов
- Быстрые действия

---

### QuestionBanksPage

**Путь:** `/teacher/question-banks`

**Назначение:** Управление банками вопросов.

**Компоненты:**
- Список банков вопросов
- Создание нового банка
- Поиск и фильтрация
- Действия: редактировать, удалить

---

### QuestionsPage

**Путь:** `/teacher/questions`

**Назначение:** Управление вопросами.

**Компоненты:**
- Фильтр по банку вопросов
- Список вопросов
- Редактор вопроса
- Массовые операции

**Редактор вопроса:**
```typescript
@Component({
  selector: 'app-question-editor',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './question-editor.component.html',
  styleUrls: ['./question-editor.component.scss']
})
export class QuestionEditorComponent {
  @Input() question: QuestionDto | null = null;
  @Output() saved = new EventEmitter<QuestionDto>();
  @Output() cancelled = new EventEmitter<void>();
  
  readonly questionForm = this.fb.group({
    text: ['', Validators.required],
    category: ['', Validators.required],
    difficulty: ['Medium', Validators.required],
    questionBankId: ['', Validators.required],
    options: this.fb.array([])
  });
  
  get options(): FormArray {
    return this.questionForm.get('options') as FormArray;
  }
  
  addOption(): void {
    this.options.push(this.fb.group({
      text: ['', Validators.required],
      isCorrect: [false]
    }));
  }
  
  removeOption(index: number): void {
    this.options.removeAt(index);
  }
  
  save(): void {
    if (this.questionForm.invalid) return;
    this.saved.emit(this.questionForm.getRawValue());
  }
}
```

---

### TestsManagementPage

**Путь:** `/teacher/tests`

**Назначение:** Управление тестами.

**Компоненты:**
- Список тестов
- Создание теста
- Назначение группам
- Активация/деактивация

---

### TestEditPage

**Путь:** `/teacher/tests/:id`

**Назначение:** Редактирование теста.

**Компоненты:**
- Форма теста (название, описание, время, проходной балл)
- Выбор вопросов из банка
- Настройки попыток
- Назначение группам

---

### TeacherResultsPage

**Путь:** `/teacher/results`

**Назначение:** Просмотр результатов студентов.

**Компоненты:**
- Фильтр по тесту/группе
- Таблица результатов
- Экспорт в CSV/Excel
- Детализация по студенту

---

### GroupsPage

**Путь:** `/teacher/groups`

**Назначение:** Управление группами студентов.

**Компоненты:**
- Список групп
- Студенты в группе
- Статистика по группе
- Назначение тестов

---

### AnalyticsPage

**Путь:** `/teacher/analytics`

**Назначение:** Аналитика и отчёты.

**Компоненты:**
- Графики успеваемости
- Топ студенты
- Распределение оценок
- Сравнение групп

---

## Admin Pages (Страницы администратора)

### AdminDashboardPage

**Путь:** `/admin/dashboard`

**Назначение:** Главная страница администратора.

**Компоненты:**
- Количество пользователей по ролям
- Активность системы
- Статус сервисов (Health Checks)
- Последние действия

---

### UsersPage

**Путь:** `/admin/users`

**Назначение:** Управление пользователями.

**Компоненты:**
- Список пользователей
- Поиск и фильтрация
- Создание/редактирование пользователя
- Назначение ролей
- Блокировка/разблокировка

---

### AdminGroupsPage

**Путь:** `/admin/groups`

**Назначение:** Управление группами.

**Компоненты:**
- Список групп
- Создание группы
- Добавление студентов в группу
- Статистика по группам

---

### SystemPage

**Путь:** `/admin/system`

**Назначение:** Конфигурация системы.

**Компоненты:**
- Настройки аутентификации
- Настройки уведомлений
- Настройки файлов
- Переменные окружения

---

### LogsPage

**Путь:** `/admin/logs`

**Назначение:** Просмотр логов аудита.

**Компоненты:**
- Фильтры по дате, пользователю, действию
- Таблица логов
- Детализация действия
- Экспорт логов

---

## Общие страницы

### NotFoundPage

**Путь:** `**` (wildcard)

**Назначение:** Страница 404.

**Компоненты:**
- Сообщение об ошибке
- Кнопка "На главную"

### ForbiddenPage

**Путь:** `/forbidden`

**Назначение:** Страница доступа запрещён.

**Компоненты:**
- Сообщение о недостатке прав
- Кнопка "На главную"
