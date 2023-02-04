import {Component, OnInit} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {AuthenticationService} from '../../shared/services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(private authenticationService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      lastName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(100)]),
      passwordGroup: new FormGroup({
        password: new FormControl('', Validators.minLength(8)),
        confirmPassword: new FormControl('', Validators.minLength(8)),
      }, this.passwordMatchValidator()),
    });
  }

  passwordMatchValidator(): ValidatorFn {
    return (passwordGroup: AbstractControl): ValidationErrors | null => {
      const password = passwordGroup.value.password;
      const confirmPassword = passwordGroup.value.confirmPassword;

      return password === confirmPassword ? null : {passwordMismatch: true};
    };
  }

  public onRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    const values = this.registerForm.value;
    if (values.passwordGroup.password !== values.passwordGroup.confirmPassword) {
      throw new Error('Passwords are different!');
    }

    this.authenticationService.register(values.firstName, values.lastName, values.email, values.passwordGroup.password).subscribe();
  }
}
