import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="register-container">
      <div class="register-card">
        <h1>TestsDelivery</h1>
        <p class="subtitle">Регистрация</p>

        <form (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="firstName">Имя</label>
            <input
              type="text"
              id="firstName"
              [(ngModel)]="firstName"
              name="firstName"
              placeholder="Введите имя"
              required
            />
          </div>

          <div class="form-group">
            <label for="lastName">Фамилия</label>
            <input
              type="text"
              id="lastName"
              [(ngModel)]="lastName"
              name="lastName"
              placeholder="Введите фамилию"
              required
            />
          </div>

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
              placeholder="Создайте пароль"
              required
              minlength="8"
            />
          </div>

          <div class="form-group">
            <label for="confirmPassword">Подтверждение пароля</label>
            <input
              type="password"
              id="confirmPassword"
              [(ngModel)]="confirmPassword"
              name="confirmPassword"
              placeholder="Повторите пароль"
              required
            />
          </div>

          <div class="form-group">
            <label for="role">Роль</label>
            <select id="role" [(ngModel)]="role" name="role" required>
              <option value="Student">Студент</option>
              <option value="Teacher">Преподаватель</option>
            </select>
          </div>

          @if (error()) {
            <div class="error-message">{{ error() }}</div>
          }

          @if (success()) {
            <div class="success-message">{{ success() }}</div>
          }

          <button type="submit" [disabled]="isLoading()">
            @if (isLoading()) {
              <span>Регистрация...</span>
            } @else {
              <span>Зарегистрироваться</span>
            }
          </button>
        </form>

        <div class="auth-links">
          <span>Уже есть аккаунт?</span>
          <a routerLink="/login" class="login-link">Войти</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .register-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f5f5f5;
      padding: 2rem 0;
    }

    .register-card {
      background: white;
      padding: 2.5rem;
      border-radius: 12px;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
      width: 100%;
      max-width: 450px;
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

    input, select {
      width: 100%;
      padding: 0.75rem;
      border: 1px solid #ddd;
      border-radius: 6px;
      font-size: 1rem;
      box-sizing: border-box;
      background: white;
    }

    input:focus, select:focus {
      outline: none;
      border-color: #007bff;
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
      margin-top: 0.5rem;
    }

    button:hover:not(:disabled) {
      background: #0056b3;
    }

    button:disabled {
      opacity: 0.7;
      cursor: not-allowed;
    }

    .error-message {
      color: #dc3545;
      font-size: 0.875rem;
      margin-bottom: 1rem;
      text-align: center;
      padding: 0.75rem;
      background: #f8d7da;
      border-radius: 4px;
    }

    .success-message {
      color: #155724;
      font-size: 0.875rem;
      margin-bottom: 1rem;
      text-align: center;
      padding: 0.75rem;
      background: #d4edda;
      border-radius: 4px;
    }

    .auth-links {
      margin-top: 1.5rem;
      text-align: center;
      font-size: 0.9rem;
    }

    .login-link {
      color: #007bff;
      text-decoration: none;
      margin-left: 0.5rem;
    }

    .login-link:hover {
      text-decoration: underline;
    }
  `]
})
export class RegisterPageComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  firstName = '';
  lastName = '';
  email = '';
  password = '';
  confirmPassword = '';
  role: 'Student' | 'Teacher' = 'Student';
  isLoading = signal(false);
  error = signal('');
  success = signal('');

  onSubmit(): void {
    if (!this.firstName || !this.lastName || !this.email || !this.password || !this.confirmPassword) {
      this.error.set('Пожалуйста, заполните все поля');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error.set('Пароли не совпадают');
      return;
    }

    if (this.password.length < 8) {
      this.error.set('Пароль должен содержать минимум 8 символов');
      return;
    }

    this.isLoading.set(true);
    this.error.set('');
    this.success.set('');

    this.authService.register({
      email: this.email,
      password: this.password,
      firstName: this.firstName,
      lastName: this.lastName,
      role: this.role,
    }).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.success.set('Регистрация успешна! Проверьте email для подтверждения.');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err) => {
        this.isLoading.set(false);
        const detail = err?.error?.detail || 'Ошибка при регистрации';
        this.error.set(detail);
      }
    });
  }
}
