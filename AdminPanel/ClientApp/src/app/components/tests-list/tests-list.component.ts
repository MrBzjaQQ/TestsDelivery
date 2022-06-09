import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ListFilter } from 'src/app/models/filters';
import { TestInListModel } from 'src/app/models/tests';
import { TestsService } from 'src/app/services/tests-service/tests.service';

@Component({
  selector: 'app-tests-list',
  templateUrl: './tests-list.component.html',
  styleUrls: ['./tests-list.component.scss']
})
export class TestsListComponent implements OnInit {

  private _tests: TestInListModel[] = [];
  private _totalCount: number = 0;
  private _listFilter: ListFilter = {
    take: 25,
    skip: 0,
    searchText: undefined
  };

  public readonly pageSizeOptions = [25, 50, 100];
  public readonly columnsToDisplay = [ 'name', 'controls' ]

  public constructor(
    private _testsService: TestsService
  ) { }

  public ngOnInit(): void {
    this._loadTests();
  }

  public get tests() : TestInListModel[] {
    return this._tests;
  }

  public get totalCount() : number {
    return this._totalCount;
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._listFilter.take = value.pageSize;
    this._listFilter.skip = value.pageSize * value.pageIndex;
    this._loadTests();
  }

  public openCreatePage() : void {
    console.log('TODO: open create test page.');
  }

  public openEditPage(test: TestInListModel) : void {
    console.log('TODO: open edit test page.', test);
  }

  private _loadTests() : void {
    this._testsService.getTests(this._listFilter)
      .subscribe(response => {
        this._tests = [...response.tests];
        this._totalCount = response.totalCount;
      });
  }

}
