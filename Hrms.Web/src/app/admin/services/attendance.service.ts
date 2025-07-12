import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { AttendanceModel} from "../attendance/model";

@Injectable()
export class AttendanceService {

    private apiEndPoint = environment.apiBaseUrl + '/attendance';

    constructor(private http: HttpClient) { }

    addAttendanceRule(model: AttendanceModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }
}