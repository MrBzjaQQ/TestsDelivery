import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleTestsComponent } from './schedule-tests.component';

describe('ScheduleTestsComponent', () => {
  let component: ScheduleTestsComponent;
  let fixture: ComponentFixture<ScheduleTestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduleTestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleTestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
