import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { ReimbursementModel } from "src/app/employee/reimbursement/models";
import { Observable } from "rxjs";
import { FilterModel } from "src/app/shared/models";
import { ReimbursementChangeStatusModel } from "src/app/employee/reimbursement/models/reimbursement-change-status.model";

@Injectable()
export class ReimbursementService {
    private apiEndPoint = environment.apiBaseUrl + '/reimbursement';

    constructor(private http: HttpClient) { }

    addReimbursement(model: ReimbursementModel): Observable<any> {
        return this.http.post(this.apiEndPoint, model);
    }

    getPageList(model: FilterModel): Observable<any> {
        const queryParams = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/page-list?${queryParams}`);
    }

    getById(id: number) {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    toggleStatus(model: ReimbursementChangeStatusModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}/change-status`, model);
    }

    update(model: ReimbursementModel): Observable<any> {
        return this.http.put(this.apiEndPoint, model);
    }
}