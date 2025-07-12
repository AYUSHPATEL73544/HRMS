import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EmployeeModel } from '../profile/models';
import { FilterModel } from 'src/app/shared/models';

@Injectable()
export class EmployeeService {

  private apiEndPoint = environment.apiBaseUrl + '/employee';

  constructor(private http: HttpClient) { }

  pageList(model: FilterModel, employeeStatus: number): Observable<any> {
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/page-list?${queryString}&employeeStatus=${employeeStatus}`);
  }

  getEmployee(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getCalendarEvent(year: number, month: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/calendar-event/${year}/${month}`);
  }

  getCandidateList(model: FilterModel) : Observable<any>{
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/get-candidate-list?${queryString}`)
  }

  update(model: EmployeeModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }

  getByUserId(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}`);
  }


  
}