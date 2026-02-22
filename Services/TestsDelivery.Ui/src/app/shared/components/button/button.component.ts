import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <button
      [type]="type"
      [disabled]="disabled || loading"
      [class]="buttonClasses"
      (click)="onClick($event)"
    >
      @if (loading) {
        <span class="spinner"></span>
      }
      <ng-content></ng-content>
    </button>
  `,
  styles: [`
    button {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      gap: 0.5rem;
      padding: 0.625rem 1.25rem;
      font-size: 0.875rem;
      font-weight: 500;
      border-radius: 0.375rem;
      cursor: pointer;
      transition: all 0.2s ease;
      border: none;

      &:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }

      &:focus-visible {
        outline: 2px solid #2563eb;
        outline-offset: 2px;
      }
    }

    .btn-primary {
      background: #2563eb;
      color: white;

      &:hover:not(:disabled) {
        background: #1d4ed8;
      }
    }

    .btn-secondary {
      background: #64748b;
      color: white;

      &:hover:not(:disabled) {
        background: #475569;
      }
    }

    .btn-outline {
      background: transparent;
      border: 1px solid #d1d5db;
      color: #374151;

      &:hover:not(:disabled) {
        background: #f3f4f6;
      }
    }

    .btn-ghost {
      background: transparent;
      color: #374151;

      &:hover:not(:disabled) {
        background: #f3f4f6;
      }
    }

    .btn-danger {
      background: #ef4444;
      color: white;

      &:hover:not(:disabled) {
        background: #dc2626;
      }
    }

    .btn-sm {
      padding: 0.375rem 0.75rem;
      font-size: 0.75rem;
    }

    .btn-lg {
      padding: 0.875rem 1.75rem;
      font-size: 1rem;
    }

    .btn-full {
      width: 100%;
    }

    .spinner {
      width: 1rem;
      height: 1rem;
      border: 2px solid transparent;
      border-top-color: currentColor;
      border-radius: 50%;
      animation: spin 0.6s linear infinite;
    }

    @keyframes spin {
      to {
        transform: rotate(360deg);
      }
    }
  `]
})
export class ButtonComponent {
  @Input() variant: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger' = 'primary';
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() fullWidth = false;

  @Output() clicked = new EventEmitter<void>();

  get buttonClasses(): string {
    return [
      `btn-${this.variant}`,
      this.size !== 'medium' ? `btn-${this.size}` : '',
      this.fullWidth ? 'btn-full' : ''
    ].filter(Boolean).join(' ');
  }

  onClick(event: MouseEvent): void {
    if (!this.disabled && !this.loading) {
      this.clicked.emit();
    }
  }
}
