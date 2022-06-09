import { Component, OnInit } from '@angular/core';
import { MatSelectionListChange } from '@angular/material/list';
import { PageEvent } from '@angular/material/paginator';
import { MatSelectChange } from '@angular/material/select';
import { CandidatesList } from 'src/app/models/candidates';
import { ListFilter } from 'src/app/models/filters';
import { TestsList } from 'src/app/models/tests';
import { CandidatesService } from 'src/app/services/candidates-service/candidates.service';
import { ScheduleTestsService } from 'src/app/services/schedule-tests-service/schedule-tests.service';
import { TestPortalInstancesService } from 'src/app/services/test-portal-instances-service/test-portal-instances.service';
import { TestsService } from 'src/app/services/tests-service/tests.service';

@Component({
  selector: 'app-schedule-tests',
  templateUrl: './schedule-tests.component.html',
  styleUrls: ['./schedule-tests.component.scss']
})
export class ScheduleTestsComponent implements OnInit {

  private _selectedTestId: number = 0;
  private _selectedInstance: string = '';
  private _selectedCandidates: number[] = [];

  private _instances: string[] = [];
  private _candidates: CandidatesList = {
    candidates: [],
    totalCount: 0
  };
  private _tests: TestsList = {
    tests: [],
    totalCount: 0
  };
  private _testsListFilter: ListFilter = {
    // TODO: lazy load
    // take: 25,
    skip: 0,
    searchText: undefined
  };
  private _candidatesListFilter: ListFilter = {
    // TODO: paging
    // take: 25,
    skip: 0
  };

  public pageSizeOptions: number[] = [25, 50, 100];

  public constructor(
    private _scheduleService: ScheduleTestsService,
    private _candidatesService: CandidatesService,
    private _testsService: TestsService,
    private _portalInstancesService: TestPortalInstancesService) { }

  public ngOnInit(): void {
    // TODO: use chaining when adding loader and/or css styles
    this._getCandidates();
    this._getInstances();
    this._getTests();
  }

  public get candidates(): CandidatesList {
    return this._candidates;
  }

  public get tests(): TestsList {
    return this._tests;
  }

  public get instances(): string[] {
    return this._instances;
  }

  // TODO: paging
  public onPageEventChanged(value: PageEvent): void {
    this._candidatesListFilter.take = value.pageSize;
    this._candidatesListFilter.skip = value.pageSize * value.pageIndex;
    this._getCandidates();
  }

  public onTestChange(testId: number): void {
    this._selectedTestId = testId;
  }

  public onInstanceChange(instance: string): void {
    this._selectedInstance = instance;
  }

  public onCandidateSelected(event: MatSelectionListChange): void {
    this._selectedCandidates = event.options.map(x => x.value);
  }

  public submit(): void {
    this._scheduleService.scheduleTest({
      testId: this._selectedTestId,
      candidateIds: this._selectedCandidates,
      destinationInstance: this._selectedInstance,

      // TODO: add controls for hardcoded fields
      duration: 60,
      startDateTime: '2022-04-22T22:00:00.000Z',
      expirationDateTime: '2024-04-22T22:00:00.000Z'
    }).subscribe(x => {
      // TODO: change location
    });
  }

  private _getCandidates(): void {
    this._candidatesService.getCandidates(this._candidatesListFilter)
      .subscribe(response => {
        this._candidates = response;
      });
  }

  private _getInstances(): void {
    this._portalInstancesService.getInstances()
      .subscribe(response => {
        this._instances = response;
      });
  }

  private _getTests(): void {
    this._testsService.getTests(this._testsListFilter)
      .subscribe(response => {
        this._tests = response;
      });
  }

}
