import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEssayComponent } from './manage-essay.component';

describe('ManageEssayComponent', () => {
  let component: ManageEssayComponent;
  let fixture: ComponentFixture<ManageEssayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageEssayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageEssayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
