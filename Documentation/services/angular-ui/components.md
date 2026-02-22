# Angular 21 UI - Компоненты

## Обзор

Документация описывает все переиспользуемые UI-компоненты приложения. Каждый компонент является standalone и использует стратегию обнаружения изменений `OnPush`.

---

## Базовые компоненты (Shared Components)

### Button

**Путь:** `src/app/shared/components/button/`

**Назначение:** Кнопка с различными вариантами отображения и состояниями загрузки.

**Входные параметры:**
```typescript
interface ButtonProps {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger';
  size?: 'small' | 'medium' | 'large';
  disabled?: boolean;
  loading?: boolean;
  type?: 'button' | 'submit' | 'reset';
  fullWidth?: boolean;
  icon?: string;
  iconPosition?: 'left' | 'right';
}
```

**Использование:**
```html
<app-button 
  variant="primary" 
  size="medium"
  [loading]="isLoading()"
  [disabled]="isDisabled"
  (click)="handleClick()">
  Сохранить
</app-button>

<app-button variant="outline" icon="plus" iconPosition="left">
  Добавить
</app-button>
```

**Пример реализации:**
```typescript
@Component({
  selector: 'app-button',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() variant: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger' = 'primary';
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() fullWidth = false;
  @Input() icon?: string;
  @Input() iconPosition: 'left' | 'right' = 'left';

  @Output() clicked = new EventEmitter<void>();

  get classes(): Record<string, boolean> {
    return {
      [`btn-${this.variant}`]: true,
      [`btn-${this.size}`]: true,
      'btn-loading': this.loading,
      'btn-full': this.fullWidth
    };
  }

  onClick(event: MouseEvent): void {
    if (!this.disabled && !this.loading) {
      this.clicked.emit();
    }
  }
}
```

---

### Input

**Путь:** `src/app/shared/components/input/`

**Назначение:** Поле ввода с поддержкой валидации, иконок и типов.

**Входные параметры:**
```typescript
interface InputProps {
  label?: string;
  placeholder?: string;
  type?: 'text' | 'email' | 'password' | 'number' | 'tel' | 'url';
  value?: string;
  disabled?: boolean;
  readonly?: boolean;
  errorMessages?: Record<string, string>;
  hint?: string;
  prefixIcon?: string;
  suffixIcon?: string;
  maxLength?: number;
  minLength?: number;
}
```

**Использование:**
```html
<app-input
  formControlName="email"
  label="Email"
  type="email"
  placeholder="Введите email"
  prefixIcon="email"
  [errorMessages]="{
    required: 'Email обязателен',
    email: 'Некорректный email'
  }">
</app-input>

<app-input
  formControlName="password"
  label="Пароль"
  type="password"
  placeholder="Введите пароль"
  suffixIcon="visibility"
  [errorMessages]="{ required: 'Пароль обязателен' }">
</app-input>
```

---

### Select

**Путь:** `src/app/shared/components/select/`

**Назначение:** Выпадающий список с поддержкой поиска и множественного выбора.

**Входные параметры:**
```typescript
interface SelectProps {
  label?: string;
  placeholder?: string;
  options: SelectOption[];
  value?: string | string[];
  disabled?: boolean;
  multiple?: boolean;
  searchable?: boolean;
  clearable?: boolean;
  errorMessages?: Record<string, string>;
}

interface SelectOption {
  value: string;
  label: string;
  disabled?: boolean;
  icon?: string;
}
```

**Использование:**
```html
<app-select
  formControlName="role"
  label="Роль"
  [options]="roleOptions"
  placeholder="Выберите роль">
</app-select>

<app-select
  formControlName="categories"
  label="Категории"
  [options]="categoryOptions"
  [multiple]="true"
  [searchable]="true"
  placeholder="Выберите категории">
</app-select>
```

---

### Checkbox

**Путь:** `src/app/shared/components/checkbox/`

**Назначение:** Флажок для одиночного или множественного выбора.

**Входные параметры:**
```typescript
interface CheckboxProps {
  label?: string;
  checked?: boolean;
  disabled?: boolean;
  indeterminate?: boolean;
  value?: string;
}
```

---

### Radio

**Путь:** `src/app/shared/components/radio/`

**Назначение:** Радиокнопка для выбора одного варианта из группы.

**Входные параметры:**
```typescript
interface RadioProps {
  name: string;
  value: string;
  label?: string;
  checked?: boolean;
  disabled?: boolean;
}
```

---

### Textarea

**Путь:** `src/app/shared/components/textarea/`

**Назначение:** Многострочное текстовое поле.

**Входные параметры:**
```typescript
interface TextareaProps {
  label?: string;
  placeholder?: string;
  value?: string;
  rows?: number;
  maxLength?: number;
  disabled?: boolean;
  autosize?: boolean;
}
```

