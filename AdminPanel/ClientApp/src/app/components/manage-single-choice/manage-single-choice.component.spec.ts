import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageSingleChoiceComponent } from './manage-single-choice.component';

describe('ManageSingleChoiceComponent', () => {
  let component: ManageSingleChoiceComponent;
  let fixture: ComponentFixture<ManageSingleChoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageSingleChoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageSingleChoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
