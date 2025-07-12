import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SelectListItemModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";

@Injectable()
export class QualificationTypeService {

    private apiEndPoint = environment.apiBaseUrl + '/qualificationType';

    constructor(private http: HttpClient) { }

    getSelectListItem(): Observable<Array<SelectListItemModel>> {
        return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items`);
    }
}