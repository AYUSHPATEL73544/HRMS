import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { AttendanceFilterModel, AttendanceLogModel } from "src/app/employee/attendance/model/index";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class AttendanceLogService {
    private apiEndPoint = environment.apiBaseUrl + '/attendanceLog';

    constructor(private http: HttpClient) { }


    clockIn(model: AttendanceLogModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}/clock-in`, model);
    }

    clockOut(model: AttendanceLogModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}/clock-out`, model);
    }

    getLogList(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/list`);
    }

    getDetail(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getDetails(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}`);
    }

    pageList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/paged-list-for-employee?${queryString}`);
    }

    getAttendanceHistory(model: AttendanceFilterModel, id: number): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-by-userId/${id}?${queryString}`);
    }
    getEmployeeAttendanceHistory(model: AttendanceFilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-employee-attendance-history?${queryString}`)
    }

    updateAttendanceLog(model: AttendanceLogModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    deleteLog(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }
}