import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AnswerOption } from 'src/app/models/answerOptions';
import { ComponentType } from 'src/app/models/components';
import { McqModel } from 'src/app/models/components/questions-wizard';

// TODO: CRITICAL - USE FORM!
@Component({
  selector: 'app-manage-multiple-choice',
  templateUrl: './manage-multiple-choice.component.html',
  styleUrls: ['./manage-multiple-choice.component.scss']
})
export class ManageMultipleChoiceComponent implements OnInit, OnChanges {

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

  public ngOnChanges(changes: SimpleChanges): void {
    this.discardChanges();
  }

  public get answerOptions() {
    return this._answerOptions;
  }
  
  public get name() : string {
    return this._name;
  }

  public get text() : string {
    return this._text;
  }

  public onCorrectOptionChange(event: MatCheckboxChange, index: number) {
    this._answerOptions[index].isCorrect = event.source._inputElement.nativeElement.checked;
  }

  public discardChanges() : void {
    switch(this.componentType) {
      case 'create': {
        this._discardCreateChanges();
        break;
      }
      case 'edit': {
        this._discardEditChanges();
        break;
      }
      default: 
        throw new Error('Unknown component type');
    }
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

  private _discardCreateChanges() : void {
    this._text = '';
    this._name = '';
    this._answerOptions = [...this._defaultAnswerOptions];
  }

  private _discardEditChanges() : void {
    if (!this.editModel) {
      console.error('Edit model should be passed for edit mode');
      return;
    }
      
    this._text = this.editModel.text;
    this._name = this.editModel.name;
    this._answerOptions = this.editModel.answerOptions;
  }

}
