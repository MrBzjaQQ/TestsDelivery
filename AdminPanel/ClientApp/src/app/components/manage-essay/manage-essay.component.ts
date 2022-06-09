import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ComponentType } from 'src/app/models/components';
import { EssayModel } from 'src/app/models/components/questions-wizard';

// TODO: CRITICAL - USE FORM!
@Component({
  selector: 'app-manage-essay',
  templateUrl: './manage-essay.component.html',
  styleUrls: ['./manage-essay.component.scss']
})
export class ManageEssayComponent implements OnInit, OnChanges {

  private _name: string = '';
  private _text: string = '';

  @Input() editModel?: EssayModel;
  @Input() componentType: ComponentType = 'create';
  @Output() save: EventEmitter<EssayModel> = new EventEmitter<EssayModel>();

  constructor() { }

  public ngOnInit(): void {
    this.discardChanges();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.discardChanges();
  }

  public get name() : string {
    return this._name;
  }

  public get text() : string {
    return this._text;
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

  public saveChanges() : void {
    this.save.emit({
      name: this._name,
      text: this._text
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

  private _discardCreateChanges() : void {
    this._text = '';
    this._name = '';
  }

  private _discardEditChanges() : void {
    if (!this.editModel) {
      console.error('Edit model should be passed for edit mode');
      return;
    }
      
    this._text = this.editModel.text;
    this._name = this.editModel.name;
  }

}
