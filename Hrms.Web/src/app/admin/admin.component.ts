import { Component, ViewEncapsulation, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Constants, AppUtils } from 'src/app/utilities';
import * as moment from 'moment';


@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    encapsulation: ViewEncapsulation.None
})

export class AdminComponent implements OnDestroy {
    subscriptions = new Array<Subscription>();
    isUserAthenticated = false;
    fullLayout: boolean;
    year :any;
    currentYear: any;
    constructor(private router: Router) {
        this.fullLayout = true;

        this.isUserAthenticated = AppUtils.isUserAuthenticated() && AppUtils.getUserRole() === Constants.Roles.Admin;

        if (!AppUtils.isUserAuthenticated() || AppUtils.getUserRole() !== Constants.Roles.Admin) {
            this.router.navigate(['/login']);
        }
        this.currentYear = moment();
        this.year = this.currentYear.format('YYYY');
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => { s.unsubscribe(); });
    }

    toggleLayout(): void {
        this.fullLayout = !this.fullLayout;
    }
}
