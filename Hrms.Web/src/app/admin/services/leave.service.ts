import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LeaveModel, LeaveRuleModel } from '../leave/models';
import { FilterModel } from 'src/app/shared/models';


@Injectable()
export class LeaveService {

  private apiEndPoint = environment.apiBaseUrl + '/leave';

  constructor(private http: HttpClient) { }

  getLeaveList(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/list`);
  }

  addLeaveRule(model: LeaveModel): Observable<any> {
    return this.http.post(`${this.apiEndPoint}`, model);
  }

  pagedList(filterModel: FilterModel): Observable<any> {
    const queryString = filterModel.toQueryString();
    return this.http.get(`${this.apiEndPoint}/paged-list?${queryString}`);
  }

  AssignRuleList(filterModel: FilterModel): Observable<any> {
    const queryString = filterModel.toQueryString();
    return this.http.get(`${this.apiEndPoint}/assign-list?${queryString}`);
  }

  inActivepagedList(filterModel: FilterModel): Observable<any> {
    const queryString = filterModel.toQueryString();
    return this.http.get(`${this.apiEndPoint}/inactive-assign-list?${queryString}`);
  }


  getRule(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  updateRule(model: LeaveRuleModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteRule(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }

  getBalance(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/get-leave-balance`);
  }

  deleteLeave (employeeId :number, ruleId: number):Observable<any>{
    return this.http.delete(`${this.apiEndPoint}/${employeeId}/${ruleId}`);
  }
}