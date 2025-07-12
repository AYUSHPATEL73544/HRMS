import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable()
export class ListenerService {

    public profileUpdateListener = new Subject<any>();

    get listenProfileUpdate(): Observable<any> {
        return this.profileUpdateListener.asObservable();
    }
}

