import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { AssetAllocationModel } from "../asset/model";

@Injectable()
export class AssetAssignService {

    private apiEndPoint = environment.apiBaseUrl + '/assetallocation';

    constructor(private http: HttpClient) { }
    add(model: AssetAllocationModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }
}