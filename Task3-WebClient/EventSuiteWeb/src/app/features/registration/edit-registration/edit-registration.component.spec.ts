import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRegistrationComponent } from './edit-registration.component';

describe('EditRegistrationComponent', () => {
  let component: EditRegistrationComponent;
  let fixture: ComponentFixture<EditRegistrationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditRegistrationComponent]
    });
    fixture = TestBed.createComponent(EditRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
