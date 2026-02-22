# Angular 21 UI - Правила кодирования

## Общие принципы

1. **Angular 21 Standalone Components** - Все компоненты должны быть standalone
2. **Signals** - Использовать signals для реактивного состояния
3. **OnPush** - Все компоненты используют стратегию обнаружения изменений OnPush
4. **Strict TypeScript** - Строгая типизация, никаких `any`
5. **SCSS** - Использование SCSS для стилизации

---

## Структура компонента

### Файл компонента (.ts)

```typescript
// 1. Imports - сторонние библиотеки
import { 
  Component, 
  Input, 
  Output, 
  EventEmitter,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  signal,
  computed,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// 2. Imports - локальные модули
import { ButtonComponent } from '@shared/components/button/button.component';
import { InputComponent } from '@shared/components/input/input.component';

// 3. Интерфейсы/типы
import type { User } from '@core/models/user.model';

@Component({
  // 4. Декоратор компонента
  selector: 'app-user-card',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    FormsModule,
    ButtonComponent,
    InputComponent
  ],
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent {
  // 5. Inputs
  @Input({ required: true }) user!: User;
  @Input() editable = false;

  // 6. Outputs
  @Output() edit = new EventEmitter<User>();
  @Output() delete = new EventEmitter<string>();

  // 7. Inject сервисов
  private readonly cdr = inject(ChangeDetectorRef);

  // 8. Signals
  private readonly _isExpanded = signal(false);
  
  // 9. Computed значения
  readonly isExpanded = this._isExpanded.asReadonly();
  readonly userFullName = computed(() => `${this.user.firstName} ${this.user.lastName}`);
  
  // 10. Публичные методы
  toggleExpanded(): void {
    this._isExpanded.update(v => !v);
  }

  onEdit(): void {
    this.edit.emit(this.user);
  }

  onDelete(): void {
    this.delete.emit(this.user.id);
  }

  // 11. Приватные методы
  private formatDate(date: Date): string {
    return date.toLocaleDateString('ru-RU');
  }
}
```

### Файл шаблона (.html)

```html
<!-- Использование control flow синтаксиса (@if, @for, @switch) -->
<div class="user-card" [class.expanded]="isExpanded()">
  @if (user) {
    <div class="card-header">
      <app-avatar 
        [url]="user.avatarUrl" 
        [name]="userFullName()"
        size="medium">
      </app-avatar>
      
      <div class="user-info">
        <h3>{{ userFullName() }}</h3>
        <p class="email">{{ user.email }}</p>
      </div>
      
      <app-button 
        variant="ghost" 
        size="small"
        (click)="toggleExpanded()">
        {{ isExpanded() ? 'Свернуть' : 'Подробнее' }}
      </app-button>
    </div>
    
    @if (isExpanded()) {
      <div class="card-body">
        <div class="detail-row">
          <span class="label">Роль:</span>
          <app-badge [type]="getRoleBadgeType(user.role)">
            {{ user.role }}
          </app-badge>
        </div>
        
        <div class="detail-row">
          <span class="label">Дата регистрации:</span>
          <span>{{ formatDate(user.createdAt) }}</span>
        </div>
        
        @if (editable) {
          <div class="card-actions">
            <app-button variant="secondary" (click)="onEdit()">
              Редактировать
            </app-button>
            <app-button variant="danger" (click)="onDelete()">
              Удалить
            </app-button>
          </div>
        }
      </div>
    }
  } @else {
    <app-loading type="skeleton" [lines]="3"></app-loading>
  }
</div>
```

### Файл стилей (.scss)

```scss
@use '@styles/variables' as *;
@use '@styles/mixins' as *;

:host {
  display: block;
}

.user-card {
  background: $white;
  border-radius: $border-radius-lg;
  box-shadow: $shadow;
  overflow: hidden;
  transition: box-shadow 0.2s ease;

  &:hover {
    box-shadow: $shadow-md;
  }

  &.expanded {
    .card-body {
      display: block;
    }
  }
}

.card-header {
  @include flex-between;
  padding: $spacing-md;
  border-bottom: 1px solid $gray-200;
}

.user-info {
  flex: 1;
  margin-left: $spacing-md;

  h3 {
    margin: 0;
    font-size: $font-size-lg;
    font-weight: $font-weight-semibold;
  }

  .email {
    margin: $spacing-xs 0 0;
    color: $gray-500;
    font-size: $font-size-sm;
  }
}

.card-body {
  display: none;
  padding: $spacing-md;
}

.detail-row {
  @include flex-between;
  padding: $spacing-sm 0;
  border-bottom: 1px solid $gray-100;

  .label {
    color: $gray-500;
    font-size: $font-size-sm;
  }
}

.card-actions {
  @include flex-between;
  margin-top: $spacing-md;
  padding-top: $spacing-md;
  border-top: 1px solid $gray-200;
}
```

---

## Именование

### Файлы

| Тип | Пример | Правило |
|-----|--------|---------|
| Компонент | `user-card.component.ts` | kebab-case с суффиксом `.component` |
| Сервис | `auth.service.ts` | kebab-case с суффиксом `.service` |
| Guard | `auth.guard.ts` | kebab-case с суффиксом `.guard` |
| Pipe | `date-format.pipe.ts` | kebab-case с суффиксом `.pipe` |
| Directive | `autofocus.directive.ts` | kebab-case с суффиксом `.directive` |
| Модель | `user.model.ts` | kebab-case с суффиксом `.model` |
| DTO | `login-request.dto.ts` | kebab-case с суффиксом `.dto` |
| Страница | `dashboard.page.ts` | kebab-case с суффиксом `.page` |

### Классы

