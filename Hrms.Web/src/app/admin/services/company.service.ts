import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CompanyModel } from '../company/models';

@Injectable()
export class CompanyService {

  private apiEndPoint = environment.apiBaseUrl + '/company';

  constructor(private http: HttpClient) { }

  get(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  update(model: CompanyModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }
}