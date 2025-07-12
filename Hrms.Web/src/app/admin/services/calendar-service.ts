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

    getAttendanceLogEvent(year: number, month: number, employeeId: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-attendancelog-event/${year}/${month}/${employeeId}`);
    }

    getAbsentEvent(year: number, month: number, employeeId: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/get-absent-event/${year}/${month}/${employeeId}`);
    }

    getHolidayEvent(year: number, month: number): Observable<any> {
        return this.http.get(`${this.holidayApiEndPoint}/holiday-event/${year}/${month}`);
    }

    getLeaveCalendarEvent(year: number, month: number, employeeId: number): Observable<any> {
        return this.http.get(`${this.employeeApiEndPoint}/calendar-leave-event/${year}/${month}/${employeeId}`);
    }
}