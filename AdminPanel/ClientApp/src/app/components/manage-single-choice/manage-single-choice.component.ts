import { Component, OnInit } from '@angular/core';
import { MatRadioChange } from '@angular/material/radio';
import { AnswerOption } from 'src/app/models/answerOptions';

@Component({
  selector: 'app-manage-single-choice',
  templateUrl: './manage-single-choice.component.html',
  styleUrls: ['./manage-single-choice.component.scss']
})
export class ManageSingleChoiceComponent implements OnInit {

  private _answerOptions : AnswerOption[] = [];
  public selectedIndex = 0;
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

  constructor() {
    this.discardChanges();
  }

  ngOnInit(): void {
  }

  public get answerOptions() {
    return this._answerOptions;
  }

  public onChange(event: MatRadioChange, index: number) {
    this.answerOptions[index].isCorrect = event.value;
  }

  public discardChanges() : void {
    this._answerOptions = [...this._defaultAnswerOptions];
  }

  public addAnswerOption() : void {
    this._answerOptions.push({
      text: '',
      isCorrect: false
    });
  }

  public deleteAnswerOption(idx: number) {
    this._answerOptions.splice(idx, 1);
  }

}
