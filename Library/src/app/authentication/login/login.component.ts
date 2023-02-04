import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthenticationService } from '../../shared/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(private authenticationService: AuthenticationService) {}

  ngOnInit(): void {}

  public onLogin(loginForm: NgForm): void {
    if (loginForm.invalid) {
      loginForm.form.markAllAsTouched();
      return;
    }
    const values = loginForm.value;
    this.authenticationService.login(values.email, values.password).subscribe();
  }
}
