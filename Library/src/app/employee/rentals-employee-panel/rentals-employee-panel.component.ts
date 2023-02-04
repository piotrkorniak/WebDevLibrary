import {Component, OnInit} from '@angular/core';
import {EmployeeService} from '../../shared/services/employee.service';
import {Rental} from '../../shared/models/rental';
import {RentalStatus} from '../../shared/models/rental-status.enum';
import {BookStatus} from '../../shared/models/book-status.enum';

@Component({
  selector: 'app-rentals-employee-panel',
  templateUrl: './rentals-employee-panel.component.html',
  styleUrls: ['./rentals-employee-panel.component.scss']
})
export class RentalsEmployeePanelComponent implements OnInit {
  rentals: Rental[];
  rowGroupMetadata: any;
  rentalStatus = RentalStatus;

  constructor(private employeeService: EmployeeService) {
  }

  ngOnInit(): void {
    this.employeeService.getRentals().subscribe(rentalsResponse => {
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
      this.updateRowGroupMetaData();
    });
  }

  onSort(): void {
    this.updateRowGroupMetaData();
  }

  updateRowGroupMetaData(): void {
    this.rowGroupMetadata = {};

    if (this.rentals) {
      for (let i = 0; i < this.rentals.length; i++) {
        const rowData = this.rentals[i];
        const representativeName = rowData.rentee.name;

        if (i === 0) {
          this.rowGroupMetadata[representativeName] = {index: 0, size: 1};
        } else {
          const previousRowData = this.rentals[i - 1];
          const previousRowGroup = previousRowData.rentee.name;
          if (representativeName === previousRowGroup) {
            this.rowGroupMetadata[representativeName].size++;
          } else {
            this.rowGroupMetadata[representativeName] = {index: i, size: 1};
          }
        }
      }
    }
  }

  onCloseRental(rental: Rental): void {
    this.employeeService.closeRental(rental.id).subscribe(() => {
      this.employeeService.getRental(rental.id).subscribe(rentalResponse => {
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

  onActiveRental(rental: Rental): void {
    this.employeeService.activeRental(rental.id).subscribe(() => {
      this.employeeService.getRental(rental.id).subscribe(rentalResponse => {
        const updatedData: any = {
          id: rentalResponse.id,
          status: RentalStatus[rentalResponse.status]
        };
        const index = this.rentals.findIndex(x => x.id === rental.id);
        this.rentals[index].status = updatedData.status;
      });
    });
  }
}
