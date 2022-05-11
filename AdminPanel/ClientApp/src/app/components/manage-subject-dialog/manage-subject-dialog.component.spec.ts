import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageSubjectDialogComponent } from './manage-subject-dialog.component';

describe('ManageSubjectDialogComponent', () => {
  let component: ManageSubjectDialogComponent;
  let fixture: ComponentFixture<ManageSubjectDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageSubjectDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageSubjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
