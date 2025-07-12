import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SelectListItemModel } from 'src/app/shared/models';
import { environment } from 'src/environments/environment';
import { DepartmentModel } from '../company/models';


@Injectable()
export class DepartmentService {

  private apiEndPoint = environment.apiBaseUrl + '/department';

  constructor(private http: HttpClient) { }

  addDepartment(model: DepartmentModel): Observable<any> {
    return this.http.post(`${this.apiEndPoint}`, model);
  }

  getDepartment(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getDepartmentList(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/list`);
  }
  
  getSelectListItem():Observable<any>{
    return this.http.get(`${this.apiEndPoint}/get-select-list-item`)
  }

  editDepartment(model: DepartmentModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteDepartment(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }
}