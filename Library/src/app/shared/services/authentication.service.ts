import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserRoles } from '../models/user-roles.enum';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthenticationResponse } from '../models/response/authentication-response';
import { CustomMessageService } from './custom-message.service';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  user$: BehaviorSubject<User> = new BehaviorSubject<User>(null);
  private tokenExpirationTimer: any;

  constructor(private http: HttpClient, private router: Router, private customMessageService: CustomMessageService) {}

  public register(
    firstName: string,
    lastName: string,
    email: string,
    password: string
  ): Observable<AuthenticationResponse> {
    return this.http
      .post<AuthenticationResponse>(`${environment.apiUrl}/authentication/register`, {
        firstName,
        lastName,
        email,
        password,
      })
      .pipe(
        tap((response) => {
          this.handleAuthentication(
            +response.id,
            response.email,
            response.firstName,
            response.lastName,
            UserRoles[response.role],
            response.token,
            new Date(+response.tokenExpirationDate * 1000)
          );
          this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.Register');
        })
      );
  }

  public login(email: string, password: string): Observable<AuthenticationResponse> {
    return this.http
      .post<AuthenticationResponse>(`${environment.apiUrl}/authentication/login`, { email, password })
      .pipe(
        tap((response) => {
          this.handleAuthentication(
            +response.id,
            response.email,
            response.firstName,
            response.lastName,
            UserRoles[response.role],
            response.token,
            new Date(+response.tokenExpirationDate * 1000)
          );
          this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.Login');
        })
      );
  }

  autoLogin(): void {
    const userDataInLocalStorage: {
      id: string;
      email: string;
      firstName: string;
      lastName: string;
      role: string;
      _token: string;
      _tokenExpirationDate: string;
    } = JSON.parse(localStorage.getItem('user'));

    if (!userDataInLocalStorage) {
      this.router.navigate(['authentication']);
      return;
    }

    const loadedUser = new User(
      +userDataInLocalStorage.id,
      userDataInLocalStorage.email,
      userDataInLocalStorage.firstName,
      userDataInLocalStorage.lastName,
      UserRoles[userDataInLocalStorage.role],
      userDataInLocalStorage._token,
      new Date(userDataInLocalStorage._tokenExpirationDate)
    );

    if (loadedUser.token) {
      this.user$.next(loadedUser);
      const expirationDuration = new Date(userDataInLocalStorage._tokenExpirationDate).getTime() - new Date().getTime();
      this.autoLogout(expirationDuration);
    } else {
      this.router.navigate(['authentication']);
      localStorage.removeItem('user');
    }
  }

  public logout(): void {
    this.router.navigate(['authentication']);
    this.user$.next(null);
    localStorage.removeItem('user');
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
    this.tokenExpirationTimer = null;
  }

  private autoLogout(expirationDuration: number): void {
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  private handleAuthentication(
    id: number,
    email: string,
    firstName: string,
    lastName: string,
    role: UserRoles,
    token: string,
    tokenExpirationDate: Date
  ): void {
    const user = new User(id, email, firstName, lastName, role, token, tokenExpirationDate);
    this.user$.next(user);
    this.router.navigate(['/books']);
    this.autoLogout(tokenExpirationDate.getTime() - new Date().getTime());
    localStorage.setItem('user', JSON.stringify(user));
  }
}
