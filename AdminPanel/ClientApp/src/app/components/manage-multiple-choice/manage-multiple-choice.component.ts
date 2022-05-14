import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AnswerOption } from 'src/app/models/answerOptions';
import { ComponentType } from 'src/app/models/components';
import { McqModel } from 'src/app/models/components/questions-wizard';

@Component({
  selector: 'app-manage-multiple-choice',
  templateUrl: './manage-multiple-choice.component.html',
  styleUrls: ['./manage-multiple-choice.component.scss']
})
export class ManageMultipleChoiceComponent implements OnInit {

  @Input() editModel?: McqModel;
  @Input() componentType: ComponentType = 'create';
  @Output() save: EventEmitter<McqModel> = new EventEmitter<McqModel>();

  private _answerOptions : AnswerOption[] = [];
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
  
  private _name: string = '';
  private _text: string = '';

  public constructor() {
  }

  public ngOnInit(): void {
    this.discardChanges();
  }

  public get answerOptions() {
    return this._answerOptions;
  }

  public onCorrectOptionChange(event: MatCheckboxChange, index: number) {
    this._answerOptions[index].isCorrect = event.source._inputElement.nativeElement.checked;
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

  public onNameChanged(event: InputEvent) : void {
    const element = event.target as HTMLInputElement;
    this._name = element.value;
  }

  public onTextChanged(event: InputEvent) : void {
    const element = event.target as HTMLInputElement;
    this._text = element.value;
  }

  public deleteAnswerOption(idx: number) : void {
    this._answerOptions.splice(idx, 1);
  }

  public saveChanges() : void {
    this.save.emit({
      name: this._name,
      text: this._text,
      answerOptions: this.answerOptions
    });
  }

}
