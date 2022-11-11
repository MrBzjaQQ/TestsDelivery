import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageCandidateDialogComponent } from './manage-candidate-dialog.component';

describe('ManageCandidateDialogComponent', () => {
  let component: ManageCandidateDialogComponent;
  let fixture: ComponentFixture<ManageCandidateDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageCandidateDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageCandidateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
