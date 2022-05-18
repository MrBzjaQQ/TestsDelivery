import { TestBed } from '@angular/core/testing';

import { ScheduleTestsService } from './schedule-tests.service';

describe('ScheduleTestsService', () => {
  let service: ScheduleTestsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScheduleTestsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
