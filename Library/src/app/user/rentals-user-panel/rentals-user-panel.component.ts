import {Component, OnInit} from '@angular/core';
import {RentalStatus} from '../../shared/models/rental-status.enum';
import {Rental} from '../../shared/models/rental';
import {UserService} from '../../shared/services/user.service';
import {BookStatus} from '../../shared/models/book-status.enum';

@Component({
  selector: 'app-rentals-user-panel',
  templateUrl: './rentals-user-panel.component.html',
  styleUrls: ['./rentals-user-panel.component.scss']
})
export class RentalsUserPanelComponent implements OnInit {
  rentals: Rental[];
  rentalStatus = RentalStatus;

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
    this.userService.getRentals().subscribe(rentalsResponse => {
      this.rentals = rentalsResponse.map(rentalResponse => ({
        id: rentalResponse.id,
        status: RentalStatus[rentalResponse.status],
        startDate: new Date(rentalResponse.startDate * 1000),
        endDate: rentalResponse.endDate === null ? null : new Date(rentalResponse.endDate * 1000),
        book: {
          id: rentalResponse.book.id,
          title: rentalResponse.book.title,
          author: rentalResponse.book.author,
          description: rentalResponse.book.description,
          imageUrl: rentalResponse.book.imageUrl,
          status: BookStatus[rentalResponse.book.status]
        },
        rentee: {
          id: rentalResponse.rentee.id,
          email: rentalResponse.rentee.email,
          name: rentalResponse.rentee.lastName + ' ' + rentalResponse.rentee.firstName,
        }
      }));
      this.rentals = this.rentals.sort((x, y) => y.startDate.getTime() - x.startDate.getTime());
    });
  }

  public onCloseRental(rental: Rental): void {
    this.userService.closeRental(rental.id).subscribe(() => {
      this.userService.getRental(rental.id).subscribe(rentalResponse => {
        const updatedData: any = {
          id: rentalResponse.id,
          status: RentalStatus[rentalResponse.status],
          endDate: rentalResponse.endDate === null ? null : new Date(rentalResponse.endDate * 1000)
        };
        const index = this.rentals.findIndex(x => x.id === rental.id);
        this.rentals[index].endDate = updatedData.endDate;
        this.rentals[index].status = updatedData.status;
      });
    });
  }
}
