import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SubjectsDialogData } from 'src/app/models/dialogs';
import { SubjectsService } from 'src/app/services/subjects-service/subjects.service';

@Component({
  selector: 'app-manage-subject-dialog',
  templateUrl: './manage-subject-dialog.component.html',
  styleUrls: ['./manage-subject-dialog.component.scss']
})
export class ManageSubjectDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ManageSubjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SubjectsDialogData,
    private subjectsService: SubjectsService
  ) { }

  public ngOnInit(): void {
    const { subject } = this.data;

    if (subject) {
      this.form.setValue({
        name: subject.name
      });
    }
  }

  public form = new FormGroup({
    name: new FormControl('', [
      Validators.required
    ])
  });

  public submit() : void {
    if (!this.form.valid) {
      console.error('form is invalid');
      return;
    }

    switch (this.data.type) {
      case 'create' : {
        this.subjectsService.createSubject({
          name: this.form.value.name
        }).subscribe(this._onRequestEnded.bind(this));
        break;
      }
      case 'edit': {
        const { subject } = this.data;
        if (!subject) 
          throw new Error('Subject is required for editing');

        this.subjectsService.editSubject({
          id: subject.id,
          name: this.form.value.name
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
