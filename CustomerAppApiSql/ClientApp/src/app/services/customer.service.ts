import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Customer } from '../models/customer';

@Injectable()
export class CustomerService {

  public baseUrl = '';

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') url: string
  ) {
    this.baseUrl = url
  }

  getAllCustomer(): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.baseUrl + 'api/Customer');
  }

  getCustomerById(id: number): Observable<Customer> {
    return this.http.get<Customer>(this.baseUrl + 'api/Customer/ ' + id);
  }

  updateCustomerById(id: number, customer: Customer): Observable<Customer> {
    return this.http.put<Customer>(this.baseUrl + 'api/Customer/' + id, customer);
  }

  addCustomer(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>(this.baseUrl + 'api/Customer', customer);
  }

  deleteById(id: number): Observable<Customer> {
    return this.http.delete<Customer>(this.baseUrl + 'api/Customer/' + id);
  }

  searchCustomer(option: string, value: string): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.baseUrl + 'api/Customer/SearchCustomer/' + option +  '/' + value);
  }

  sortCustomer(option: string, value: number): Observable<Customer[]> {
    return this.http.get<Customer[]>(this.baseUrl + 'api/Customer/SortCustomer/' + option + '/' + value);
  }

}
