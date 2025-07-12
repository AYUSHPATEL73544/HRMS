import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { FamilyModel } from "../profile/models/family.model";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class FamilyServices {

    private apiEndPoint = environment.apiBaseUrl + '/family';
    constructor(private http: HttpClient) { }

    add(model: FamilyModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    get(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}`);
    }

    getPageList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/paged-list-by-user-id?${queryString}`);
    }

    getById(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    update(model: FamilyModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }
}