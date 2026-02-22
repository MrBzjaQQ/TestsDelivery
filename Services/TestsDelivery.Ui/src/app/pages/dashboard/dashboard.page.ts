import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PortalApiService } from '../../services/portal-api.service';
import { AuthService } from '../../services/auth.service';
import { StudentProfile, AvailableTest } from '../../models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="dashboard">
      <header class="header">
        <h1>TestsDelivery</h1>
        <div class="user-info">
          <span>{{ authService.fullName() }}</span>
          <button (click)="logout()">Выйти</button>
        </div>
      </header>

      <main class="content">
        @if (isLoading()) {
          <div class="loading">Загрузка...</div>
        } @else {
          @if (profile()) {
            <section class="profile-section">
              <h2>Профиль</h2>
              <div class="profile-card">
                <div class="profile-info">
                  <div class="avatar">
                    {{ getInitials() }}
                  </div>
                  <div class="details">
                    <h3>{{ profile()!.firstName }} {{ profile()!.lastName }}</h3>
                    <p>{{ profile()!.email }}</p>
                    @if (profile()!.group) {
                      <p class="group">{{ profile()!.group!.name }}</p>
                    }
                  </div>
                </div>
                @if (profile()!.statistics) {
                  <div class="statistics">
                    <div class="stat">
                      <span class="value">{{ profile()!.statistics!.totalTests }}</span>
                      <span class="label">Всего тестов</span>
                    </div>
                    <div class="stat">
                      <span class="value">{{ profile()!.statistics!.completedTests }}</span>
                      <span class="label">Завершено</span>
                    </div>
                    <div class="stat">
                      <span class="value">{{ profile()!.statistics!.averageScore | number:'1.0-0' }}%</span>
                      <span class="label">Средний балл</span>
                    </div>
                  </div>
                }
              </div>
            </section>
          }

          <section class="tests-section">
            <h2>Доступные тесты</h2>
            @if (tests().length > 0) {
              <div class="tests-grid">
                @for (test of tests(); track test.testId) {
                  <div class="test-card">
                    <div class="test-header">
                      <h3>{{ test.testTitle }}</h3>
                      <span class="status" [class]="test.status">{{ test.status }}</span>
                    </div>
                    <p class="description">{{ test.description }}</p>
                    <div class="test-meta">
                      <span>⏱ {{ test.durationMinutes }} мин</span>
                      <span>📝 {{ test.questionsCount }} вопросов</span>
                      <span>🎯 {{ test.passingScore }}% для прохождения</span>
                    </div>
                    @if (test.canTake) {
                      <button class="btn-primary" [routerLink]="['/test', test.testId]">
                        Начать тест
                      </button>
                    } @else if (test.completedAt) {
                      <div class="result">
                        <span>Результат: {{ test.score }}/{{ test.maxScore }}</span>
                        <span class="percentage" [class.passed]="test.isPassed">
                          {{ test.percentage | number:'1.0-0' }}%
                        </span>
                      </div>
                      @if (!test.canTake && test.attemptsUsed < test.maxAttempts) {
                        <button class="btn-secondary" [routerLink]="['/test', test.testId]">
                          Пройти повторно
                        </button>
                      }
                    }
                  </div>
                }
              </div>
            } @else {
              <p class="no-tests">Нет доступных тестов</p>
            }
          </section>
        }
      </main>
    </div>
  `,
  styles: [`
    .dashboard {
      min-height: 100vh;
      background: #f5f5f5;
    }

    .header {
      background: white;
      padding: 1rem 2rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);

      h1 {
        margin: 0;
        color: #333;
        font-size: 1.5rem;
      }

      .user-info {
        display: flex;
        align-items: center;
        gap: 1rem;

        button {
          padding: 0.5rem 1rem;
          background: transparent;
          border: 1px solid #ddd;
          border-radius: 6px;
          cursor: pointer;

          &:hover {
            background: #f5f5f5;
          }
        }
      }
    }

    .content {
      max-width: 1200px;
      margin: 0 auto;
      padding: 2rem;
    }

    .loading {
      text-align: center;
      padding: 3rem;
      color: #666;
    }

    section {
      margin-bottom: 2rem;
    }

    h2 {
      color: #333;
      margin-bottom: 1rem;
    }

    .profile-card {
      background: white;
      border-radius: 12px;
      padding: 1.5rem;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }

    .profile-info {
      display: flex;
      gap: 1.5rem;
      align-items: center;
      margin-bottom: 1.5rem;
    }

    .avatar {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      background: #007bff;
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 1.5rem;
      font-weight: 500;
    }

    .details {
      h3 {
        margin: 0 0 0.25rem;
        color: #333;
      }

      p {
        margin: 0;
        color: #666;
      }

      .group {
        margin-top: 0.5rem;
        font-weight: 500;
        color: #007bff;
      }
    }

    .statistics {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 1rem;
      padding-top: 1rem;
      border-top: 1px solid #eee;
    }

    .stat {
      text-align: center;

      .value {
        display: block;
        font-size: 1.5rem;
        font-weight: 600;
        color: #333;
      }

      .label {
        font-size: 0.875rem;
        color: #666;
      }
    }

    .tests-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
      gap: 1.5rem;
    }

    .test-card {
      background: white;
      border-radius: 12px;
      padding: 1.5rem;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);

      .test-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 0.75rem;

        h3 {
          margin: 0;
          color: #333;
          font-size: 1.125rem;
        }

        .status {
          font-size: 0.75rem;
          padding: 0.25rem 0.5rem;
          border-radius: 4px;
          font-weight: 500;

          &.available {
            background: #d4edda;
            color: #155724;
          }

          &.completed {
            background: #cce5ff;
            color: #004085;
          }

          &.expired {
            background: #f8d7da;
            color: #721c24;
          }
        }
      }

      .description {
        color: #666;
        margin-bottom: 1rem;
        font-size: 0.875rem;
      }

      .test-meta {
        display: flex;
        flex-wrap: wrap;
        gap: 1rem;
        font-size: 0.875rem;
        color: #666;
        margin-bottom: 1rem;
      }

      .result {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0.75rem;
        background: #f8f9fa;
        border-radius: 6px;
        margin-bottom: 0.75rem;

        .percentage {
          font-weight: 600;

          &.passed {
            color: #28a745;
          }
        }
      }

      .btn-primary, .btn-secondary {
        width: 100%;
        padding: 0.75rem;
        border: none;
        border-radius: 6px;
        font-size: 1rem;
        font-weight: 500;
        cursor: pointer;
        transition: opacity 0.2s;
      }

      .btn-primary {
        background: #007bff;
        color: white;

        &:hover {
          opacity: 0.9;
        }
      }

      .btn-secondary {
        background: #6c757d;
        color: white;

        &:hover {
          opacity: 0.9;
        }
      }
    }

    .no-tests {
      text-align: center;
      color: #666;
      padding: 2rem;
    }
  `]
})
export class DashboardPageComponent implements OnInit {
  readonly authService = inject(AuthService);
  private readonly portalApi = inject(PortalApiService);

  profile = signal<StudentProfile | null>(null);
  tests = signal<AvailableTest[]>([]);
  isLoading = signal(true);

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.portalApi.getStudentProfile().subscribe({
      next: (response) => {
        if (!response.isError && response.data) {
          this.profile.set(response.data);
        }
      },
      error: () => {
        // Handle error
      }
    });

    this.portalApi.getAvailableTests().subscribe({
      next: (response) => {
        if (!response.isError && response.data) {
          this.tests.set(response.data.tests);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  getInitials(): string {
    const profile = this.profile();
    if (!profile) return '?';
    return `${profile.firstName.charAt(0)}${profile.lastName.charAt(0)}`;
  }

  logout(): void {
    this.authService.logout();
  }
}
