import {Component, OnDestroy, OnInit} from '@angular/core';
import {Book} from '../../shared/models/book';
import {ActivatedRoute, Data} from '@angular/router';
import {BookService} from '../../shared/services/book.service';
import {UserService} from '../../shared/services/user.service';
import {AuthenticationService} from '../../shared/services/authentication.service';
import {User} from '../../shared/models/user';
import {Subscription} from 'rxjs';
import {UserRoles} from '../../shared/models/user-roles.enum';
import {BookStatus} from '../../shared/models/book-status.enum';

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.scss']
})
export class BookDetailComponent implements OnInit, OnDestroy {
  currentBook: Book;
  user: User;
  authenticationSubscription: Subscription;
  userRoles = UserRoles;
  bookStatus = BookStatus;

  constructor(
    private activatedRoute: ActivatedRoute,
    private bookService: BookService,
    private userService: UserService,
    private authenticationService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.authenticationSubscription = this.authenticationService.user$.subscribe(user => this.user = user);
    this.activatedRoute.data.subscribe((data: Data) => {
      this.currentBook = new Book(
        data.bookDetail.id,
        data.bookDetail.title,
        data.bookDetail.author,
        data.bookDetail.description,
        data.bookDetail.imageUrl,
        BookStatus[data.bookDetail.status]);
    });
  }

  public ngOnDestroy(): void {
    this.authenticationSubscription.unsubscribe();
  }

  public onReserveBook(): void {
    this.userService.reserveBook(this.currentBook.id).subscribe(() => {
      this.bookService.getBook(this.currentBook.id).subscribe(bookResponse => {
        const updatedData: any = {
          status: BookStatus[bookResponse.status]
        };
        this.currentBook.status = updatedData.status;
      });
    });
  }
}