---

### Modal

**Путь:** `src/app/shared/components/modal/`

**Назначение:** Модальное окно с анимациями и функциями закрытия.

**Входные параметры:**
```typescript
interface ModalProps {
  title?: string;
  size?: 'small' | 'medium' | 'large' | 'fullscreen';
  closable?: boolean;
  maskClosable?: boolean;
  footer?: TemplateRef<unknown>;
  showHeader?: boolean;
  showFooter?: boolean;
}
```

**Использование:**
```html
<app-modal
  [(visible)]="showModal"
  title="Подтверждение"
  size="medium"
  [maskClosable]="false"
  (closed)="onModalClose()">
  
  <p>Вы уверены, что хотите удалить этот элемент?</p>
  
  <ng-template #footer>
    <app-button variant="secondary" (click)="showModal = false">
      Отмена
    </app-button>
    <app-button variant="danger" (click)="confirmDelete()">
      Удалить
    </app-button>
  </ng-template>
</app-modal>
```

---

### Toast (Snackbar)

**Путь:** `src/app/shared/components/toast/`

**Назначение:** Всплывающие уведомления.

**Типы:**
- `success` - Успешное действие
- `error` - Ошибка
- `warning` - Предупреждение
- `info` - Информация

**Использование через сервис:**
```typescript
@Injectable({ providedIn: 'root' })
export class ToastService {
  show(message: string, type: ToastType = 'info', duration = 3000): void {
    // Показать toast
  }

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  warning(message: string): void {
    this.show(message, 'warning');
  }

  info(message: string): void {
    this.show(message, 'info');
  }
}

// Использование
constructor(private toast: ToastService) {}

save() {
  this.toast.success('Данные сохранены');
}

onError() {
  this.toast.error('Произошла ошибка');
}
```

---

### Loading

**Путь:** `src/app/shared/components/loading/`

**Назначение:** Индикаторы загрузки (spinner, skeleton).

**Варианты:**
```html
<!-- Spinner -->
<app-loading type="spinner" size="medium"></app-loading>

<!-- Skeleton для карточек -->
<app-loading type="skeleton" [lines]="3"></app-loading>

<!-- Skeleton для таблицы -->
<app-loading type="table" [rows]="5" [columns]="4"></app-loading>
```

---

### Pagination

**Путь:** `src/app/shared/components/pagination/`

**Назначение:** Пагинация для списков.

**Входные параметры:**
```typescript
interface PaginationProps {
  total: number;
  page: number;
  pageSize: number;
  pageSizeOptions?: number[];
  showPageSize?: boolean;
  showTotal?: boolean;
}
```

**Использование:**
```html
<app-pagination
  [total]="totalItems"
  [page]="currentPage"
  [pageSize]="pageSize"
  [pageSizeOptions]="[10, 25, 50, 100]"
  (pageChange)="onPageChange($event)"
  (pageSizeChange)="onPageSizeChange($event)">
</app-pagination>
```

---

### Table

**Путь:** `src/app/shared/components/table/`

**Назначение:** Таблица с сортировкой, пагинацией и действиями.

**Входные параметры:**
```typescript
interface TableProps {
  columns: TableColumn[];
  data: unknown[];
  sortable?: boolean;
  paginated?: boolean;
  pageSize?: number;
  selectable?: boolean;
  rowActions?: TableAction[];
}
```

**Использование:**
```html
<app-table
  [columns]="columns"
  [data]="data"
  [sortable]="true"
  [paginated]="true"
  [pageSize]="10"
  [selectable]="true"
  (rowClick)="onRowClick($event)"
  (sortChange)="onSortChange($event)"
  (selectionChange)="onSelectionChange($event)">
  
  <ng-template #cellTpl let-row let-column="column">
    @switch (column.key) {
      @case ('status') {
        <app-badge [type]="row.status">{{ row.status }}</app-badge>
      }
      @case ('actions') {
        <app-button variant="ghost" size="small" (click)="edit(row)">
          Редактировать
        </app-button>
      }
      @default {
        {{ row[column.key] }}
      }
    }
  </ng-template>
</app-table>
```

---

### Card

**Путь:** `src/app/shared/components/card/`

**Назначение:** Контейнер-карточка для контента.

**Входные параметры:**
```typescript
interface CardProps {
  title?: string;
  subtitle?: string;
  bordered?: boolean;
  hoverable?: boolean;
  padding?: 'none' | 'small' | 'medium' | 'large';
}
```

---

### Badge

**Путь:** `src/app/shared/components/badge/`

**Назначение:** Бейдж для статусов и меток.

