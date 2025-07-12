import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class TeamService {

    private apiEndPoint = environment.apiBaseUrl + '/team';

    constructor(private http: HttpClient) { }

    get(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}`);
    }

    getReportessList():Observable<any>{
        return this.http.get(`${this.apiEndPoint}/get-reprotess-list`);
    }
}