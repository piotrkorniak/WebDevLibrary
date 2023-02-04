import {Injectable, Input} from '@angular/core';
import {ActivatedRoute, ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs';
import {BookService} from '../../shared/services/book.service';
import {BookResponse} from '../../shared/models/response/book-response';

@Injectable({
  providedIn: 'root'
})
export class BookDetailResolver implements Resolve<BookResponse> {
  constructor(private activatedRoute: ActivatedRoute, private bookService: BookService) {
  }

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<BookResponse> {
    const currentBookId = +route.params.id;
    return this.bookService.getBook(currentBookId);
  }
}
