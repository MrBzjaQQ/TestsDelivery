import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionWizardComponent } from './question-wizard.component';

describe('QuestionWizardComponent', () => {
  let component: QuestionWizardComponent;
  let fixture: ComponentFixture<QuestionWizardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionWizardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
