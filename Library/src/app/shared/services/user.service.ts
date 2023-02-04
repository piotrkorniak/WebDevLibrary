import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {CustomMessageService} from './custom-message.service';
import {environment} from '../../../environments/environment';
import {tap} from 'rxjs/operators';
import {RentalResponse} from '../models/response/rental-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private customMessageService: CustomMessageService) {
  }

  public reserveBook(id: number): Observable<any> {
    const bookId: any = {bookId: id};
    return this.http.post(`${environment.apiUrl}/user/rental`, bookId).pipe(
      tap(() => {
        this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.ReserveBook');
      })
    );
  }

  public getRental(id: number): Observable<RentalResponse> {
    return this.http.get<RentalResponse>(`${environment.apiUrl}/user/rental/${id}`);
  }

  public getRentals(): Observable<RentalResponse[]> {
    return this.http.get<RentalResponse[]>(`${environment.apiUrl}/user/rental`);
  }

  public closeRental(rentalId: number): Observable<any> {
    return this.http.patch(`${environment.apiUrl}/user/rental/${rentalId}/close`, '').pipe(
      tap(() => {
        this.customMessageService.pushSuccessMessage('Template.Success.Success', 'Template.Success.CloseRental');
      })
    );
  }
}
