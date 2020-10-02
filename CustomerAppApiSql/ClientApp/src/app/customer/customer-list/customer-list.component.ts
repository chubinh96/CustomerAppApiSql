import { Component, OnInit } from '@angular/core';
import { Customer } from '../../models/customer';
import { Subscription } from 'rxjs/Subscription';
import { CustomerService } from '../../services/customer.service';
import { error } from 'util';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  public title = '';
  public customers: Customer[];
  public subscription: Subscription;
  public noCustomer: boolean = false;

  //Sort Function
  optionSort = 'id';
  valueSort = 1;
  sortById = 1;
  sortByName = 1;
  sortByCountry = 1;

  constructor(
    private customerService: CustomerService,
    private activeRoute: Router
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.subscription = this.customerService.getAllCustomer().subscribe((data) => {
      this.customers = data;
      if (data.length == 0) {
        this.noCustomer = true;
      } else {
        this.noCustomer = false;
      }
    }, error => console.error(error));
  }

  onDelete(id: number) {
    if (confirm("Do you want to delete this customer ?")) {
      this.subscription = this.customerService.deleteById(id).subscribe((data) => {
        alert("Deleted !");
        this.reloadData();
      }, error => console.error(error));
    }
  }

  reloadData() {
    this.subscription = this.customerService.getAllCustomer().subscribe((data) => {
      this.customers = data;
      if (data.length == 0) {
        this.noCustomer = true;
      } else {
        this.noCustomer = false;
      }
    }, error => console.error(error));
  }

  onSearch(option: string, value: string) {
    if (value === "") {
      this.subscription = this.customerService.getAllCustomer().subscribe((data) => {
        this.customers = data;
        this.noCustomer = false;
      }, error => console.error(error));
    }
    else {
      this.subscription = this.customerService.searchCustomer(option, value).subscribe((data) => {
        this.customers = data; console.log(typeof data);
        console.log(data.length);
        if (data.length == 0) {
          this.noCustomer = true;
        } else {
          this.noCustomer = false;
        }
      }, error => console.error(error));
    }
  }

  onSort(option: string) {
    this.optionSort = option;
    if (option == 'id') {
      this.sortById = -this.sortById;
      this.valueSort = this.sortById;
    }
    if (option == 'name') {
      this.sortByName = -this.sortByName;
      this.valueSort = this.sortByName;
    }
    if (option == 'country') {
      this.sortByCountry = -this.sortByCountry;
      this.valueSort = this.sortByCountry;
    }
    this.subscription = this.customerService.sortCustomer(this.optionSort, this.valueSort).subscribe((data) => {
      this.customers = data;
    });
  }

}
