import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthenticationService} from '../services/authentication.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService) {
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler):
    Observable<HttpEvent<unknown>> {
    const user = this.authenticationService.user$.value;
    if (!user) {
      return next.handle(request);
    }
    const modifiedReq = request.clone({
      headers: new HttpHeaders().append(
        'Authorization',
        `Bearer ${user.token}`)
    });
    return next.handle(modifiedReq);
  }
}

// return this.authenticationService.user$.pipe(
//   take(1),
//   exhaustMap(user => {
//     if (!user) {
//       return next.handle(request);
//     }
//     const modifiedReq = request.clone({
//       headers: new HttpHeaders().append(
//         'Authorization',
//         `Bearer ${user.token}`)
//     });
//     return next.handle(modifiedReq);
//   })
// );
