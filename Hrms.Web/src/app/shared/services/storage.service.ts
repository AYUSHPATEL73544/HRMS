import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable()
export class StorageService {
    private apiEndPoint = environment.apiBaseUrl + '/storage';

    constructor(private http: HttpClient) { }

    uploadSingleFile(file: File, key: string): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post(`${this.apiEndPoint}/upload-single?key=${key}`, formData);
    }

    deleteSingleFile(key: string): Observable<any> {
        return this.http.delete(`${this.apiEndPoint}/delete-single/${key}`, {});
    }
}