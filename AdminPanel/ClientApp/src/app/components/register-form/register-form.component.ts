import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth-service/auth.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  form = new FormGroup({
    userName: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.email,
      Validators.required
    ]),
    password: new FormControl('', [
      Validators.required
    ]),
    passwordConfirm: new FormControl('', [
      Validators.required,
    ])
  });

  submit(): void {
    if (this.form.valid) {
      this.authService.Register(this.form.value);
    } else {
      // TODO: validation
      console.error('form is invalid');
    }
  }
}
