import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ListFilter } from 'src/app/models/filters';
import { ScheduledTestInList } from 'src/app/models/scheduled-tests';
import { ScheduledTestsService } from 'src/app/services/scheduled-tests-service/scheduled-tests.service';

@Component({
  selector: 'app-scheduled-tests-list',
  templateUrl: './scheduled-tests-list.component.html',
  styleUrls: ['./scheduled-tests-list.component.scss']
})
export class ScheduledTestsListComponent implements OnInit {

  private _scheduledTests: ScheduledTestInList[] = [];
  private _totalCount: number = 0;
  private _listFilter: ListFilter = {
    take: 25,
    skip: 0,
    searchText: undefined
  };

  public readonly pageSizeOptions = [25, 50, 100];
  public readonly columnsToDisplay = [
    'testName',
    'keycode',
    'pin',
    'candidate',
    'duration',
    'startDate',
    'expirationDate',
    'controls'];

  public constructor(private _scheduledTestsService: ScheduledTestsService) { }

  public get scheduledTests() : ScheduledTestInList[] {
    return this._scheduledTests;
  }

  public ngOnInit(): void {
    this._loadScheduledTests();
  }

  public get totalCount(): number {
    return this._totalCount;
  }

  public openCreateTestPage(): void {
    console.log('TODO: open create test page');
  }

  public openScheduleTestsPage(): void {
    console.log('TODO: open schedule tests page');
  }

  public markTest(test: ScheduledTestInList): void {
    console.log('TODO: open mark page', test);
  }

  public viewResults(test: ScheduledTestInList): void {
    console.log('TODO: open results page', test);
  }

  public downloadResults(test: ScheduledTestInList): void {
    console.log('TODO: download results', test);
  }

  public voidTest(test: ScheduledTestInList): void {
    console.log('TODO: void test', test);
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._listFilter.take = value.pageSize;
    this._listFilter.skip = value.pageSize * value.pageIndex;
    this._loadScheduledTests();
  }

  private _loadScheduledTests(): void {
    this._scheduledTestsService.getScheduledTests(this._listFilter)
      .subscribe(response => {
        this._scheduledTests = [...response.scheduledTests];
        this._totalCount = response.totalCount;
      });
  }

}
