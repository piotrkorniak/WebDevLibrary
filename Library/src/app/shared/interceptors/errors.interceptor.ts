import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {CustomMessageService} from '../services/custom-message.service';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {

  constructor(private customMessageService: CustomMessageService) {
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorCode;
        if (error.status === 401) {
          errorCode = 'ApiError.Common.Unauthorized';
        } else if (error.status === 403) {
          errorCode = 'ApiError.Common.Forbidden';
        } else if (!error.error.Code) {
          errorCode = 'ApiError.Common.InternalError';
        } else {
          errorCode = error.error.Code;
        }
        this.customMessageService.pushErrorMessage(
          'Template.Error',
          errorCode);
        return throwError(errorCode);
      }),
    );
  }
}
