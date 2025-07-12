import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { FilterModel } from "src/app/shared/models";
import { environment } from "src/environments/environment";
import { AssetModel } from "../asset/model";

@Injectable()
export class AssetService {

    private apiEndPoint = environment.apiBaseUrl + '/asset';

    constructor(private http: HttpClient) { }

    getList(model: FilterModel): Observable<any> {
        const queryString = model.toQueryString();
        return this.http.get(`${this.apiEndPoint}/get-list?${queryString}`);
    }
    add(model: AssetModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    deleteAsset(id: number): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }

    getAsset(id: number): Observable<any> {
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }
    update(model: AssetModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

}