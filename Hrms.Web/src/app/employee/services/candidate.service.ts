import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { FilterModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";
import { CandidateChangeStatusModel, CandidateModel, HireModel } from "../job-application/model";

@Injectable()
export class CandidateService{

    private apiEndPoint = environment.apiBaseUrl + '/candidate';

    constructor(private http: HttpClient) { }

    getList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-list?${queryString}`);
    }

    getShortlist(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-shortlist-list?${queryString}`);
    }

    addCandidate(model: CandidateModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    deleteCandidate(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }

    getCandidate(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getDetail(id:number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/by-id/${id}`);
    }

    shortlist(id:number):Observable<any>{
        return this.http.put(`${this.apiEndPoint}/shortlist`, id);
    }

    hire(model: HireModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}/hire`, model);
    }

    update(model: CandidateModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    changeStatus(model: CandidateChangeStatusModel):Observable<any>{
        return this.http.put(`${this.apiEndPoint}/change-status`, model);
    }


}