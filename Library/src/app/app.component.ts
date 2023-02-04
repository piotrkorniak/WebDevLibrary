import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthenticationService} from './shared/services/authentication.service';
import {User} from './shared/models/user';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  user: User;
  userSubscription: Subscription;

  constructor(private authenticationService: AuthenticationService) {
  }

  public ngOnInit(): void {
    this.authenticationService.autoLogin();
    this.userSubscription = this.authenticationService.user$.subscribe(user => {
      this.user = user;
    });
  }

   public ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
}
