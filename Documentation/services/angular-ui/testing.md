# Angular 21 UI - Тестирование

## Обзор тестирования

Приложение использует следующие инструменты для тестирования:

| Инструмент | Назначение |
|------------|------------|
| Jest | Test runner |
| Angular Testing Library | Компонентное тестирование |
| Cypress | E2E тестирование |
| ng-mocks | Mocking библиотека |

---

## Unit тестирование

### Настройка Jest

**jest.config.js:**
```javascript
module.exports = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
  testPathIgnorePatterns: ['<rootDir>/node_modules/', '<rootDir>/dist/'],
  coverageDirectory: 'coverage',
  collectCoverageFrom: [
    'src/**/*.ts',
    '!src/**/*.module.ts',
    '!src/main.ts',
    '!src/**/*.d.ts'
  ],
  moduleNameMapper: {
    '^@core/(.*)$': '<rootDir>/src/app/core/$1',
    '^@features/(.*)$': '<rootDir>/src/app/features/$1',
    '^@shared/(.*)$': '<rootDir>/src/app/shared/$1',
    '^@layout/(.*)$': '<rootDir>/src/app/layout/$1',
    '^@env/(.*)$': '<rootDir>/src/environments/$1'
  }
};
```

**setup-jest.ts:**
```typescript
import 'jest-preset-angular/setup-jest';

// Mock global objects
Object.defineProperty(window, 'CSS', { value: null });
Object.defineProperty(window, 'getComputedStyle', {
  value: () => ({
    display: 'none',
    appearance: ['-webkit-appearance']
  })
});

Object.defineProperty(document, 'doctype', {
  value: '<!DOCTYPE html>'
});

Object.defineProperty(document.body.style, 'transform', {
  value: () => ({
    enumerable: true,
    configurable: true
  })
});
```

---

### Тестирование компонентов

#### Простой компонент

```typescript
// user-avatar.component.ts
@Component({
  selector: 'app-user-avatar',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="avatar" [class.circle]="circle">
      <img *ngIf="url" [src]="url" [alt]="name">
      <span *ngIf="!url">{{ initials }}</span>
    </div>
  `,
  styleUrls: ['./user-avatar.component.scss']
})
export class UserAvatarComponent {
  @Input() url?: string;
  @Input() name?: string;
  @Input() circle = false;

  get initials(): string {
    if (!this.name) return '';
    const parts = this.name.split(' ');
    return parts.map(p => p[0]).join('').toUpperCase().slice(0, 2);
  }
}

// user-avatar.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserAvatarComponent } from './user-avatar.component';

describe('UserAvatarComponent', () => {
  let component: UserAvatarComponent;
  let fixture: ComponentFixture<UserAvatarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserAvatarComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(UserAvatarComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display initials when no url', () => {
    component.name = 'Иван Иванов';
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;
    expect(element.textContent).toContain('ИИ');
  });

  it('should display image when url provided', () => {
    component.name = 'Иван Иванов';
    component.url = 'https://example.com/avatar.jpg';
    fixture.detectChanges();

    const img = fixture.nativeElement.querySelector('img');
    expect(img).toBeTruthy();
    expect(img.src).toBe('https://example.com/avatar.jpg');
  });

  it('should add circle class when circle input is true', () => {
    component.name = 'Тест';
    component.circle = true;
    fixture.detectChanges();

    const element = fixture.nativeElement.querySelector('.avatar');
    expect(element.classList).toContain('circle');
  });
});
```

#### Компонент с формами

```typescript
// login-form.component.ts
@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <input formControlName="email" type="email">
      <input formControlName="password" type="password">
      <button type="submit" [disabled]="form.invalid">Войти</button>
    </form>
  `
})
export class LoginFormComponent {
  @Output() submitForm = new EventEmitter<{ email: string; password: string }>();

  form = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)])
  });

  onSubmit(): void {
    if (this.form.valid) {
      this.submitForm.emit(this.form.getRawValue());
    }
  }
}

// login-form.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginFormComponent } from './login-form.component';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let fixture: ComponentFixture<LoginFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginFormComponent, ReactiveFormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create form with empty values', () => {
    expect(component.form.get('email')?.value).toBe('');
    expect(component.form.get('password')?.value).toBe('');
  });

  it('should emit submitForm when form is valid', () => {
    jest.spyOn(component.submitForm, 'emit');

    component.form.patchValue({
      email: 'test@example.com',
      password: 'password123'
    });
    fixture.detectChanges();

    const form = fixture.nativeElement.querySelector('form');
    form.dispatchEvent(new Event('submit'));

    expect(component.submitForm.emit).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'password123'
    });
  });

  it('should not emit when form is invalid', () => {
    jest.spyOn(component.submitForm, 'emit');

    component.form.patchValue({
      email: 'invalid',
      password: 'short'
    });
    fixture.detectChanges();

    const form = fixture.nativeElement.querySelector('form');
    form.dispatchEvent(new Event('submit'));

    expect(component.submitForm.emit).not.toHaveBeenCalled();
  });

  it('should show validation errors', () => {
    component.form.get('email')?.markAsTouched();
    fixture.detectChanges();

    // Проверка отображения ошибок валидации
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.error')).toBeTruthy();
  });
});
```

