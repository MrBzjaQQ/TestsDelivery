import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledTestsListComponent } from './scheduled-tests-list.component';

describe('ScheduledTestsListComponent', () => {
  let component: ScheduledTestsListComponent;
  let fixture: ComponentFixture<ScheduledTestsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduledTestsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduledTestsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
