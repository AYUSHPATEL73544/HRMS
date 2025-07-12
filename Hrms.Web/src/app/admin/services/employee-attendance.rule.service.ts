import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { EmployeeAttendanceModel } from "../attendance/model";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class EmployeeAttendanceRuleService {

    private apiEndPoint = environment.apiBaseUrl + '/employeeAttendanceRule';

    constructor(private http: HttpClient) { }

    add(model: EmployeeAttendanceModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    pageList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/list?${queryString}`);
    }
    inActivepageList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/inactive-list?${queryString}`);
    }


}