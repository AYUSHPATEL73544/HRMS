import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class SkillService {

  private apiEndPoint = environment.apiBaseUrl + '/skill';

  constructor(private http: HttpClient) { }

  getSkillList(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/select-list-items`);
  }
}