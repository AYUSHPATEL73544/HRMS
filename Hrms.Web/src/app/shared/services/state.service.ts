import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SelectListItemModel } from 'src/app/shared/models/select-list-item.model';
import { Observable } from 'rxjs';

@Injectable()
export class StateService {
  private apiEndPoint = environment.apiBaseUrl + '/state';

  constructor(private http: HttpClient) { }


  getSelectListItem(countryId: number): Observable<Array<SelectListItemModel>> {
    return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items?countryId=${countryId}`);
  }


}