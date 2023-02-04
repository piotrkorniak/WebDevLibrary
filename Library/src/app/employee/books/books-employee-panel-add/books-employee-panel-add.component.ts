import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {EmployeeService} from '../../../shared/services/employee.service';

@Component({
  selector: 'app-books-employee-panel-add',
  templateUrl: './books-employee-panel-add.component.html',
  styleUrls: ['./books-employee-panel-add.component.scss']
})
export class BooksEmployeePanelAddComponent implements OnInit {
  addBookForm: FormGroup;

  constructor(private employeeService: EmployeeService) {
  }

  ngOnInit(): void {
    this.addBookForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      author: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      imageUrl: new FormControl('', [Validators.required, Validators.maxLength(1000)]),
      description: new FormControl('', [Validators.required, Validators.maxLength(1000)]),
    });
  }

  onSubmit(): void {
    if (this.addBookForm.invalid) {
      Object.keys(this.addBookForm.controls).forEach(key => {
        this.addBookForm.controls[key].markAsTouched();
      });
      return;
    }
    const title = this.addBookForm.value.title;
    const author = this.addBookForm.value.author;
    const imageUrl = this.addBookForm.value.imageUrl;
    const description = this.addBookForm.value.description;

    this.employeeService.addBook(title, author, imageUrl, description).subscribe();
  }
}
