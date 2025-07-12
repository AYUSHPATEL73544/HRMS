import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { AttendanceFilterModel, AttendanceLogModel } from "src/app/admin/attendance/model/index";
import { AttendanceLogChangeStatusModel } from "../attendance/model/attedance-log-change-status.model";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class AttendanceLogService {
    private apiEndPoint = environment.apiBaseUrl + '/attendanceLog';

    constructor(private http: HttpClient) { }


    addAttendanceLog(model: AttendanceLogModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }


    getLogList(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/list`);
    }

    getAttendanceHistory(model: AttendanceFilterModel, id: number): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-by-attendanceId/${id}?${queryString}`);
    }

    getDetail(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getAttendanceDeatil(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/detail-by-employeeid/${id}`);
    }

    updateAttendanceLog(model: AttendanceLogModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    deleteLog(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }

    deleteAttendance(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/by-attendance/${id}`);
    }

    getAttendanceHistoryList(model: AttendanceFilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-attendance-history?${queryString}`);
    }

    changeStatus(model: AttendanceLogChangeStatusModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}/change-status`, model)
    }

    pageList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/paged-list?${queryString}`);
    }

   
}