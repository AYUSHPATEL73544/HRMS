import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { NoteModel } from "src/app/admin/directory/models";
import { Observable } from "rxjs";

@Injectable()
export class NoteService {
    private apiEndPoint = environment.apiBaseUrl + '/note';

    constructor(private http: HttpClient) { }

    add(model:NoteModel): Observable<any> {
        return this.http.post(`${this.apiEndPoint}`, model);
    }

    update(model:NoteModel): Observable<any> {
        return this.http.put(`${this.apiEndPoint}`, model);
    }

    getNoteById(id: number): Observable<any>{
        return this.http.get(`${this.apiEndPoint}/${id}`);
    }

    getNoteList(id: number): Observable<any>{
        return this.http.get(`${this.apiEndPoint}/note-list/${id}`);
    }

    delete(id: number): Observable<any>{
        return this.http.delete(`${this.apiEndPoint}/${id}`);
    }
}