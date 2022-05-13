import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CandidatesDialogData } from 'src/app/models/components';
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

  public ngOnInit(): void {
    const { candidate } = this.data;
    if (candidate)
      this.form.setValue({
        firstName: candidate.firstName,
        lastName: candidate.email,
        email: candidate.email
      });
  }

  public form = new FormGroup({
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
        this.candidateService.createCandidate({
          firstName: this.form.value.firstName,
          lastName: this.form.value.lastName,
          email: this.form.value.email
        }).subscribe(this._onRequestEnded.bind(this));

        break;
      }
      case 'edit': {
        const { candidate } = this.data;

        if (!candidate)
          throw new Error('Candidate is required for editing');

        this.candidateService.editCandidate({
          id: candidate.id,
          email: this.form.value.email,
          firstName: this.form.value.firstName,
          lastName: this.form.value.lastName
        }).subscribe(this._onRequestEnded.bind(this));

        break;
      }
      default:
        throw new Error('Unknown dialog type');
    }
  }

  private _onRequestEnded() {
    this.dialogRef.close();
  }

}
