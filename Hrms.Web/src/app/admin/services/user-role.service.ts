import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { UserRoleModel } from "../directory/models";

@Injectable()
export class UserRoleService {

    private apiEndPoint = environment.apiBaseUrl + '/userRole';

    constructor(private http: HttpClient) { }

    add(model: UserRoleModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    get(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getSelectListItems(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}`);
    }
}