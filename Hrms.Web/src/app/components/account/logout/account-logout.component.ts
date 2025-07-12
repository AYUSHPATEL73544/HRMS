import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';

@Component({
    selector: 'app-admin-logout',
    templateUrl: './account-logout.component.html'
})

export class AccountLogoutComponent {
    constructor(private router: Router,
        private msalService: MsalService) {
        if (this.msalService.instance.getAllAccounts().length > 0) {
            this.msalService.logout();
        }
        localStorage.clear();
        this.router.navigate(['account/login']);
    }
}
