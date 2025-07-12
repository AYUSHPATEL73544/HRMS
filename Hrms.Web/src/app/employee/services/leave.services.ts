import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class LeaveService {

    private apiEndPoint = environment.apiBaseUrl + '/leave';

    constructor(private http: HttpClient) { }

    getLeave(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}`);
    }

    getList(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/list`);
    }

    getByRuleId(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/by-ruleid/${id}`)
    }
}