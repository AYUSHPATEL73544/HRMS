import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { LeaveLogModel, TotalLeaveCountModel } from "src/app/employee/leave/models/index";
import { LeaveLogFilterModel } from "../leave/models/leave-log-filter.model";
import { LeaveLogChangeStatusModel } from "src/app/employee/leave/models/index";

@Injectable()
export class LeaveLogService {
    private apiEndPoint = environment.apiBaseUrl + '/leaveLog';

    constructor(private http: HttpClient) { }

    addLeave(model: LeaveLogModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    getPageList(model: LeaveLogFilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/paged-list?${queryString}`);
    }

    update(model: LeaveLogModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    deleteLog(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`)
    }

    getDetail(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    GetPagedList(model: LeaveLogFilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/reportee-list?${queryString}`);
    }

    changeStatus(model: LeaveLogChangeStatusModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}/change-status`, model)
    }

    getTotalLeaveCount(model: TotalLeaveCountModel): Observable<any> {
        const queryString = model.toQueryString(); 
        return this.http.get(`${this.apiEndPoint}/total-leave-count?${queryString}`);
    }
}