import { Component, inject, signal, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PortalApiService } from '../../services/portal-api.service';
import { TestQuestions, TestQuestion, AnswerItem } from '../../models';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="test-container">
      @if (isLoading()) {
        <div class="loading">Загрузка теста...</div>
      } @else if (test()) {
        <header class="test-header">
          <div class="test-info">
            <h1>{{ test()!.testTitle }}</h1>
            <p>{{ test()!.testDescription }}</p>
          </div>
          <div class="timer" [class.warning]="timeRemaining() < 300">
            <span class="time">{{ formatTime(timeRemaining()) }}</span>
            <span class="label">осталось</span>
          </div>
        </header>

        <main class="test-content">
          <div class="progress-bar">
            <div class="progress" [style.width.%]="progress()"></div>
          </div>

          <div class="question-card">
            <div class="question-header">
              <span class="question-number">Вопрос {{ currentQuestionIndex() + 1 }} из {{ test()!.totalQuestions }}</span>
              <span class="question-category">{{ currentQuestion()!.category }}</span>
            </div>

            <div class="question-text">
              {{ currentQuestion()!.text }}
            </div>

            @if (currentQuestion()!.options.length > 0) {
              <div class="options">
                @for (option of currentQuestion()!.options; track option.optionId) {
                  <label class="option" [class.selected]="isOptionSelected(option.optionId)">
                    <input
                      type="radio"
                      [name]="'question-' + currentQuestion()!.questionId"
                      [value]="option.optionId"
                      (change)="selectOption(option.optionId)"
                      [checked]="isOptionSelected(option.optionId)"
                    />
                    <span class="option-text">{{ option.text }}</span>
                  </label>
                }
              </div>
            } @else if (currentQuestion()!.answerType === 'text') {
              <div class="text-answer">
                <textarea
                  [(ngModel)]="textAnswers()[currentQuestion()!.questionId]"
                  [maxlength]="currentQuestion()!.maxLength ?? 1000"
                  placeholder="Введите ваш ответ..."
                ></textarea>
              </div>
            }
          </div>

          <div class="navigation">
            <button
              class="btn-secondary"
              [disabled]="currentQuestionIndex() === 0"
              (click)="previousQuestion()"
            >
              Назад
            </button>
            
            <div class="question-dots">
              @for (q of test()!.questions; track q.questionId; let i = $index) {
                <button
                  class="dot"
                  [class.active]="i === currentQuestionIndex()"
                  [class.answered]="hasAnswer(q.questionId)"
                  (click)="goToQuestion(i)"
                ></button>
              }
            </div>

            @if (currentQuestionIndex() < test()!.questions.length - 1) {
              <button class="btn-primary" (click)="nextQuestion()">
                Далее
              </button>
            } @else {
              <button class="btn-submit" (click)="submitTest()">
                Завершить тест
              </button>
            }
          </div>
        </main>
      } @else {
        <div class="error">Тест не найден</div>
      }
    </div>
  `,
  styles: [`
    .test-container {
      min-height: 100vh;
      background: #f5f5f5;
    }

    .loading, .error {
      text-align: center;
      padding: 3rem;
      color: #666;
    }

    .test-header {
      background: white;
      padding: 1.5rem 2rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);

      h1 {
        margin: 0 0 0.25rem;
        color: #333;
        font-size: 1.5rem;
      }

      p {
        margin: 0;
        color: #666;
      }

      .timer {
        text-align: center;
        padding: 0.75rem 1.5rem;
        background: #e9ecef;
        border-radius: 8px;

        &.warning {
          background: #fff3cd;
        }

        .time {
          display: block;
          font-size: 1.5rem;
          font-weight: 600;
          color: #333;
        }

        .label {
          font-size: 0.75rem;
          color: #666;
        }
      }
    }

    .test-content {
      max-width: 800px;
      margin: 0 auto;
      padding: 2rem;
    }

    .progress-bar {
      height: 4px;
      background: #e9ecef;
      border-radius: 2px;
      margin-bottom: 2rem;

      .progress {
        height: 100%;
        background: #007bff;
        border-radius: 2px;
        transition: width 0.3s;
      }
    }

    .question-card {
      background: white;
      border-radius: 12px;
      padding: 2rem;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
      margin-bottom: 2rem;
    }

    .question-header {
      display: flex;
      justify-content: space-between;
      margin-bottom: 1rem;

      .question-number {
        font-weight: 500;
        color: #333;
      }

      .question-category {
        font-size: 0.875rem;
        color: #666;
        background: #e9ecef;
        padding: 0.25rem 0.75rem;
        border-radius: 4px;
      }
    }

    .question-text {
      font-size: 1.125rem;
      color: #333;
      margin-bottom: 1.5rem;
      line-height: 1.6;
    }

    .options {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }

    .option {
      display: flex;
      align-items: center;
      gap: 1rem;
      padding: 1rem;
      border: 2px solid #e9ecef;
      border-radius: 8px;
      cursor: pointer;
      transition: all 0.2s;

      &:hover {
        border-color: #007bff;
        background: #f8f9fa;
      }

      &.selected {
        border-color: #007bff;
        background: #e7f1ff;
      }

      input {
        width: 20px;
        height: 20px;
      }

      .option-text {
        color: #333;
      }
    }

    .text-answer {
      textarea {
        width: 100%;
        min-height: 150px;
        padding: 1rem;
        border: 2px solid #e9ecef;
        border-radius: 8px;
        font-size: 1rem;
        resize: vertical;
        box-sizing: border-box;

        &:focus {
          outline: none;
          border-color: #007bff;
        }
      }
    }

    .navigation {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 1rem;
    }

    .question-dots {
      display: flex;
      gap: 0.5rem;

      .dot {
        width: 12px;
        height: 12px;
        border-radius: 50%;
        border: none;
        background: #e9ecef;
        cursor: pointer;

        &.active {
          background: #007bff;
        }

        &.answered {
          background: #28a745;
        }
      }
    }

    .btn-primary, .btn-secondary, .btn-submit {
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 6px;
      font-size: 1rem;
      font-weight: 500;
      cursor: pointer;
      transition: opacity 0.2s;

      &:disabled {
        opacity: 0.5;
        cursor: not-allowed;
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

    .btn-submit {
      background: #28a745;
      color: white;

      &:hover:not(:disabled) {
        opacity: 0.9;
      }
    }
  `]
})
export class TestPageComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly portalApi = inject(PortalApiService);

  test = signal<TestQuestions | null>(null);
  isLoading = signal(true);
  currentQuestionIndex = signal(0);
  
  private selectedOptions = signal<Map<string, string[]>>(new Map());
  protected textAnswers = signal<Record<string, string>>({});
  private timer: ReturnType<typeof setInterval> | null = null;
  protected timeRemaining = signal(0);

  progress = signal(0);

  ngOnInit(): void {
    const testId = this.route.snapshot.paramMap.get('id');
    if (testId) {
      this.loadTest(testId);
    }
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  private loadTest(testId: string): void {
    this.portalApi.getTestQuestions(testId).subscribe({
      next: (response) => {
        if (!response.isError && response.data) {
          this.test.set(response.data);
          this.timeRemaining.set(response.data.durationMinutes * 60);
          this.startTimer();
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  private startTimer(): void {
    this.timer = setInterval(() => {
      const remaining = this.timeRemaining() - 1;
      this.timeRemaining.set(remaining);
      
      if (remaining <= 0) {
        this.submitTest();
      }
    }, 1000);
  }

  formatTime(seconds: number): string {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  }

  currentQuestion(): TestQuestion | null {
    const t = this.test();
    if (!t) return null;
    return t.questions[this.currentQuestionIndex()];
  }

  isOptionSelected(optionId: string): boolean {
    const q = this.currentQuestion();
    if (!q) return false;
    const selected = this.selectedOptions().get(q.questionId);
    return selected?.includes(optionId) ?? false;
  }

  selectOption(optionId: string): void {
    const q = this.currentQuestion();
    if (!q) return;

    const updated = new Map(this.selectedOptions());
    const current = updated.get(q.questionId) || [];
    
    if (current.includes(optionId)) {
      updated.set(q.questionId, current.filter(id => id !== optionId));
    } else {
      updated.set(q.questionId, [...current, optionId]);
    }
    
    this.selectedOptions.set(updated);
    this.updateProgress();
  }

  hasAnswer(questionId: string): boolean {
    const options = this.selectedOptions().get(questionId);
    const text = this.textAnswers()[questionId];
    return Boolean((options && options.length > 0) || (text && text.length > 0));
  }

  private updateProgress(): void {
    const t = this.test();
    if (!t) return;
    
    const answered = t.questions.filter(q => this.hasAnswer(q.questionId)).length;
    this.progress.set((answered / t.questions.length) * 100);
  }

  previousQuestion(): void {
    if (this.currentQuestionIndex() > 0) {
      this.currentQuestionIndex.update(i => i - 1);
    }
  }

  nextQuestion(): void {
    const t = this.test();
    if (t && this.currentQuestionIndex() < t.questions.length - 1) {
      this.currentQuestionIndex.update(i => i + 1);
    }
  }

  goToQuestion(index: number): void {
    this.currentQuestionIndex.set(index);
  }

  submitTest(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }

    const t = this.test();
    if (!t) return;

    const answers: AnswerItem[] = t.questions.map(q => {
      const selectedOptions = this.selectedOptions().get(q.questionId) || [];
      const textAnswer = this.textAnswers()[q.questionId];
      
      return {
        questionId: q.questionId,
        selectedOptionIds: selectedOptions,
        textAnswer: textAnswer || undefined
      };
    });

    this.portalApi.submitTest(t.testId, { answers }).subscribe({
      next: (response) => {
        if (!response.isError) {
          this.router.navigate(['/results', t.testId]);
        }
      },
      error: () => {
        this.router.navigate(['/dashboard']);
      }
    });
  }
}
