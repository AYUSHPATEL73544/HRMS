import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class CandidateService {

    private apiEndPoint = environment.apiBaseUrl + '/candidate';

    constructor(private http: HttpClient) { }

    getDetail(id:number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/by-id/${id}`);
    }

    get(id:number):Observable<any>{
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }
}