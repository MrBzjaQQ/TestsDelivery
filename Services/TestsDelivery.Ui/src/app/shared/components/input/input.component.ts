import { Component, Input, forwardRef, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    }
  ],
  template: `
    <div class="input-wrapper">
      @if (label) {
        <label class="label">{{ label }}</label>
      }
      <div class="input-container" [class.has-error]="error" [class.disabled]="disabled">
        @if (prefixIcon) {
          <span class="prefix-icon">{{ prefixIcon }}</span>
        }
        <input
          [type]="type"
          [placeholder]="placeholder"
          [disabled]="disabled"
          [readonly]="readonly"
          [maxlength]="maxLength ?? null"
          [minlength]="minLength ?? null"
          [(ngModel)]="value"
          (ngModelChange)="onValueChange($event)"
          (blur)="onTouched()"
        />
        @if (suffixIcon) {
          <span class="suffix-icon">{{ suffixIcon }}</span>
        }
      </div>
      @if (error) {
        <span class="error-message">{{ error }}</span>
      } @else if (hint) {
        <span class="hint">{{ hint }}</span>
      }
    </div>
  `,
  styles: [`
    .input-wrapper {
      display: flex;
      flex-direction: column;
      gap: 0.375rem;
    }

    .label {
      font-size: 0.875rem;
      font-weight: 500;
      color: #374151;
    }

    .input-container {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.625rem 0.875rem;
      border: 1px solid #d1d5db;
      border-radius: 0.375rem;
      background: white;
      transition: all 0.2s;

      &:focus-within {
        border-color: #2563eb;
        box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
      }

      &.has-error {
        border-color: #ef4444;

        &:focus-within {
          box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
        }
      }

      &.disabled {
        background: #f3f4f6;
        cursor: not-allowed;
      }
    }

    input {
      flex: 1;
      border: none;
      outline: none;
      font-size: 0.875rem;
      color: #1f2937;
      background: transparent;

      &::placeholder {
        color: #9ca3af;
      }

      &:disabled {
        cursor: not-allowed;
      }
    }

    .prefix-icon,
    .suffix-icon {
      color: #9ca3af;
      font-size: 1rem;
    }

    .error-message {
      font-size: 0.75rem;
      color: #ef4444;
    }

    .hint {
      font-size: 0.75rem;
      color: #6b7280;
    }
  `]
})
export class InputComponent implements ControlValueAccessor {
  @Input() label?: string;
  @Input() placeholder = '';
  @Input() type: 'text' | 'email' | 'password' | 'number' | 'tel' | 'url' = 'text';
  @Input() disabled = false;
  @Input() readonly = false;
  @Input() error?: string;
  @Input() hint?: string;
  @Input() prefixIcon?: string;
  @Input() suffixIcon?: string;
  @Input() maxLength?: number;
  @Input() minLength?: number;

  value = '';

  private onChange: (value: string) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onValueChange(value: string): void {
    this.value = value;
    this.onChange(value);
  }
}
