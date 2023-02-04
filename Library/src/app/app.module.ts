import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {AppRoutingModule} from './app-routing.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import {AppComponent} from './app.component';

import {HeaderComponent} from './header/header.component';

import {BookListComponent} from './books/book-list/book-list.component';
import {BookDetailComponent} from './books/book-detail/book-detail.component';

import {LoginComponent} from './authentication/login/login.component';
import {RegisterComponent} from './authentication/register/register.component';

import {RentalsEmployeePanelComponent} from './employee/rentals-employee-panel/rentals-employee-panel.component';
import {BooksEmployeePanelAddComponent} from './employee/books/books-employee-panel-add/books-employee-panel-add.component';
import {BooksEmployeePanelListComponent} from './employee/books/books-employee-panel-list/books-employee-panel-list.component';

import {RentalsUserPanelComponent} from './user/rentals-user-panel/rentals-user-panel.component';

import {ErrorsInterceptor} from './shared/interceptors/errors.interceptor';
import {AuthenticationInterceptor} from './shared/interceptors/authentication.interceptor';

import {PrimeNGModule} from './moduls/prime-ng.module';
import {TranslateConfigModule} from './moduls/translate-config.module';

@NgModule({
  declarations: [
    AppComponent,
    BookListComponent,
    BookDetailComponent,
    HeaderComponent,
    LoginComponent,
    RegisterComponent,
    RentalsEmployeePanelComponent,
    RentalsUserPanelComponent,
    BooksEmployeePanelAddComponent,
    BooksEmployeePanelListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    PrimeNGModule,
    TranslateConfigModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorsInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
