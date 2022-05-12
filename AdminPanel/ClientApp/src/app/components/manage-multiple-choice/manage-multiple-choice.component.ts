import { Component, OnInit } from '@angular/core';
import { AnswerOption } from 'src/app/models/answerOptions';

@Component({
  selector: 'app-manage-multiple-choice',
  templateUrl: './manage-multiple-choice.component.html',
  styleUrls: ['./manage-multiple-choice.component.scss']
})
export class ManageMultipleChoiceComponent implements OnInit {

  private readonly _defaultAnswerOptions : AnswerOption[] = [
    {
      isCorrect: true,
      text: '',
    },
    {
      isCorrect: false,
      text: ''
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

  public get answerOptions() {
    return this._defaultAnswerOptions;
  }

}
