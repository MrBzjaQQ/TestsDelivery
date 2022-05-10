import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { CandidateReadModel } from 'src/app/models/candidates';
import { ListFilter } from 'src/app/models/filters';
import { CandidatesService } from 'src/app/services/candidates-service/candidates.service';
import { ManageCandidateDialogComponent } from '../manage-candidate-dialog/manage-candidate-dialog.component';

@Component({
  selector: 'app-candidates-list',
  templateUrl: './candidates-list.component.html',
  styleUrls: ['./candidates-list.component.scss']
})
export class CandidatesListComponent implements OnInit {

  private _candidates : CandidateReadModel[] = [];
  private _totalCount : number = 0;
  private _listFilter: ListFilter = {
    take: 25,
    skip: 0,
    searchText: undefined
  };

  public readonly pageSizeOptions = [25, 50, 100];
  public readonly columnsToDisplay = [ 'firstName', 'lastName', 'email', 'controls' ];

  constructor(
    public dialogService: MatDialog,
    private candidateService: CandidatesService) {}

  public get candidates() : CandidateReadModel[] {
    return this._candidates;
  }

  public get totalCount() : number {
    return this._totalCount;
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._listFilter.take = value.pageSize;
    this._listFilter.skip = value.pageSize * value.pageIndex;
    this.loadCandidates();
  }

  ngOnInit(): void {
    this.loadCandidates();
  }

  public loadCandidates(): void {
    this.candidateService.getCandidates(this._listFilter)
    .subscribe(response => {
      this._candidates = [...response.candidates];
      this._totalCount = response.totalCount;
    });
  }

  public openCreateDialog() : void {
    const dialogRef = this.dialogService.open(ManageCandidateDialogComponent, {
      data: {
        type: 'create'
      }
    });

    dialogRef.afterClosed().subscribe(this.loadCandidates.bind(this));
  }

  public openEditDialog(candidate : CandidateReadModel) : void {
    const dialogRef = this.dialogService.open(ManageCandidateDialogComponent, {
      data: {
        type: 'edit',
        candidate
      } 
    });

    dialogRef.afterClosed().subscribe(this.loadCandidates.bind(this));
  }

}
