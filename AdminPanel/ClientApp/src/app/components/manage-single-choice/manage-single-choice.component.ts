import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatRadioChange } from '@angular/material/radio';
import { AnswerOption } from 'src/app/models/answerOptions';
import { ComponentType } from 'src/app/models/components';
import { ScqModel } from 'src/app/models/components/questions-wizard';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';

// TODO: CRITICAL - USE FORM!
@Component({
  selector: 'app-manage-single-choice',
  templateUrl: './manage-single-choice.component.html',
  styleUrls: ['./manage-single-choice.component.scss']
})
export class ManageSingleChoiceComponent implements OnInit {

  @Input() editModel?: ScqModel;
  @Input() componentType: ComponentType = 'create';
  @Output() save: EventEmitter<ScqModel> = new EventEmitter<ScqModel>();

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

  public onCorrectOptionChange(event: MatRadioChange) {
    this.answerOptions.forEach(x => x.isCorrect = false);
    const option = event.value as AnswerOption;
    option.isCorrect = event.source._inputElement.nativeElement.checked;
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

  public onNameChanged(event: Event) : void {
    const element = event.target as HTMLInputElement;
    this._name = element.value;
  }

  public onTextChanged(event: Event) : void {
    const element = event.target as HTMLInputElement;
    this._text = element.value;
  }

  public onAnswerTextChanged(event: Event, index: number) : void {
    const element = event.target as HTMLInputElement;
    this._answerOptions[index].text = element.value;
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
