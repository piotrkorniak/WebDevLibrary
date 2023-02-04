import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {BookResponse} from '../models/response/book-response';

@Injectable({
  providedIn: 'root'
})

export class BookService {
  constructor(private http: HttpClient) {
  }

  static createQueryParams(object: any): HttpParams {
    let params = new HttpParams();

    for (const [key, value] of Object.entries(object)) {
      if (!value) {
        continue;
      }
      params = params.append(key, String(value));
    }

    return params;
  }

  public getBooks(globalSearch: string, isAvailable: boolean): Observable<BookResponse[]> {
    const queryParamsObject = {
      globalSearch,
      isAvailable
    };

    return this.http.get<BookResponse[]>(`${environment.apiUrl}/book`, {
      params: BookService.createQueryParams(queryParamsObject)
    });
  }

  public getBook(bookId: number): Observable<BookResponse> {
    return this.http.get<BookResponse>(
      `${environment.apiUrl}/book/${bookId}`
    );
  }
}
