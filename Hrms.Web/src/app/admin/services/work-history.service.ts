import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { WorkHistoryModel } from "../directory/models";

@Injectable()
export class WorkHistoryService {

    private apiEndPoint = environment.apiBaseUrl + '/workHistory';

    constructor(private http: HttpClient) { }

    get(employeeId: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-by-employeeId/${employeeId}`);
    }

    getById(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    update(model: WorkHistoryModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }
}