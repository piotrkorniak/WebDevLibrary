import {BookResponse} from './book-response';
import {RenteeResponse} from './rentee-response';

export interface RentalResponse {
  id: number;
  status: string;
  startDate: number;
  endDate: number;
  book: BookResponse;
  rentee: RenteeResponse;
}
