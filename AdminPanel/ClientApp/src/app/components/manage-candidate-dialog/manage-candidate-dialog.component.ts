import { Component, Input, OnInit, Output, EventEmitter, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { CandiadteEditModel, CandidateCreateModel, CandidateReadModel } from 'src/app/models/candidates';
import { CandidatesDialogData, DialogType } from 'src/app/models/dialogs';
import { CandidatesService } from 'src/app/services/candidates-service/candidates.service';

@Component({
  selector: 'app-manage-candidate-dialog',
  templateUrl: './manage-candidate-dialog.component.html',
  styleUrls: ['./manage-candidate-dialog.component.scss']
})
export class ManageCandidateDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ManageCandidateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CandidatesDialogData,
    private candidateService: CandidatesService) { }

  ngOnInit(): void {
    const { candidate } = this.data;
    if (candidate)
      this.form.setValue({
        firstName: candidate.firstName,
        lastName: candidate.email,
        email: candidate.email
      });
  }

  form = new FormGroup({
    firstName: new FormControl('', [
      Validators.required
    ]),
    lastName: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ])
  })

  public submit(): void {
    if (!this.form.valid) {
      console.error('form is invalid');
      return;
    }

    switch (this.data.type) {
      case 'create': {
        this.createCandidate({
          firstName: this.form.value.firstName,
          lastName: this.form.value.lastName,
          email: this.form.value.email
        }).subscribe(this.onRequestEnded.bind(this));

        break;
      }
      case 'edit': {
        const { candidate } = this.data;

        if (!candidate)
          throw new Error('Candidate is required for editing');

        this.editCandidate({
          id: candidate.id,
          email: this.form.value.email,
          firstName: this.form.value.firstName,
          lastName: this.form.value.lastName
        }).subscribe(this.onRequestEnded.bind(this));

        break;
      }
      default:
        throw new Error('Unknown dialog type');
    }
  }

  private createCandidate(candidateModel: CandidateCreateModel): Observable<CandidateReadModel> {
    return this.candidateService.createCandidate(candidateModel);
  }

  private editCandidate(candidateModel: CandiadteEditModel): Observable<any> {
    return this.candidateService.editCandidate(candidateModel);
  }

  private onRequestEnded() {
    this.dialogRef.close();
  }

}
