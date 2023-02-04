export interface AuthenticationResponse {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  token: string;
  tokenExpirationDate: number;
}
