import {NgModule} from '@angular/core';
import {Route, RouterModule} from '@angular/router';

import {BookListComponent} from './books/book-list/book-list.component';
import {BookDetailComponent} from './books/book-detail/book-detail.component';
import {LoginComponent} from './authentication/login/login.component';
import {RegisterComponent} from './authentication/register/register.component';
import {AuthenticationGuard} from './shared/guards/authentication.guard';
import {RentalsEmployeePanelComponent} from './employee/rentals-employee-panel/rentals-employee-panel.component';
import {RentalsUserPanelComponent} from './user/rentals-user-panel/rentals-user-panel.component';
import {BooksEmployeePanelAddComponent} from './employee/books/books-employee-panel-add/books-employee-panel-add.component';
import {BooksEmployeePanelListComponent} from './employee/books/books-employee-panel-list/books-employee-panel-list.component';
import {BookDetailResolver} from './books/book-detail/book-detail.resolver';

const routes: Route[] = [
  {
    path: 'authentication', canDeactivate: [AuthenticationGuard],
    canActivate: [AuthenticationGuard], children: [
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      {path: '', redirectTo: 'login', pathMatch: 'full'}
    ]
  },
  {
    path: 'books', children: [
      {
        path: ':id',
        component: BookDetailComponent,
        resolve: {
          bookDetail: BookDetailResolver
        }
      },
      {path: '', component: BookListComponent},
    ]
  },
  {
    path: 'employee', children: [
      {path: 'rentals', component: RentalsEmployeePanelComponent},
      {
        path: 'books', children: [
          {path: 'add', component: BooksEmployeePanelAddComponent},
          {path: 'list', component: BooksEmployeePanelListComponent},
          {path: '', redirectTo: 'add', pathMatch: 'full'}
        ]
      },
      {path: '', redirectTo: 'rentals', pathMatch: 'full'}
    ]
  },
  {
    path: 'user', children: [
      {path: 'rentals', component: RentalsUserPanelComponent},
      {path: '', redirectTo: 'rentals', pathMatch: 'full'}
    ]
  },
  {path: '', redirectTo: '/authentication/login', pathMatch: 'full'},
  {path: '**', redirectTo: '/authentication/login'}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
