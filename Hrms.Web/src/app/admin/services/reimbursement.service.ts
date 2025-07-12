import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { FilterModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";
import { ReimbursementChangeStatusModel } from "src/app/admin/reimbursement/models/reimbursement-change-status.model";
import { ReimbursementFilterModel } from 'src/app/admin/reimbursement/models/reimbursement-filter.model'

@Injectable()
export class ReimbursementService {
    private apiEndPoint = environment.apiBaseUrl + '/reimbursement';

    constructor(private http: HttpClient) { }

    getList(model: FilterModel): Observable<any> {
        const queryParams = model.toQueryString(); 
        return this.http.get(`${this.apiEndPoint}?${queryParams}`);
    }

    getPendingList(model: FilterModel): Observable<any> {
        const queryParams = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/pending-list?${queryParams}`);
    }

    toggleStatus(model: ReimbursementChangeStatusModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}/change-status`, model);
    }

    getPageListById(id: number, model: ReimbursementFilterModel): Observable<any> {
        const queryParams = model.toQueryString();
        const url = `${this.apiEndPoint}/reimbursement-history/${id}?${queryParams}`;
        return this.http.get(url);
    }
}