---

### Тестирование сервисов

```typescript
// auth.service.ts
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _currentUser = signal<User | null>(null);
  readonly currentUser = this._currentUser.asReadonly();

  constructor(private readonly http: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<ApiResponse<LoginResponse>>('/api/v1/auth/login', credentials)
      .pipe(
        map(response => {
          this._currentUser.set(response.data.user);
          return response.data;
        })
      );
  }

  logout(): void {
    this._currentUser.set(null);
  }
}

// auth.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set currentUser on successful login', () => {
    const mockResponse: ApiResponse<LoginResponse> = {
      isError: false,
      timestamp: '',
      message: '',
      data: {
        accessToken: 'token',
        refreshToken: 'refresh',
        expiresIn: 3600,
        user: {
          id: '1',
          email: 'test@test.com',
          firstName: 'Test',
          lastName: 'User',
          role: 'Student',
          emailVerified: true
        }
      }
    };

    service.login({ email: 'test@test.com', password: 'password' }).subscribe();

    const req = httpMock.expectOne('/api/v1/auth/login');
    req.flush(mockResponse);

    expect(service.currentUser()).toEqual(mockResponse.data.user);
  });

  it('should clear currentUser on logout', () => {
    service.logout();
    expect(service.currentUser()).toBeNull();
  });
});
```

---

### Тестирование guards

```typescript
// auth.guard.ts
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  return router.createUrlTree(['/auth/login']);
};

// auth.guard.spec.ts
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { authGuard } from './auth.guard';

describe('authGuard', () => {
  let mockAuthService: { isAuthenticated: jest.Mock };
  let mockRouter: { createUrlTree: jest.Mock };

  beforeEach(() => {
    mockAuthService = {
      isAuthenticated: jest.fn()
    };

    mockRouter = {
      createUrlTree: jest.fn().mockReturnValue({})
    };

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    });
  });

  it('should return true when user is authenticated', () => {
    mockAuthService.isAuthenticated.mockReturnValue(true);

    const result = authGuard({} as any, {} as any);

    expect(result).toBe(true);
  });

  it('should return url tree when user is not authenticated', () => {
    mockAuthService.isAuthenticated.mockReturnValue(false);

    const result = authGuard({} as any, {} as any);

    expect(mockRouter.createUrlTree).toHaveBeenCalledWith(['/auth/login']);
    expect(result).toEqual({});
  });
});
```

---

## E2E тестирование

### Cypress настройка

**cypress.config.ts:**
```typescript
import { defineConfig } from 'cypress';

export default defineConfig({
  e2e: {
    baseUrl: 'http://localhost:4200',
    supportFile: 'cypress/support/e2e.ts',
    specPattern: 'cypress/e2e/**/*.cy.ts',
    viewportWidth: 1280,
    viewportHeight: 720,
    video: false,
    screenshotOnRunFailure: true,
    defaultCommandTimeout: 10000,
    retries: {
      runMode: 2,
      openMode: 0
    }
  },
  component: {
    devServer: {
      framework: 'angular',
      bundler: 'webpack'
    },
    specPattern: 'cypress/component/**/*.cy.ts'
  }
});
```

---

### E2E тесты

#### Тест авторизации

```typescript
// cypress/e2e/auth/login.cy.ts
describe('Login', () => {
  beforeEach(() => {
    cy.visit('/auth/login');
  });

  it('should display login form', () => {
    cy.get('form').should('be.visible');
    cy.get('input[type="email"]').should('be.visible');
    cy.get('input[type="password"]').should('be.visible');
    cy.get('button[type="submit"]').should('be.visible');
  });

  it('should show validation errors for empty form', () => {
    cy.get('button[type="submit"]').click();
    cy.contains('Email обязателен').should('be.visible');
    cy.contains('Пароль обязателен').should('be.visible');
  });

  it('should login successfully with valid credentials', () => {
    cy.intercept('POST', '**/auth/login', {
      statusCode: 200,
      body: {
        isError: false,
        data: {
          accessToken: 'mock-token',
          refreshToken: 'mock-refresh',
          user: {
            id: '1',
            email: 'student@test.com',
            firstName: 'Student',
            lastName: 'Test',
            role: 'Student'
          }
        }
      }
    }).as('loginRequest');

    cy.get('input[type="email"]').type('student@test.com');
    cy.get('input[type="password"]').type('password123');
    cy.get('button[type="submit"]').click();

    cy.url().should('include', '/student/dashboard');
  });

  it('should show error for invalid credentials', () => {
    cy.intercept('POST', '**/auth/login', {
      statusCode: 400,
      body: {
        isError: true,
        detail: 'Invalid credentials'
      }
    }).as('loginRequest');

    cy.get('input[type="email"]').type('wrong@test.com');
    cy.get('input[type="password"]').type('wrongpassword');
    cy.get('button[type="submit"]').click();

    cy.contains('Неверный email или пароль').should('be.visible');
  });
});
```

