import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="login-container">
      <div class="login-card">
        <h1>TestsDelivery</h1>
        <p class="subtitle">Вход в систему</p>

        <form (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="email">Email</label>
            <input
              type="email"
              id="email"
              [(ngModel)]="email"
              name="email"
              placeholder="student@university.edu"
              required
            />
          </div>

          <div class="form-group">
            <label for="password">Пароль</label>
            <input
              type="password"
              id="password"
              [(ngModel)]="password"
              name="password"
              placeholder="Введите пароль"
              required
            />
          </div>

          @if (error()) {
            <div class="error-message">{{ error() }}</div>
          }

          <button type="submit" [disabled]="isLoading()">
            @if (isLoading()) {
              <span>Вход...</span>
            } @else {
              <span>Войти</span>
            }
          </button>
        </form>

        <div class="auth-links">
          <a routerLink="/forgot-password" class="forgot-link">Забыли пароль?</a>
          <span class="separator">|</span>
          <a routerLink="/register" class="register-link">Регистрация</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f5f5f5;
    }

    .login-card {
      background: white;
      padding: 2.5rem;
      border-radius: 12px;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
      width: 100%;
      max-width: 400px;
    }

    h1 {
      margin: 0 0 0.5rem;
      color: #333;
      font-size: 1.75rem;
      text-align: center;
    }

    .subtitle {
      text-align: center;
      color: #666;
      margin-bottom: 2rem;
    }

    .form-group {
      margin-bottom: 1.25rem;
    }

    label {
      display: block;
      margin-bottom: 0.5rem;
      color: #444;
      font-weight: 500;
    }

    input {
      width: 100%;
      padding: 0.75rem;
      border: 1px solid #ddd;
      border-radius: 6px;
      font-size: 1rem;
      box-sizing: border-box;

      &:focus {
        outline: none;
        border-color: #007bff;
      }
    }

    button {
      width: 100%;
      padding: 0.875rem;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 6px;
      font-size: 1rem;
      font-weight: 500;
      cursor: pointer;
      transition: background 0.2s;

      &:hover:not(:disabled) {
        background: #0056b3;
      }

      &:disabled {
        opacity: 0.7;
        cursor: not-allowed;
      }
    }

    .error-message {
      color: #dc3545;
      font-size: 0.875rem;
      margin-bottom: 1rem;
      text-align: center;
    }

    .auth-links {
      margin-top: 1.5rem;
      text-align: center;
      font-size: 0.9rem;
    }

    .forgot-link, .register-link {
      color: #007bff;
      text-decoration: none;
      transition: color 0.2s;
    }

    .forgot-link:hover, .register-link:hover {
      color: #0056b3;
      text-decoration: underline;
    }

    .separator {
      margin: 0 0.75rem;
      color: #999;
    }
  `]
})
export class LoginPageComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  isLoading = signal(false);
  error = signal('');

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.error.set('Пожалуйста, заполните все поля');
      return;
    }

    this.isLoading.set(true);
    this.error.set('');

    this.authService.login({
      email: this.email,
      password: this.password,
    }).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.isLoading.set(false);
        const detail = err?.error?.detail || 'Неверный email или пароль';
        this.error.set(detail);
      }
    });
  }
}
