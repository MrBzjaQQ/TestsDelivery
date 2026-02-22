# Angular 21 UI - Архитектура

## Структура проекта

```
TestsDelivery.Web/
├── src/
│   ├── app/
│   │   ├── core/                           # Core-модуль
│   │   │   ├── services/                   # Сервисы-одиночки
│   │   │   │   ├── auth.service.ts         # Аутентификация
│   │   │   │   ├── token.service.ts        # Управление токенами
│   │   │   │   ├── api.service.ts          # Базовый API-сервис
│   │   │   │   └── error-handling.service.ts
│   │   │   ├── interceptors/               # HTTP-интерцепторы
│   │   │   │   ├── auth.interceptor.ts     # Добавление токена
│   │   │   │   ├── error.interceptor.ts    # Обработка ошибок
│   │   │   │   └── loading.interceptor.ts  # Индикатор загрузки
│   │   │   ├── guards/                     # Защита маршрутов
│   │   │   │   ├── auth.guard.ts           # Проверка авторизации
│   │   │   │   ├── role.guard.ts           # Проверка роли
│   │   │   │   └── guest.guard.ts          # Для неавторизованных
│   │   │   └── models/                     # Базовые модели
│   │   │       ├── user.model.ts
│   │   │       ├── token.model.ts
│   │   │       └── api-response.model.ts
│   │   │
│   │   ├── features/                       # Feature-модули
│   │   │   ├── auth/                       # Модуль аутентификации
│   │   │   │   ├── pages/
│   │   │   │   │   ├── login/
│   │   │   │   │   │   ├── login.page.ts
│   │   │   │   │   │   ├── login.page.html
│   │   │   │   │   │   └── login.page.scss
│   │   │   │   │   ├── register/
│   │   │   │   │   │   ├── register.page.ts
│   │   │   │   │   │   ├── register.page.html
│   │   │   │   │   │   └── register.page.scss
│   │   │   │   │   ├── forgot-password/
│   │   │   │   │   └── reset-password/
│   │   │   │   ├── components/
│   │   │   │   │   └── auth-form/
│   │   │   │   ├── services/
│   │   │   │   │   └── auth-api.service.ts
│   │   │   │   └── auth.routes.ts
│   │   │   │
│   │   │   ├── student/                    # Модуль студента
│   │   │   │   ├── pages/
│   │   │   │   │   ├── dashboard/
│   │   │   │   │   │   ├── dashboard.page.ts
│   │   │   │   │   │   ├── dashboard.page.html
│   │   │   │   │   │   └── dashboard.page.scss
│   │   │   │   │   ├── test-list/
│   │   │   │   │   ├── test-taking/
│   │   │   │   │   ├── test-results/
│   │   │   │   │   └── profile/
│   │   │   │   ├── components/
│   │   │   │   │   ├── test-card/
│   │   │   │   │   ├── question-card/
│   │   │   │   │   ├── timer/
│   │   │   │   │   ├── progress-bar/
│   │   │   │   │   └── score-display/
│   │   │   │   ├── services/
│   │   │   │   │   ├── student-api.service.ts
│   │   │   │   │   └── test-taking.service.ts
│   │   │   │   ├── models/
│   │   │   │   │   ├── test.model.ts
│   │   │   │   │   ├── question.model.ts
│   │   │   │   │   └── result.model.ts
│   │   │   │   └── student.routes.ts
│   │   │   │
│   │   │   ├── teacher/                    # Модуль преподавателя
│   │   │   │   ├── pages/
│   │   │   │   │   ├── dashboard/
│   │   │   │   │   ├── question-banks/
│   │   │   │   │   ├── questions/
│   │   │   │   │   ├── tests/
│   │   │   │   │   ├── test-results/
│   │   │   │   │   ├── groups/
│   │   │   │   │   └── analytics/
│   │   │   │   ├── components/
│   │   │   │   │   ├── question-editor/
│   │   │   │   │   ├── test-builder/
│   │   │   │   │   ├── student-list/
│   │   │   │   │   └── results-table/
│   │   │   │   ├── services/
│   │   │   │   │   └── teacher-api.service.ts
│   │   │   │   ├── models/
│   │   │   │   └── teacher.routes.ts
│   │   │   │
│   │   │   └── admin/                      # Модуль администратора
│   │   │       ├── pages/
│   │   │       │   ├── dashboard/
│   │   │       │   ├── users/
│   │   │       │   ├── groups/
│   │   │       │   ├── system/
│   │   │       │   └── logs/
│   │   │       ├── components/
│   │   │       ├── services/
│   │   │       └── admin.routes.ts
│   │   │
│   │   ├── shared/                         # Общие компоненты
│   │   │   ├── components/
│   │   │   │   ├── button/
│   │   │   │   │   ├── button.component.ts
│   │   │   │   │   ├── button.component.html
│   │   │   │   │   └── button.component.scss
│   │   │   │   ├── input/
│   │   │   │   ├── select/
│   │   │   │   ├── checkbox/
│   │   │   │   ├── radio/
│   │   │   │   ├── textarea/
│   │   │   │   ├── modal/
│   │   │   │   ├── toast/
│   │   │   │   ├── loading/
│   │   │   │   ├── pagination/
│   │   │   │   ├── table/
│   │   │   │   ├── card/
│   │   │   │   ├── badge/
│   │   │   │   ├── avatar/
│   │   │   │   ├── dropdown/
│   │   │   │   ├── tooltip/
│   │   │   │   ├── tabs/
│   │   │   │   ├── accordion/
│   │   │   │   ├── file-upload/
│   │   │   │   ├── date-picker/
│   │   │   │   ├── search-box/
│   │   │   │   └── empty-state/
│   │   │   ├── directives/
│   │   │   │   ├── autofocus.directive.ts
│   │   │   │   ├── debounce.directive.ts
│   │   │   │   ├── click-outside.directive.ts
│   │   │   │   └── permission.directive.ts
│   │   │   ├── pipes/
│   │   │   │   ├── truncate.pipe.ts
│   │   │   │   ├── date-format.pipe.ts
│   │   │   │   ├── safe-html.pipe.ts
│   │   │   │   └── duration.pipe.ts
│   │   │   └── shared.module.ts
│   │   │
│   │   ├── layout/                         # Layout-компоненты
│   │   │   ├── main-layout/
│   │   │   │   ├── main-layout.component.ts
│   │   │   │   ├── main-layout.component.html
│   │   │   │   └── main-layout.component.scss
│   │   │   ├── auth-layout/
│   │   │   ├── header/
│   │   │   │   ├── header.component.ts
│   │   │   │   ├── header.component.html
│   │   │   │   └── header.component.scss
│   │   │   ├── sidebar/
│   │   │   │   ├── sidebar.component.ts
│   │   │   │   ├── sidebar.component.html
│   │   │   │   └── sidebar.component.scss
│   │   │   └── footer/
│   │   │
│   │   ├── app.component.ts                # Корневой компонент
│   │   ├── app.component.html
│   │   ├── app.component.scss
│   │   ├── app.routes.ts                   # Маршрутизация
│   │   └── app.config.ts                   # Конфигурация приложения
│   │
│   ├── assets/
│   │   ├── images/
│   │   │   ├── logo.svg
│   │   │   ├── logo-dark.svg
│   │   │   └── icons/
│   │   ├── fonts/
│   │   └── i18n/                           # Интернационализация
│   │       ├── en.json
│   │       └── ru.json
│   │
│   ├── environments/
│   │   ├── environment.ts                  # Development
│   │   └── environment.prod.ts             # Production
│   │
│   ├── styles/
│   │   ├── _variables.scss                 # CSS-переменные
│   │   ├── _mixins.scss                    # SCSS-миксины
│   │   ├── _functions.scss                 # SCSS-функции
│   │   ├── _utilities.scss                 # Утилитарные классы
│   │   ├── _reset.scss                     # CSS-сброс
│   │   ├── _typography.scss                # Типографика
│   │   ├── _animations.scss                # Анимации
│   │   ├── _grid.scss                      # Сетка
│   │   └── styles.scss                     # Главный файл стилей
│   │
│   ├── index.html
│   ├── main.ts                             # Точка входа
│   └── styles.scss                         # Глобальные стили
│
├── angular.json                            # Конфигурация Angular CLI
├── package.json
├── tsconfig.json
├── tsconfig.app.json
├── tsconfig.spec.json
└── .eslintrc.json
```

