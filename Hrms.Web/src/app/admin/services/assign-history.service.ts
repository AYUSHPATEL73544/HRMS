import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";


@Injectable()
export class AssignHistoryService {

    private apiEndPoint = environment.apiBaseUrl + '/assetAllocation';

    constructor(private http: HttpClient) { }
    assignHistory(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/history-by-assetId/${id}`);
    }
}