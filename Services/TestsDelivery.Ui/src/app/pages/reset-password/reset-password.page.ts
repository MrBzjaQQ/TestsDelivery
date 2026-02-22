import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="reset-password-container">
      <div class="reset-password-card">
        <h1>TestsDelivery</h1>
        <p class="subtitle">Новый пароль</p>

        @if (invalidToken()) {
          <div class="error-message">
            Ссылка для сброса пароля недействительна или истекла.
          </div>
          <div class="auth-links">
            <a routerLink="/forgot-password" class="back-link">Запросить новую ссылку</a>
          </div>
        } @else if (success()) {
          <div class="success-message">
            <div class="success-icon">✓</div>
            <p>Пароль успешно изменён!</p>
            <p class="hint">Теперь вы можете войти с новым паролем.</p>
          </div>
          <button type="button" (click)="goToLogin()" class="login-button">
            Войти
          </button>
        } @else {
          <p class="description">
            Введите новый пароль для вашей учётной записи.
          </p>

          <form (ngSubmit)="onSubmit()">
            <div class="form-group">
              <label for="newPassword">Новый пароль</label>
              <input
                type="password"
                id="newPassword"
                [(ngModel)]="newPassword"
                name="newPassword"
                placeholder="Создайте новый пароль"
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
                placeholder="Повторите новый пароль"
                required
              />
            </div>

            @if (error()) {
              <div class="error-message">{{ error() }}</div>
            }

            <button type="submit" [disabled]="isLoading()">
              @if (isLoading()) {
                <span>Сохранение...</span>
              } @else {
                <span>Сохранить пароль</span>
              }
            </button>
          </form>
        }

        <div class="auth-links">
          <a routerLink="/login" class="back-link">← Назад к входу</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .reset-password-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f5f5f5;
      padding: 2rem 0;
    }

    .reset-password-card {
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
      margin-bottom: 1.5rem;
    }

    .description {
      color: #666;
      font-size: 0.9rem;
      text-align: center;
      margin-bottom: 1.5rem;
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
    }

    input:focus {
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
    }

    button:hover:not(:disabled) {
      background: #0056b3;
    }

    button:disabled {
      opacity: 0.7;
      cursor: not-allowed;
    }

    .login-button {
      margin-top: 1rem;
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
      text-align: center;
      padding: 1rem;
    }

    .success-icon {
      width: 60px;
      height: 60px;
      background: #28a745;
      color: white;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 2rem;
      margin: 0 auto 1rem;
    }

    .success-message p {
      color: #155724;
      margin: 0.5rem 0;
    }

    .hint {
      font-size: 0.85rem;
      color: #666 !important;
    }

    .auth-links {
      margin-top: 1.5rem;
      text-align: center;
      font-size: 0.9rem;
    }

    .back-link {
      color: #007bff;
      text-decoration: none;
    }

    .back-link:hover {
      text-decoration: underline;
    }
  `]
})
export class ResetPasswordPageComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  token = '';
  newPassword = '';
  confirmPassword = '';
  isLoading = signal(false);
  error = signal('');
  success = signal(false);
  invalidToken = signal(false);

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      if (!this.token) {
        this.invalidToken.set(true);
      }
    });
  }

  onSubmit(): void {
    if (!this.newPassword || !this.confirmPassword) {
      this.error.set('Пожалуйста, заполните все поля');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.error.set('Пароли не совпадают');
      return;
    }

    if (this.newPassword.length < 8) {
      this.error.set('Пароль должен содержать минимум 8 символов');
      return;
    }

    this.isLoading.set(true);
    this.error.set('');

    this.authService.resetPassword(this.token, this.newPassword).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.success.set(true);
      },
      error: (err) => {
        this.isLoading.set(false);
        const detail = err?.error?.detail || 'Не удалось сбросить пароль';
        this.error.set(detail);
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
