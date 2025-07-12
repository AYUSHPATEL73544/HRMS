import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Constants, AppUtils } from 'src/app/utilities';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router) { }

    canActivate(_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem(Constants.varAuthToken) && AppUtils.isUserAuthenticated()) {
            return true;
        }

        this.router.navigate(['/account/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
