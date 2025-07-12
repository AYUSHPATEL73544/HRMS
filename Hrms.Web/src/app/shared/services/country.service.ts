import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SelectListItemModel } from '../models';

@Injectable()
export class CountryService {
  private apiEndPoint = environment.apiBaseUrl + '/country';

  constructor(private http: HttpClient) { }

  getSelectListItem(): Observable<Array<SelectListItemModel>> {
    return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items`);
  }
}