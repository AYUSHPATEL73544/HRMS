import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SelectListItemModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";

@Injectable()
export class CourseTypeService {

    private apiEndPoint = environment.apiBaseUrl + '/courseType';

    constructor(private http: HttpClient) { }

    getSelectListItem(): Observable<Array<SelectListItemModel>> {
        return this.http.get<Array<SelectListItemModel>>(`${this.apiEndPoint}/select-list-items`);
    }
}