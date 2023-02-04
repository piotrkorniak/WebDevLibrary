import {UserRoles} from './user-roles.enum';

export class User {

  constructor(
    public id: number,
    public email: string,
    public firstName: string,
    public lastName: string,
    public role: UserRoles,
    private _token: string,
    private _tokenExpirationDate: Date) {
  }

  get token(): any {
    if (!this._tokenExpirationDate || new Date() > this._tokenExpirationDate) {
      return null;
    }
    return this._token;
  }
}
