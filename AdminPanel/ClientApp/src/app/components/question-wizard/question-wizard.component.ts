import { Component, OnInit } from '@angular/core';
import { QuestionType } from 'src/app/models/questions';

@Component({
  selector: 'app-question-wizard',
  templateUrl: './question-wizard.component.html',
  styleUrls: ['./question-wizard.component.scss']
})
export class QuestionWizardComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  public get type() : QuestionType {
    return QuestionType.SingleChoice;
  }

}
