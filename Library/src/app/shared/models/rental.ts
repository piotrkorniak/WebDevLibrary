import {Book} from './book';
import {Rentee} from './rentee';
import {RentalStatus} from './rental-status.enum';

export class Rental {
  constructor(
    public id: number,
    public status: RentalStatus,
    public startDate: Date,
    public endDate: Date,
    public book: Book,
    public rentee: Rentee) {
  }
}
