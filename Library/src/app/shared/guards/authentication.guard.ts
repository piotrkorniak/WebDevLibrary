import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, CanDeactivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthenticationService} from '../services/authentication.service';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate, CanDeactivate<unknown> {
  constructor(private authenticationService: AuthenticationService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authenticationService.user$.pipe(
      map(user => !user),
      tap(isAccessAllowed => {
        if (!isAccessAllowed) {
          this.router.navigate(['/books']);
          console.warn('AuthenticationGuard: cannot active');
        }
      })
    );
  }

  canDeactivate(
    component: unknown,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authenticationService.user$.pipe(
      map(user => !!user),
      tap(isAccessAllowed => {
        if (!isAccessAllowed) {
          console.warn('AuthenticationGuard: cannot deactivate');
        }
      })
    );
  }
}
