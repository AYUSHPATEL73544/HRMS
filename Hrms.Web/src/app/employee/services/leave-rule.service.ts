import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FilterModel } from 'src/app/shared/models';
import { environment } from 'src/environments/environment';


@Injectable()
export class LeaveRuleService {

  private apiEndPoint = environment.apiBaseUrl + '/leaveRule';

  constructor(private http: HttpClient) { }

  getPageList(model: FilterModel): Observable<any> {
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/list?${queryString}`);
}

  getRule(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getSelectListItem(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/get-select-list-item`);
  }
}