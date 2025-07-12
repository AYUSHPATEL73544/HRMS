import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { TeamModel } from "../directory/models";

@Injectable()
export class TeamService {

    private apiEndPoint = environment.apiBaseUrl + '/team';

    constructor(private http: HttpClient) { }

    add(model: TeamModel): Observable<any> {
        
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    getByEmployeeId(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getById(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-by/${id}`);
    }

    getByManagerId(id : number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-by-manager/${id}`);
    }

    update(model: TeamModel): Observable<any> {     
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }
}