## Архитектурные слои

### Слой Core (Ядро)

Содержит singleton-сервисы, guards, interceptors и базовые модели.

```
core/
├── services/           # Singleton-сервисы
│   ├── auth.service.ts
│   ├── token.service.ts
│   └── api.service.ts
├── interceptors/       # HTTP-интерцепторы
│   ├── auth.interceptor.ts
│   └── error.interceptor.ts
├── guards/             # Защита маршрутов
│   ├── auth.guard.ts
│   └── role.guard.ts
└── models/             # Базовые модели
    └── api-response.model.ts
```

### Слой Features (Функциональные модули)

Каждый feature-модуль содержит страницы, компоненты, сервисы и модели.

```
features/
├── auth/               # Аутентификация
├── student/            # Панель студента
├── teacher/            # Панель преподавателя
└── admin/              # Панель администратора
```

### Слой Shared (Общие компоненты)

Переиспользуемые компоненты, директивы и pipes.

```
shared/
├── components/         # UI-компоненты
├── directives/         # Директивы
└── pipes/              # Pipes
```

### Слой Layout (Макеты)

Компоненты для отображения структуры страниц.

```
layout/
├── main-layout/        # Основной layout с sidebar
├── auth-layout/        # Layout для страниц аутентификации
├── header/             # Шапка приложения
├── sidebar/            # Боковое меню
└── footer/             # Подвал
```

