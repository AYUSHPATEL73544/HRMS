import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { LeaveLogFilterModel, LeaveLogModel } from "../leave/models";
import { LeaveLogChangeStatus } from "../leave/models/leave-log-change-status.model";

@Injectable()
export class LeaveLogService {

    private apiEndPoint = environment.apiBaseUrl + '/leaveLog';

    constructor(private http: HttpClient) { }

    GetPagedList(model: LeaveLogFilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/list?${queryString}`);
    }

    getPendingLeavesList(model: LeaveLogFilterModel): Observable<any>{
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/pending-leaves-list?${queryString}`);
    }

    getPagedListByEmployeeId(model:LeaveLogFilterModel, id: number):Observable<any>{
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/paged-list-by-employee/${id}?${queryString}`);
    }

    addLeaveLog(model: LeaveLogModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    changeStatus(model: LeaveLogChangeStatus): Observable<any> {
        return this.http.post(`${this.apiEndPoint}/change-status`, model)
    }

    getLeaveLogs(startDate:any,endDate:any):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/${startDate}/${endDate}`);
    }
}
