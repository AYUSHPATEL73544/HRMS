import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { FilterModel, InterviewModel } from "../models";

@Injectable()
export class InterviewService {

    private apiEndPoint = environment.apiBaseUrl + '/interview';

    constructor(private http: HttpClient) { }

    add(model: InterviewModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    update(model: InterviewModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    getDetail(id: number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/list?${queryString}`);
    }

    getListByCandidateId(id: number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/list/${id}`);
    }
    
    get(id: number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/by-id/${id}`);
    }
}