## Маршрутизация

### Структура маршрутов

```typescript
// app.routes.ts
export const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayout,
    canActivate: [guestGuard],
    children: [
      { path: 'login', component: LoginPage },
      { path: 'register', component: RegisterPage },
      { path: 'forgot-password', component: ForgotPasswordPage },
      { path: 'reset-password', component: ResetPasswordPage }
    ]
  },
  {
    path: 'student',
    component: MainLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Student'] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: StudentDashboardPage },
      { path: 'tests', component: TestListPage },
      { path: 'tests/:id', component: TestTakingPage },
      { path: 'tests/:id/results', component: TestResultsPage },
      { path: 'profile', component: ProfilePage }
    ]
  },
  {
    path: 'teacher',
    component: MainLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Teacher', 'Admin'] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: TeacherDashboardPage },
      { path: 'questions', component: QuestionsPage },
      { path: 'question-banks', component: QuestionBanksPage },
      { path: 'tests', component: TestsManagementPage },
      { path: 'tests/:id', component: TestEditPage },
      { path: 'results', component: ResultsPage },
      { path: 'groups', component: GroupsPage },
      { path: 'analytics', component: AnalyticsPage }
    ]
  },
  {
    path: 'admin',
    component: MainLayout,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardPage },
      { path: 'users', component: UsersPage },
      { path: 'groups', component: AdminGroupsPage },
      { path: 'system', component: SystemPage },
      { path: 'logs', component: LogsPage }
    ]
  },
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },
  { path: '**', component: NotFoundPage }
];
```

## Управление состоянием

### Использование Signals

