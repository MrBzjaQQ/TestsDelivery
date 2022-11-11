import { TestBed } from '@angular/core/testing';

import { TestPortalInstancesService } from './test-portal-instances.service';

describe('TestPortalInstancesService', () => {
  let service: TestPortalInstancesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TestPortalInstancesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
