import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class DocumentService {

  private apiEndpoint = environment.apiBaseUrl + '/document';

  constructor(private http: HttpClient) {
  }

  addImage(model: any): Observable<any> {
    return this.http.post(`${this.apiEndpoint}/addProfileImage`, model);
  }

  updateImage(model: any): Observable<any> {
    return this.http.put(`${this.apiEndpoint}/updateProfileImage`, model);
  }

  getProfileImage(): Observable<any> {
    return this.http.get(`${this.apiEndpoint}/getProfileImageByUserId`);
  }

  deleteImage(id: any): Observable<any> {
    return this.http.delete(`${this.apiEndpoint}/${id}`);
  }

}