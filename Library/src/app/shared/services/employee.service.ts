import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {tap} from 'rxjs/operators';
import {CustomMessageService} from './custom-message.service';
import {RentalResponse} from '../models/response/rental-response';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient, private customMessageService: CustomMessageService) {
  }

  public deleteBook(bookId: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/employee/book/${bookId}`).pipe(
      tap(() => {
          this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.DeleteBook');
        }
      ));
  }

  public addBook(
    title: string,
    author: string,
    imageUrl: string,
    description: string): Observable<any> {
    return this.http.post(
      `${environment.apiUrl}/employee/book`,
      {title, author, imageUrl, description})
      .pipe(
        tap(() => {
          this.customMessageService.pushSuccessMessage(
            'Template.Success.Success',
            'Template.Success.AddBook');
        })
      );
  }

  public getRental(id: number): Observable<RentalResponse> {
    return this.http.get<RentalResponse>(`${environment.apiUrl}/employee/rental/${id}`);
  }

  public getRentals(): Observable<RentalResponse[]> {
    return this.http.get<RentalResponse[]>(`${environment.apiUrl}/employee/rental`);
  }

  public activeRental(rentalId: number): Observable<any> {
    return this.http.patch(`${environment.apiUrl}/employee/rental/${rentalId}/active`, '').pipe(
      tap(() => {
        this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.ActiveRental');
      })
    );
  }

  public closeRental(rentalId: number): Observable<any> {
    return this.http.patch(`${environment.apiUrl}/employee/rental/${rentalId}/close`, '').pipe(
      tap(() => {
        this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.CloseRental');
      })
    );
  }
}
