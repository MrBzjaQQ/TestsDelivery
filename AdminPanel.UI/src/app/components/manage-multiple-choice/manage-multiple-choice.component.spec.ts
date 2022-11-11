import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageMultipleChoiceComponent } from './manage-multiple-choice.component';

describe('ManageMultipleChoiceComponent', () => {
  let component: ManageMultipleChoiceComponent;
  let fixture: ComponentFixture<ManageMultipleChoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageMultipleChoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageMultipleChoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
