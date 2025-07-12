import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LeaveRuleModel } from '../leave/models';
import { FilterModel } from 'src/app/shared/models';


@Injectable()
export class LeaveRuleService {

  private apiEndPoint = environment.apiBaseUrl + '/leaveRule';

  constructor(private http: HttpClient) { }

  addLeaveRule(model: LeaveRuleModel): Observable<any> {
    return this.http.post(`${this.apiEndPoint}`, model);
  }

  getRule(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getSelectListItem(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/get-select-list-item`);
  }

  updateRule(model: LeaveRuleModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteRule(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }

  getPagedList(model: FilterModel):Observable<any>{
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/paged-list?${queryString}`);
  }

}