import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { AttendanceRuleModel } from "../attendance/model";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class AttendanceRuleService {

    private apiEndPoint = environment.apiBaseUrl + '/attendanceRule';

    constructor(private http: HttpClient) { }

    addAttendanceRule(model: AttendanceRuleModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }
    
    getPagedList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/list?${queryString}`);
    }

    getSelectListItem(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-select-list-item`);
    }

    getDetail(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    updateAttendanceRule(model: AttendanceRuleModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    deleteRule(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }

    getbyYear(year: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/getByYear/${year}`);
    }

}