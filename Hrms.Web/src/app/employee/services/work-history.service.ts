import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class WorkHistoryService {

    private apiEndPoint = environment.apiBaseUrl + '/workHistory';

    constructor(private http: HttpClient) { }

   get():Observable<any>{
    return this.http.get(`${this.apiEndPoint}`);
   }
}