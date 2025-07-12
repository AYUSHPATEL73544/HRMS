import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AppUtils, Constants } from 'src/app/utilities';

@Injectable()
export class EmployeeGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (AppUtils.isUserAuthenticated() && (AppUtils.getUserRole() !== Constants.Roles.Admin)) {
      return true;
    }
    this.router.navigate(['account/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}

