import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PortalApiService } from '../../services/portal-api.service';
import { TestResults } from '../../models';

@Component({
  selector: 'app-results',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="results-container">
      @if (isLoading()) {
        <div class="loading">Загрузка результатов...</div>
      } @else if (results()) {
        <div class="results-card">
          <div class="result-header" [class.passed]="results()!.isPassed" [class.failed]="!results()!.isPassed">
            <div class="status-icon">
              @if (results()!.isPassed) {
                <span>✓</span>
              } @else {
                <span>✗</span>
              }
            </div>
            <h1>{{ results()!.isPassed ? 'Тест пройден!' : 'Тест не пройден' }}</h1>
            <p class="test-title">{{ results()!.testTitle }}</p>
          </div>

          <div class="score-section">
            <div class="score-circle" [class.passed]="results()!.isPassed">
              <span class="score">{{ results()!.percentage | number:'1.0-0' }}%</span>
            </div>
            <div class="score-details">
              <p>Баллы: <strong>{{ results()!.score }} / {{ results()!.maxScore }}</strong></p>
              <p>Порог прохождения: <strong>{{ results()!.passedThreshold }}%</strong></p>
              <p>Попытка: <strong>#{{ results()!.attemptNumber }}</strong></p>
            </div>
          </div>

          <div class="answers-section">
            <h2>Ответы</h2>
            @for (answer of results()!.answers; track answer.questionId) {
              <div class="answer-card" [class.correct]="answer.isCorrect" [class.incorrect]="!answer.isCorrect">
                <div class="answer-header">
                  <span class="status">
                    @if (answer.isCorrect) {
                      <span class="icon-correct">✓</span>
                    } @else {
                      <span class="icon-incorrect">✗</span>
                    }
                  </span>
                  <span class="points">{{ answer.pointsEarned }} баллов</span>
                </div>
                <p class="question-text">{{ answer.questionText }}</p>
                @if (answer.feedback) {
                  <p class="feedback">{{ answer.feedback }}</p>
                }
              </div>
            }
          </div>

          <div class="actions">
            @if (results()!.canRetake) {
              <button class="btn-primary" [routerLink]="['/test', results()!.testId]">
                Пройти заново ({{ results()!.attemptsRemaining }} попыток осталось)
              </button>
            }
            <button class="btn-secondary" routerLink="/dashboard">
              Вернуться к списку тестов
            </button>
          </div>
        </div>
      } @else {
        <div class="error">Результаты не найдены</div>
      }
    </div>
  `,
  styles: [`
    .results-container {
      min-height: 100vh;
      background: #f5f5f5;
      padding: 2rem;
    }

    .loading, .error {
      text-align: center;
      padding: 3rem;
      color: #666;
    }

    .results-card {
      max-width: 800px;
      margin: 0 auto;
      background: white;
      border-radius: 12px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
      overflow: hidden;
    }

    .result-header {
      text-align: center;
      padding: 2rem;
      color: white;

      &.passed {
        background: linear-gradient(135deg, #28a745, #20c997);
      }

      &.failed {
        background: linear-gradient(135deg, #dc3545, #c82333);
      }

      .status-icon {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        background: rgba(255,255,255,0.2);
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto 1rem;
        font-size: 2.5rem;
      }

      h1 {
        margin: 0 0 0.5rem;
        font-size: 1.75rem;
      }

      .test-title {
        margin: 0;
        opacity: 0.9;
      }
    }

    .score-section {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 2rem;
      padding: 2rem;
      border-bottom: 1px solid #eee;
    }

    .score-circle {
      width: 120px;
      height: 120px;
      border-radius: 50%;
      background: #dc3545;
      display: flex;
      align-items: center;
      justify-content: center;

      &.passed {
        background: #28a745;
      }

      .score {
        font-size: 2rem;
        font-weight: 700;
        color: white;
      }
    }

    .score-details {
      p {
        margin: 0.5rem 0;
        color: #666;

        strong {
          color: #333;
        }
      }
    }

    .answers-section {
      padding: 2rem;

      h2 {
        margin: 0 0 1rem;
        color: #333;
      }
    }

    .answer-card {
      padding: 1rem;
      border: 1px solid #e9ecef;
      border-radius: 8px;
      margin-bottom: 1rem;

      &.correct {
        border-left: 4px solid #28a745;
      }

      &.incorrect {
        border-left: 4px solid #dc3545;
      }

      .answer-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 0.5rem;

        .status {
          .icon-correct {
            color: #28a745;
            font-size: 1.25rem;
          }

          .icon-incorrect {
            color: #dc3545;
            font-size: 1.25rem;
          }
        }

        .points {
          font-size: 0.875rem;
          color: #666;
        }
      }

      .question-text {
        margin: 0;
        color: #333;
      }

      .feedback {
        margin: 0.5rem 0 0;
        font-size: 0.875rem;
        color: #666;
        font-style: italic;
      }
    }

    .actions {
      padding: 2rem;
      display: flex;
      flex-direction: column;
      gap: 1rem;

      button {
        padding: 0.875rem;
        border: none;
        border-radius: 6px;
        font-size: 1rem;
        font-weight: 500;
        cursor: pointer;
        transition: opacity 0.2s;

        &:hover {
          opacity: 0.9;
        }
      }

      .btn-primary {
        background: #007bff;
        color: white;
      }

      .btn-secondary {
        background: #6c757d;
        color: white;
      }
    }
  `]
})
export class ResultsPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly portalApi = inject(PortalApiService);

  results = signal<TestResults | null>(null);
  isLoading = signal(true);

  ngOnInit(): void {
    const testId = this.route.snapshot.paramMap.get('id');
    if (testId) {
      this.loadResults(testId);
    }
  }

  private loadResults(testId: string): void {
    this.portalApi.getTestResults(testId).subscribe({
      next: (response) => {
        if (!response.isError && response.data) {
          this.results.set(response.data);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }
}
