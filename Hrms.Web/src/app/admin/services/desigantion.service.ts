import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DesignationModel } from '../company/models';
import { FilterModel } from 'src/app/shared/models';

@Injectable()
export class DesignationService {

  private apiEndPoint = environment.apiBaseUrl + '/designation';

  constructor(private http: HttpClient) { }

  addDesignation(model: DesignationModel): Observable<any> {
    return this.http.post(`${this.apiEndPoint}`, model);
  }

  getDesignation(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getDesignationList(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/list`);
  }

  pageList(model: FilterModel): Observable<any> {
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/page-list?${queryString}`);
  }

  getSelectListItem():Observable<any>{
    return this.http.get(`${this.apiEndPoint}/get-select-list-item`)
  }
  
  editDesignation(model: DesignationModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteDesignation(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }

}