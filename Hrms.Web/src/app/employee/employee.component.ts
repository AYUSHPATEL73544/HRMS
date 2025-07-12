import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppUtils, Constants } from 'src/app/utilities/index';
import * as moment from 'moment';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  encapsulation: ViewEncapsulation.None
})

export class EmployeeComponent {
  subscriptions = new Array<Subscription>();
  isUserAthenticated = false;
  fullLayout: boolean;
  currentYear: any;
  year: any;

  constructor(private router: Router) {
    this.fullLayout = true;

    this.isUserAthenticated = AppUtils.isUserAuthenticated()
      && AppUtils.getUserRole() !== Constants.Roles.Admin;

    if (!AppUtils.isUserAuthenticated() || AppUtils.getUserRole() === Constants.Roles.Admin) {
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