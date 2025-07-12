import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { FilterModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";

@Injectable()
export class FamilyService {

    private apiEndPoint = environment.apiBaseUrl + '/family';
    constructor(private http: HttpClient) { }

    getPagedList(model: FilterModel, id:number):Observable<any>{
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/list/${id}?${queryString}`);
    }

}