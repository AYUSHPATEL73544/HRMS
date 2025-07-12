import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { SelectListItemModel } from "src/app/shared/models";

@Injectable()
export class ManufacturerService {

    private apiEndPoint = environment.apiBaseUrl + '/manufacturer';

    constructor(private http: HttpClient) { }
    getSelectListItem(assetTypeId: number): Observable<Array<SelectListItemModel>> {
        return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items?assetTypeId=${assetTypeId}`);
    }

}