#### Тест прохождения теста

```typescript
// cypress/e2e/student/take-test.cy.ts
describe('Take Test', () => {
  beforeEach(() => {
    cy.login('student@test.com', 'password123');
    cy.visit('/student/tests');
  });

  it('should display available tests', () => {
    cy.get('app-test-card').should('have.length.greaterThan', 0);
  });

  it('should start test when clicking start button', () => {
    cy.intercept('POST', '**/tests/*/start', {
      statusCode: 200,
      body: {
        isError: false,
        data: {
          testId: 'test-1',
          status: 'InProgress'
        }
      }
    }).as('startTest');

    cy.get('app-test-card').first().find('button').click();

    cy.url().should('include', '/student/tests/test-1');
  });

  it('should answer questions and submit', () => {
    cy.intercept('GET', '**/tests/*/questions', {
      statusCode: 200,
      body: {
        isError: false,
        data: {
          testId: 'test-1',
          testTitle: 'Test',
          questions: [
            { questionId: 'q1', text: 'Question 1?', options: [] }
          ]
        }
      }
    });

    cy.get('.question-text').should('contain', 'Question 1?');
    cy.get('button:contains("Завершить тест")').click();
    
    cy.contains('Подтверждение').should('be.visible');
    cy.get('button:contains("Да")').click();

    cy.url().should('include', '/results');
  });
});
```

---

### Component тесты в Cypress

```typescript
// cypress/component/button.cy.ts
import ButtonComponent from '../../src/app/shared/components/button/button.component';

describe('ButtonComponent', () => {
  beforeEach(() => {
    cy.mount(ButtonComponent, {
      componentProperties: {
        variant: 'primary',
        loading: false,
        disabled: false
      }
    });
  });

  it('should render button with text', () => {
    cy.get('button').should('contain.text', 'Click me');
  });

  it('should show loading state', () => {
    cy.mount(ButtonComponent, {
      componentProperties: {
        loading: true
      }
    });
    
    cy.get('.spinner').should('be.visible');
    cy.get('button').should('be.disabled');
  });

  it('should emit click event', () => {
    const onClick = cy.stub().as('onClick');
    cy.mount(ButtonComponent, {
      componentProperties: {
        clicked: onClick
      }
    });
    
    cy.get('button').click();
    cy.get('@onClick').should('have.been.called');
  });
});
```

---

## Запуск тестов

### Unit тесты (Jest)

```bash
# Все тесты
npm test

# С покрытием
npm test -- --coverage

# С watcher
npm test -- --watch

# Для конкретного файла
npm test -- --testPathPattern="auth.service"
```

### E2E тесты (Cypress)

```bash
# Открыть Cypress UI
npm run e2e

# Запустить тесты headless
npm run e2e:headless

# Запустить конкретный тест
npm run e2e -- --spec "cypress/e2e/auth/login.cy.ts"
```

---

## Mock данных

### Mock для HTTP

```typescript
// helpers/mock-http.ts
export const mockApiResponse = <T>(data: T): ApiResponse<T> => ({
  isError: false,
  timestamp: new Date().toISOString(),
  message: 'Success',
  data
});

export const mockUser: User = {
  id: '1',
  email: 'test@test.com',
  firstName: 'Test',
  lastName: 'User',
  role: 'Student',
  emailVerified: true
};

export const mockTest: AvailableTestDto = {
  testId: 'test-1',
  testTitle: 'Sample Test',
  description: 'Test description',
  durationMinutes: 60,
  passingScore: 70,
  maxAttempts: 3,
  attemptsUsed: 0,
  status: 'Available',
  availableFrom: new Date().toISOString(),
  availableUntil: new Date(Date.now() + 86400000).toISOString(),
  questionsCount: 10,
  canTake: true
};
```

---

## Best Practices

1. **Тестируйте поведение, не реализацию**
2. **Используйте Test Bed минимально** - предпочитайте изолированные тесты
3. **Давайте понятные имена тестам** - `should display error when form is invalid`
4. **Тестируйте edge cases** - пустые значения, граничные значения
5. **Используйте моки для внешних зависимостей** - HTTP, router
6. **Поддерживайте тесты** - обновляйте при изменении требований
7. **Стремитесь к 80% покрытию** для бизнес-логики
