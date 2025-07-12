import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class CalendarService {

    private apiEndPoint = environment.apiBaseUrl + '/attendanceLog';
    private holidayApiEndPoint = environment.apiBaseUrl + '/holiday';
    private employeeApiEndPoint = environment.apiBaseUrl + '/employee';

    constructor(private http: HttpClient) { }

    getEmployeeAtteandanceLog(year: number, month: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-employee-attendance-log/${year}/${month}`);
    }

    getEmployeeAbsentEvents(year: number, month: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-employee-absent-event/${year}/${month}`);
    }

    getHolidayEvent(year: number, month: number): Observable<any> {
        return this.http.get(`${this.holidayApiEndPoint}/holiday-event/${year}/${month}`);
    }

    getEmployeeLeaveCalendarEvent(year: number, month: number): Observable<any> {
        return this.http.get(`${this.employeeApiEndPoint}/employee-leave-calendar-event/${year}/${month}`);
    }

}