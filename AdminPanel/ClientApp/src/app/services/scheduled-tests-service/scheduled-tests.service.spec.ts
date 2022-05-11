import { TestBed } from '@angular/core/testing';

import { ScheduledTestsService } from './scheduled-tests.service';

describe('ScheduledTestsService', () => {
  let service: ScheduledTestsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScheduledTestsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
