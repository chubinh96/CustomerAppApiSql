import { Component, OnInit } from '@angular/core';
import { Customer } from '../../models/customer';
import { Subscription } from 'rxjs/Subscription';
import { CustomerService } from '../../services/customer.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {
  public title = '';
  public status: boolean = true;
  public currentCustomer = new Customer();
  public subscription: Subscription;

  constructor(
    private customerService: CustomerService,
    private routeActive: Router,
    private activatedRoute: ActivatedRoute
    ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    var id = this.activatedRoute.snapshot.params["id"];
    if (id === 0 || id == null) {
      this.title = 'Add';
      this.status = false;
    } else {
      this.title = 'Edit';
      this.status = true;
      
      this.subscription = this.customerService.getCustomerById(id).subscribe((data) => {
        this.currentCustomer = data;
      }, error => console.log(error));
    }
  };

  onAdd() {
    this.subscription = this.customerService.addCustomer(this.currentCustomer).subscribe((data) => {
      this.routeActive.navigateByUrl('customer');
    }, error => console.error(error));
  };

  onEdit() {
    if (confirm("Do you want to update this customer ?")) {
      this.subscription = this.customerService.updateCustomerById(this.currentCustomer.id, this.currentCustomer).subscribe((data) => {
        this.routeActive.navigateByUrl('customer');
      }, error => console.error(error));
    }
  }

}
