import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';

import {MenuItem} from 'primeng/api';
import {AuthenticationService} from '../shared/services/authentication.service';
import {TranslateService} from '@ngx-translate/core';
import {User} from '../shared/models/user';
import {UserRoles} from '../shared/models/user-roles.enum';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  user: User;
  searchForm: FormGroup;
  authorizedUserItems: MenuItem[];
  unauthorizedUserItems: MenuItem[];

  constructor(
    private authenticationService: AuthenticationService,
    private translateService: TranslateService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.authenticationService.user$.subscribe(user => {
      this.user = user;
      this.updateAuthorizedUserItems();
      this.updateUnauthorizedUserItems();
    });

    this.searchForm = new FormGroup({
      searchValue: new FormControl(null)
    });

    this.activatedRoute.queryParams.subscribe(x => {
      this.searchForm = new FormGroup({
        searchValue: new FormControl(x.searchValue ?? '')
      });
    });
  }

  onSearch(): void {
    const searchValue = this.searchForm.value.searchValue;
    this.router.navigate(['/books'], {queryParams: {searchValue}});
  }

  public onLogout(): void {
    this.authenticationService.logout();
  }

  private updateAuthorizedUserItems(): void {
    this.authorizedUserItems = [
      {
        label: this.translateService.instant('Template.Header.Home'),
        icon: 'pi pi-fw pi-home',
        routerLink: '/'
      },
      !!this.user ? (this.user.role === UserRoles.User ? this.setUserMenuItem() : this.setEmployeeMenuItem()) : null,
      this.setTranslateMenuItem(),
    ];
  }

  private updateUnauthorizedUserItems(): void {
    this.unauthorizedUserItems = [
      this.setTranslateMenuItem()
    ];
  }

  public setTranslateMenuItem(): MenuItem {
    return {
      label: this.translateService.instant('Template.Header.Language'),
      icon: 'pi pi-fw pi-cog',
      items: [
        {
          label: this.translateService.instant('Template.Header.English'),
          command: () => {
            this.translateService.use('en').subscribe(() => {
              this.updateAuthorizedUserItems();
              this.updateUnauthorizedUserItems();
            });
          },
          disabled: this.translateService.currentLang === 'en'
        },
        {
          label: this.translateService.instant('Template.Header.Polish'),
          command: () => {
            this.translateService.use('pl').subscribe(() => {
              this.updateAuthorizedUserItems();
              this.updateUnauthorizedUserItems();
            });
          },
          disabled: this.translateService.currentLang === 'pl'
        }
      ]
    };
  }

  public setUserMenuItem(): MenuItem {
    return {
      label: this.translateService.instant('Template.Header.Rentals'),
      routerLink: '/user/rentals'
    };
  }

  public setEmployeeMenuItem(): MenuItem {
    return {
      label: this.translateService.instant('Template.Header.Panel'),
      icon: 'pi pi-fw pi-user',
      items: [
        {
          label: this.translateService.instant('Template.Header.Rentals'),
          routerLink: '/employee/rentals'
        },
        {
          label: this.translateService.instant('Template.Header.Books'),
          icon: 'pi pi-fw pi-book',
          items: [
            {
              label: this.translateService.instant('Template.Header.AddBook'),
              routerLink: '/employee/books/add'
            },
            {
              label: this.translateService.instant('Template.Header.ListBooks'),
              routerLink: '/employee/books/list'
            }
          ]
        }
      ]
    };
  }
}
