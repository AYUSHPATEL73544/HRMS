import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class CalendarService {

  private apiEndPoint = environment.apiBaseUrl + '/employee';
  private holidayApiEndPoint = environment.apiBaseUrl + '/holiday';

  constructor(private http: HttpClient) { }

  getCalendarEvent(year: number, month: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/calendar-event/${year}/${month}`);
  }

 
  getHolidayEvent(year: number, month: number): Observable<any> {
    return this.http.get(`${this.holidayApiEndPoint}/holiday-event/${year}/${month}`);
  }

  
}