**Варианты:**
```html
<app-badge type="success">Пройден</app-badge>
<app-badge type="danger">Не пройден</app-badge>
<app-badge type="warning">В процессе</app-badge>
<app-badge type="info">Доступен</app-badge>
```

---

### Avatar

**Путь:** `src/app/shared/components/avatar/`

**Назначение:** Аватар пользователя.

**Входные параметры:**
```typescript
interface AvatarProps {
  url?: string | null;
  name?: string;
  size?: 'small' | 'medium' | 'large' | 'xlarge';
  shape?: 'circle' | 'square';
}
```

**Использование:**
```html
<app-avatar 
  [url]="user.avatarUrl" 
  [name]="user.firstName + ' ' + user.lastName"
  size="large">
</app-avatar>

<!-- Фоллбек на инициалы -->
<app-avatar name="Иван Иванов" size="medium"></app-avatar>
```

---

### Dropdown

**Путь:** `src/app/shared/components/dropdown/`

**Назначение:** Выпадающее меню.

**Использование:**
```html
<app-dropdown>
  <button dropdownToggle>Меню</button>
  <div dropdownMenu>
    <a dropdownItem>Профиль</a>
    <a dropdownItem>Настройки</a>
    <div dropdownDivider></div>
    <a dropdownItem (click)="logout()">Выйти</a>
  </div>
</app-dropdown>
```

---

### Tabs

**Путь:** `src/app/shared/components/tabs/`

**Назначение:** Вкладки для организации контента.

**Использование:**
```html
<app-tabs [(activeTab)]="activeTab">
  <app-tab title="Основное" name="main">
    Контент основной вкладки
  </app-tab>
  <app-tab title="Дополнительно" name="additional">
    Контент дополнительной вкладки
  </app-tab>
  <app-tab title="Настройки" name="settings">
    Контент настроек
  </app-tab>
</app-tabs>
```

---

### Accordion

**Путь:** `src/app/shared/components/accordion/`

**Назначение:** Аккордеон для сворачиваемого контента.

**Использование:**
```html
<app-accordion>
  <app-accordion-item title="Вопрос 1" expanded>
    Ответ на вопрос 1
  </app-accordion-item>
  <app-accordion-item title="Вопрос 2">
    Ответ на вопрос 2
  </app-accordion-item>
  <app-accordion-item title="Вопрос 3">
    Ответ на вопрос 3
  </app-accordion-item>
</app-accordion>
```

---

### Tooltip

**Путь:** `src/app/shared/components/tooltip/`

**Назначение:** Всплывающая подсказка.

**Использование:**
```html
<button appTooltip="Нажмите для сохранения">Сохранить</button>

<div appTooltip="Редактировать" placement="right">
  <app-icon name="edit"></app-icon>
</div>
```

---

### Empty State

**Путь:** `src/app/shared/components/empty-state/`

**Назначение:** Отображение пустого состояния.

**Использование:**
```html
<app-empty-state
  icon="folder-open"
  title="Нет данных"
  description="Загрузите файл или создайте новый тест">
  <app-button (click)="createNew()">Создать</app-button>
</app-empty-state>
```

---

## Бизнес-компоненты (Feature Components)

### TestCard

**Путь:** `src/app/features/student/components/test-card/`

**Назначение:** Карточка теста для списка тестов.

**Входные параметры:**
```typescript
interface TestCardProps {
  test: AvailableTestDto;
}
```

**Отображаемая информация:**
- Название теста
- Описание
- Длительность
- Количество вопросов
- Проходной балл
- Статус
- Количество попыток
- Срок действия

**Визуальные состояния:**
```html
<!-- Доступный тест -->
<app-test-card 
  [test]="test" 
  (startTest)="onStartTest($event)">
</app-test-card>

<!-- Тест в процессе -->
<app-test-card 
  [test]="test" 
  status="in-progress"
  (continueTest)="onContinueTest($event)">
</app-test-card>

<!-- Завершённый тест -->
<app-test-card 
  [test]="test" 
  status="completed"
  (viewResults)="onViewResults($event)">
</app-test-card>
```

---

### QuestionCard

**Путь:** `src/app/features/student/components/question-card/`

**Назначение:** Карточка вопроса при прохождении теста.

**Входные параметры:**
```typescript
interface QuestionCardProps {
  question: TestQuestionDto;
  selectedOptions?: string[];
  showAnswer?: boolean;
}
```

**Типы вопросов:**

1. **Один ответ (Radio)**
```html
<mat-radio-group 
  [value]="selectedOptions[0]"
  (change)="onOptionSelect($event.value)">
  <mat-radio-button *ngFor="let option of question.options" [value]="option.optionId">
    {{ option.text }}
  </mat-radio-button>
</mat-radio-group>
```

