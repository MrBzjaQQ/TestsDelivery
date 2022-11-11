import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ListFilter } from 'src/app/models/filters';
import { SubjectInListModel } from 'src/app/models/subjects';
import { SubjectsService } from 'src/app/services/subjects-service/subjects.service';
import { ManageSubjectDialogComponent } from '../manage-subject-dialog/manage-subject-dialog.component';

@Component({
  selector: 'app-subjects-list',
  templateUrl: './subjects-list.component.html',
  styleUrls: ['./subjects-list.component.scss']
})
export class SubjectsListComponent implements OnInit {

  private _subjects: SubjectInListModel[] = [];
  private _totalCount: number = 0;
  private _listFilter: ListFilter = {
    take: 25,
    skip: 0,
    searchText: undefined
  }; 

  public readonly pageSizeOptions = [25, 50, 100];
  public readonly columnsToDisplay = [ 'name', 'controls' ];

  constructor(
    public dialogService: MatDialog,
    private subjectsService: SubjectsService
  ) { }

  public get subjects() : SubjectInListModel[] {
    return this._subjects;
  }

  public get totalCount() : number {
    return this._totalCount;
  }

  public ngOnInit(): void {
    this._loadSubjects();
  }

  public openCreateDialog() : void {
    const dialogRef = this.dialogService.open(ManageSubjectDialogComponent, {
      data: {
        type: 'create'
      }
    });

    dialogRef.afterClosed().subscribe(this._loadSubjects.bind(this));
  }

  public openEditDialog(subject : SubjectInListModel) : void {
    const dialogRef = this.dialogService.open(ManageSubjectDialogComponent, {
      data: {
        type: 'edit',
        subject
      },
    });

    dialogRef.afterClosed().subscribe(this._loadSubjects.bind(this));
  }

  public manageQuestions(subject : SubjectInListModel): void {
    // TODO: manage questions
    console.log('Manage questions page is opened', subject);
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._listFilter.take = value.pageSize;
    this._listFilter.skip = value.pageSize * value.pageIndex;
    this._loadSubjects();
  }

  private _loadSubjects(): void {
    this.subjectsService.getSubjects(this._listFilter)
    .subscribe(response => {
      this._subjects = [...response.subjects];
      this._totalCount = response.totalCount;
    })
  }

}