```typescript
// Компоненты
export class UserCardComponent { }
export class LoginPage { }

// Сервисы
export class AuthService { }
export class ApiService { }

// Guards
export const authGuard: CanActivateFn = () => {};

// Pipes
export class DateFormatPipe implements PipeTransform { }

// Directives
export class AutofocusDirective implements OnInit { }
```

### Переменные и сигналы

```typescript
// Приватные сигналы
private readonly _users = signal<User[]>([]);
private readonly _isLoading = signal(false);

// Публичные сигналы (readonly)
readonly users = this._users.asReadonly();
readonly isLoading = this._isLoading.asReadonly();

// Computed
readonly userCount = computed(() => this.users().length);

// Константы
const MAX_FILE_SIZE = 5 * 1024 * 1024;
const ALLOWED_FILE_TYPES = ['image/jpeg', 'image/png', 'image/gif'];

// Enum
enum UserRole {
  Student = 'Student',
  Teacher = 'Teacher',
  Admin = 'Admin'
}

// Interface
interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
}
```

---

## TypeScript правила

### Типизация

```typescript
// ❌ Плохо
const data: any = response;
function parse(value: any): any { }

// ✅ Хорошо
interface UserResponse {
  id: string;
  name: string;
}

const data: UserResponse = response;
function parse(value: string): UserResponse | null { }
```

### Optional chaining и nullish

```typescript
// ❌ Плохо
const name = user && user.profile && user.profile.name;

// ✅ Хорошо
const name = user?.profile?.name;

// Nullish coalescing
const displayName = user?.name ?? 'Гость';
```

### Generic constraints

```typescript
// ❌ Плохо
function getItem(items: any[], id: any): any {
  return items.find(item => item.id === id);
}

// ✅ Хорошо
interface Identifiable {
  id: string;
}

function getItem<T extends Identifiable>(items: T[], id: string): T | undefined {
  return items.find(item => item.id === id);
}
```

---

## Angular специфичные правила

### Input/Output

```typescript
// ❌ Плохо - без типа
@Input() name: string;
@Output() save = new EventEmitter();

// ✅ Хорошо - с явным указанием
@Input({ required: true }) name!: string;
@Output() save = new EventEmitter<void>();
```

### Signals vs RxJS

```typescript
// Используйте signals для синхронного состояния
private readonly _count = signal(0);
readonly count = this._count.asReadonly();

// Используйте RxJS для асинхронных потоков
readonly user$ = this.http.get<User>('/api/user');
```

### Dependency Injection

```typescript
// ❌ Плохо - конструктор
constructor(
  private authService: AuthService,
  private router: Router,
  private cdr: ChangeDetectorRef
) {}

// ✅ Хорошо - inject функция
private readonly authService = inject(AuthService);
private readonly router = inject(Router);
private readonly cdr = inject(ChangeDetectorRef);
```

### Control Flow синтаксис

```html
<!-- ❌ Плохо - *ngIf, *ngFor -->
<div *ngIf="users.length > 0">
  <div *ngFor="let user of users">{{ user.name }}</div>
</div>

<!-- ✅ Хорошо - @if, @for -->
@if (users.length > 0) {
  @for (user of users; track user.id) {
    <div>{{ user.name }}</div>
  }
}
```

---

## SCSS правила

### Организация стилей

```scss
// 1. External (bootstrap, etc.)
@use 'bootstrap/scss/functions';

// 2. Variables
@use 'variables' as *;

// 3. Mixins
@use 'mixins' as *;

// 4. Base
@use 'reset';

// 5. Components
.component { }

// 6. Utilities
@use 'utilities';
```

### BEM в SCSS

```scss
// Блок
.card { }

// Элемент
.card__header { }
.card__body { }
.card__footer { }

// Модификатор
.card--featured { }
.card__header--compact { }

// Вложенность (с осторожностью)
.card {
  &__header {
    // ...
  }
  
  &--featured {
    .card__body {
      // ...
    }
  }
}
```

### Переменные

```scss
// ❌ Плохо - хардкод
.element {
  color: #2563eb;
  padding: 16px;
}

// ✅ Хорошо - переменные
.element {
  color: $primary;
  padding: $spacing-md;
}
```

---

## Тестирование

### Unit тесты компонентов

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserCardComponent } from './user-card.component';
import { ButtonComponent } from '@shared/components/button/button.component';
import { AvatarComponent } from '@shared/components/avatar/avatar.component';

describe('UserCardComponent', () => {
  let component: UserCardComponent;
  let fixture: ComponentFixture<UserCardComponent>;

  const mockUser: User = {
    id: '1',
    firstName: 'Иван',
    lastName: 'Иванов',
    email: 'ivan@example.com',
    role: UserRole.Student
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UserCardComponent,
        ButtonComponent,
        AvatarComponent
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserCardComponent);
    component = fixture.componentInstance;
    component.user = mockUser;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display user full name', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Иван Иванов');
  });

  it('should emit edit event when edit button clicked', () => {
    jest.spyOn(component.edit, 'emit');
    
    const button = fixture.nativeElement.querySelector('app-button');
    button.click();
    
    expect(component.edit.emit).toHaveBeenCalledWith(mockUser);
  });
});
```

---

## Git workflow

### Сообщения коммитов

```
feat: добавить страницу профиля студента
fix: исправить валидацию формы входа
refactor: вынести общие компоненты в shared
style: исправить отступы в карточке теста
docs: обновить документацию API
test: добавить тесты для компонента вопроса
chore: обновить зависимости
```

### Branch naming

```
feature/user-profile-page
bugfix/login-validation
refactor/auth-service
hotfix/security-patch
```