```typescript
// Пример использования signals в сервисе
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _currentUser = signal<User | null>(null);
  private readonly _isAuthenticated = signal(false);
  
  readonly currentUser = this._currentUser.asReadonly();
  readonly isAuthenticated = this._isAuthenticated.asReadonly();
  
  constructor(private readonly http: HttpClient) {
    this.checkAuthState();
  }
  
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<ApiResponse<LoginResponse>>('/api/v1/auth/login', credentials)
      .pipe(
        tap(response => {
          this._currentUser.set(response.data.user);
          this._isAuthenticated.set(true);
          this.tokenService.setTokens(response.data.accessToken, response.data.refreshToken);
        })
      );
  }
  
  logout(): void {
    this._currentUser.set(null);
    this._isAuthenticated.set(false);
    this.tokenService.clearTokens();
  }
}
```

### Использование в компонентах

```typescript
@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  templateUrl: './dashboard.page.html',
  styleUrls: ['./dashboard.page.scss']
})
export class StudentDashboardPage implements OnInit {
  private readonly studentService = inject(StudentApiService);
  
  readonly profile = this.studentService.profile;
  readonly availableTests = this.studentService.availableTests;
  readonly isLoading = this.studentService.isLoading;
  
  ngOnInit(): void {
    this.studentService.loadProfile();
    this.studentService.loadAvailableTests();
  }
}
```

## HTTP-клиент

### Базовый API-сервис

```typescript
@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly baseUrl = environment.apiUrl;
  
  constructor(private readonly http: HttpClient) {}
  
  get<T>(endpoint: string, params?: HttpParams): Observable<T> {
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}${endpoint}`, { params })
      .pipe(map(response => response.data));
  }
  
  post<T>(endpoint: string, body: unknown): Observable<T> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}${endpoint}`, body)
      .pipe(map(response => response.data));
  }
  
  put<T>(endpoint: string, body: unknown): Observable<T> {
    return this.http.put<ApiResponse<T>>(`${this.baseUrl}${endpoint}`, body)
      .pipe(map(response => response.data));
  }
  
  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<ApiResponse<T>>(`${this.baseUrl}${endpoint}`)
      .pipe(map(response => response.data));
  }
}
```

## Интерцепторы

### Auth Interceptor

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const token = tokenService.accessToken;
  
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  
  return next(req);
};
```

### Error Interceptor

```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const tokenService = inject(TokenService);
  const errorHandlingService = inject(ErrorHandlingService);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        tokenService.clearTokens();
        router.navigate(['/auth/login']);
      } else if (error.status === 403) {
        router.navigate(['/forbidden']);
      } else {
        errorHandlingService.handleError(error);
      }
      
      return throwError(() => error);
    })
  );
};
```

## Guards

### Auth Guard

```typescript
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  if (authService.isAuthenticated()) {
    return true;
  }
  
  router.navigate(['/auth/login']);
  return false;
};
```

### Role Guard

```typescript
export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const requiredRoles = route.data['roles'] as string[];
  
  const user = authService.currentUser();
  
  if (user && requiredRoles.includes(user.role)) {
    return true;
  }
  
  router.navigate(['/forbidden']);
  return false;
};
```

## Стратегия обнаружения изменений

Все компоненты используют `ChangeDetectionStrategy.OnPush` для оптимизации производительности:

```typescript
@Component({
  selector: 'app-test-card',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './test-card.component.html',
  styleUrls: ['./test-card.component.scss']
})
export class TestCardComponent {
  @Input() test!: TestDto;
}
```

## Ленивая загрузка

Все feature-модули загружаются лениво:

```typescript
{
  path: 'student',
  loadComponent: () => import('./features/student/student.routes')
    .then(m => m.STUDENT_ROUTES)
}
```

## Поток данных

```
User Action
    ↓
Component (Signal/Event)
    ↓
Service (HttpClient)
    ↓
Interceptor (Auth/Error)
    ↓
BffPortalService API
    ↓
Response
    ↓
Service (Signal Update)
    ↓
Component (Signal Read)
    ↓
UI Update
```
