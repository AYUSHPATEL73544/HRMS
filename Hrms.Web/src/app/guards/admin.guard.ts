import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Constants, AppUtils } from 'src/app/utilities';
import { Injectable } from '@angular/core';


@Injectable()
export class AdminGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (AppUtils.isUserAuthenticated() && AppUtils.getUserRole() === Constants.Roles.Admin) {
            return true;
        }
        this.router.navigate(['account/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}

