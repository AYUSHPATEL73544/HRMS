import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginModel, ChangePasswordModel } from 'src/app/shared/models';

@Injectable()
export class UserService {

    private apiEndPoint = environment.apiBaseUrl + '/user';

    constructor(private http: HttpClient) { }

    login(model: LoginModel): Observable<any> {
        return this.http.post(this.apiEndPoint + '/login', model);
    }

    adLogin(accessToken: string): Observable<any> {
        return this.http.post(this.apiEndPoint + '/ad-login?accessToken=' + accessToken, null);
    }

    logout(): Observable<any> {
        return this.http.post(this.apiEndPoint + '/logout', null);
    }

    changePassword(model: ChangePasswordModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}/change-password`,model);
    }

    resetPassword(model: ChangePasswordModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}/reset-password`,model);
    }
    getSelectListItem(): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/list`)
    }
    
}
