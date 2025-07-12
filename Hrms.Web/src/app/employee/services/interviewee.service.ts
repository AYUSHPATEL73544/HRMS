import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CandidateChangeStatusModel } from '../job-recruitment/model';

@Injectable()
export class IntervieweeService {

    private apiEndPoint = environment.apiBaseUrl + '/candidate';

    constructor(private http: HttpClient) { }

    getDetail(id:number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/by-id/${id}`);
    }
    
    changeStatus(model: CandidateChangeStatusModel):Observable<any>{
        return this.http.put(`${this.apiEndPoint}/change-status`, model);
    }
    
    getCandidate(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

}