2. **Несколько ответов (Checkbox)**
```html
<mat-checkbox 
  *ngFor="let option of question.options"
  [checked]="selectedOptions?.includes(option.optionId)"
  (change)="onOptionToggle(option.optionId, $event.checked)">
  {{ option.text }}
</mat-checkbox>
```

3. **Текстовый ответ**
```html
<textarea
  [maxlength]="question.maxLength"
  [(ngModel)]="textAnswer"
  (ngModelChange)="onTextAnswerChange($event)">
</textarea>
<mat-hint>{{ textAnswer?.length || 0 }} / {{ question.maxLength }}</mat-hint>
```

---

### Timer

**Путь:** `src/app/features/student/components/timer/`

**Назначение:** Таймер обратного отсчёта для теста.

**Входные параметры:**
```typescript
interface TimerProps {
  timeRemaining: number; // в секундах
  showWarning?: boolean;  // показать предупреждение при низком времени
  warningThreshold?: number; // порог для предупреждения (по умолчанию 300с)
}
```

**Выходные события:**
- `timeUp` - Время вышло
- `warning` - Предупреждение о низком времени

**Визуальные состояния:**
```html
<!-- Обычное состояние -->
<app-timer [timeRemaining]="3600"></app-timer>

<!-- Предупреждение (красный) -->
<app-timer [timeRemaining]="120" [showWarning]="true"></app-timer>
```

---

### ProgressBar

**Путь:** `src/app/features/student/components/progress-bar/`

**Назначение:** Прогресс прохождения теста.

**Входные параметры:**
```typescript
interface ProgressBarProps {
  current: number;
  total: number;
  mode?: 'determinate' | 'indeterminate';
}
```

---

### ScoreDisplay

**Путь:** `src/app/features/student/components/score-display/`

**Назначение:** Отображение оценки/результата.

**Входные параметры:**
```typescript
interface ScoreDisplayProps {
  score: number;
  maxScore: number;
  percentage: number;
  isPassed: boolean;
  passedThreshold: number;
  animated?: boolean;
}
```

**Визуализация:**
- Круговая диаграмма с процентом
- Цвет: зелёный (passed), красный (failed)
- Анимация при загрузке

---

### QuestionNavigator

**Путь:** `src/app/features/student/components/question-navigator/`

**Назначение:** Навигация по вопросам теста.

**Функции:**
- Показать все вопросы
- Отметить отвеченные/неотвеченные
- Отметить вопросы для проверки
- Быстрый переход к вопросу

---

### AnswerResult

**Путь:** `src/app/features/student/components/answer-result/`

**Назначение:** Детализация ответа в результатах теста.

**Входные параметры:**
```typescript
interface AnswerResultProps {
  answer: AnswerResultDto;
  showCorrectAnswers?: boolean;
}
```

**Отображаемая информация:**
- Текст вопроса
- Выбранный ответ
- Правильный ответ
- Баллы
- Обратная связь

---

### QuestionEditor

**Путь:** `src/app/features/teacher/components/question-editor/`

**Назначение:** Редактор вопроса для преподавателя.

**Функции:**
- Текст вопроса
- Категория и сложность
- Добавление/удаление вариантов
- Указание правильного ответа
- Загрузка изображения

---

### TestBuilder

**Путь:** `src/app/features/teacher/components/test-builder/`

**Назначение:** Конструктор теста.

**Функции:**
- Название и описание
- Выбор вопросов из банка
- Настройки (время, попытки, проходной балл)
- Предпросмотр

---

### ResultsTable

**Путь:** `src/app/features/teacher/components/results-table/`

**Назначение:** Таблица результатов студентов.

**Функции:**
- Фильтрация
- Сортировка
- Экспорт
- Детализация по клику

---

## Директивы

### AutofocusDirective

Автоматический фокус на элементе при отображении.

```html
<input appAutofocus>
```

### DebounceDirective

Задержка события (для поиска).

```html
<input (inputChange)="onSearch($event)" appDebounce="300">
```

### ClickOutsideDirective

Закрытие по клику вне элемента.

```html
<div (clickOutside)="close()">Контент</div>
```

### PermissionDirective

Показ/скрытие по роли.

```html
<button *appHasRole="'Admin'">Удалить</button>
<div *appHasRole="['Teacher', 'Admin']">Редактировать</div>
```

---

## Pipes

### DateFormatPipe

Форматирование даты.

```html
{{ date | dateFormat:'DD.MM.YYYY HH:mm' }}
```

### TruncatePipe

Обрезание текста.

```html
{{ text | truncate:100:'...' }}
```

### DurationPipe

Форматирование длительности.

```html
{{ seconds | duration }} <!-- 1:30:00 -->
```

### SafeHtmlPipe

Безопасный HTML.

```html
<div [innerHTML]="html | safeHtml"></div>
```
