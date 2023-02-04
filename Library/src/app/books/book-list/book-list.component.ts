import {Component, OnInit} from '@angular/core';
import {BookService} from '../../shared/services/book.service';
import {Book} from '../../shared/models/book';
import {ActivatedRoute} from '@angular/router';
import {BookStatus} from '../../shared/models/book-status.enum';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];

  constructor(private bookService: BookService, private activatedRoute: ActivatedRoute) {
  }

  public ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(x => {
      const searchValue = x.searchValue ?? '';

      this.bookService.getBooks(searchValue, true).subscribe(booksResponse => {
        this.books = booksResponse.map(bookResponse => ({
          id: bookResponse.id,
          title: bookResponse.title,
          author: bookResponse.author,
          description: bookResponse.description,
          imageUrl: bookResponse.imageUrl,
          status: BookStatus[bookResponse.status]
        }));
      });
    });
  }
}
