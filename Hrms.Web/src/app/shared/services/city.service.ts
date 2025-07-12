import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SelectListItemModel } from '../models';

@Injectable()
export class CityService {
  private apiEndPoint = environment.apiBaseUrl + '/city';

  constructor(private http: HttpClient) { }

  getSelectListItem(stateId: number): Observable<Array<SelectListItemModel>> {
    return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items?stateId=${stateId}`);
  }
}