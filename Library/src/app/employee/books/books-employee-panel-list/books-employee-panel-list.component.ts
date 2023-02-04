import {Component, OnInit} from '@angular/core';
import {Book} from '../../../shared/models/book';
import {BookService} from '../../../shared/services/book.service';
import {BookStatus} from '../../../shared/models/book-status.enum';

@Component({
  selector: 'app-books-employee-panel-list',
  templateUrl: './books-employee-panel-list.component.html',
  styleUrls: ['./books-employee-panel-list.component.scss']
})
export class BooksEmployeePanelListComponent implements OnInit {
  books: Book[];

  constructor(private bookService: BookService) {
  }

  ngOnInit(): void {
    this.bookService.getBooks('', null).subscribe(booksResponse => {
      this.books = booksResponse.map(bookResponse => ({
        id: bookResponse.id,
        title: bookResponse.title,
        author: bookResponse.author,
        description: bookResponse.description,
        imageUrl: bookResponse.imageUrl,
        status: BookStatus[bookResponse.status]
      }));
    });
  }

}
