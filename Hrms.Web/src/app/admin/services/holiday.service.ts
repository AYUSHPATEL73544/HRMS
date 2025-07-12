import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { HolidayGroupModel } from "../holidays/models";

@Injectable()
export class HolidayService {
    private apiEndPoint = environment.apiBaseUrl + '/holiday';
    constructor(private http: HttpClient) { }

    add(model: HolidayGroupModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    getByYear(year: number): Observable<any> {
        const queryString = `year=${year}`;
        return this.http.get(`${this.apiEndPoint}/detail?${queryString}`,);
    }

    getPreviousYear(year: number, isChecked: boolean): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/previous-year/${year}/${isChecked}`);
    }
}