import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComponentType } from 'src/app/models/components';
import { EssayModel } from 'src/app/models/components/questions-wizard';

@Component({
  selector: 'app-manage-essay',
  templateUrl: './manage-essay.component.html',
  styleUrls: ['./manage-essay.component.scss']
})
export class ManageEssayComponent implements OnInit {

  private _name: string = '';
  private _text: string = '';

  @Input() editModel?: EssayModel;
  @Input() componentType: ComponentType = 'create';
  @Output() save: EventEmitter<EssayModel> = new EventEmitter<EssayModel>();

  constructor() { }

  ngOnInit(): void {
  }

  public discardChanges() : void {

  }

  public onNameChanged(event: Event) : void {
    const element = event.target as HTMLInputElement;
    this._name = element.value;
  }

  public onTextChanged(event: Event) : void {
    const element = event.target as HTMLInputElement;
    this._text = element.value;
  }
}
