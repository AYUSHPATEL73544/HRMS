import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MsalService, MSAL_GUARD_CONFIG, MsalGuardConfiguration, MsalBroadcastService } from '@azure/msal-angular';
import { InteractionStatus, RedirectRequest } from '@azure/msal-browser';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { BaseService, UserService } from 'src/app/shared/services';
import { LoginModel } from 'src/app/shared/models';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-account-login',
    templateUrl: './account-login.component.html',
    styleUrls: ['./account-login.component.scss']
})

export class AccountLoginComponent implements OnInit {
    @BlockUI('container-blockui') blockUI: NgBlockUI;
    model = new LoginModel();
    returnUrl: string;
    hidePassword = true;
    private readonly _destroying$ = new Subject<void>();

    constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
        private authService: MsalService,
        private msalBroadcastService: MsalBroadcastService,
        private router: Router,
        private route: ActivatedRoute,
        private userService: UserService,
        private baseService: BaseService) {
        this.route.queryParams.subscribe(params => {
            this.returnUrl = params['returnUrl'];
        });
    }

    ngOnInit(): void {
        if (localStorage.getItem(Constants.varAuthToken)) {
            if (AppUtils.getUserRole() === Constants.Roles.Admin) {
                this.router.navigate(['/admin/dashboard']);
            }
            else {
                this.router.navigate(['/employee/dashboard']);
            }
        }
        this.msalBroadcastService.inProgress$
            .pipe(
                filter((status: InteractionStatus) => status === InteractionStatus.None),
                takeUntil(this._destroying$)
            )
            .subscribe(() => {
            });
    }

    submit(): void {
        this.blockUI.start();
        this.userService.login(this.model).subscribe({
            next: (token: string) => {
                localStorage.setItem(Constants.varAuthToken, token);
                if (this.returnUrl) {
                    const decodeUrl = this.returnUrl.indexOf('%') !== -1 ? decodeURI(this.returnUrl) : this.returnUrl;
                    this.router.navigateByUrl(decodeUrl);
                }
                else if (AppUtils.getUserRole() === Constants.Roles.Admin) {
                    this.router.navigate(['/admin/dashboard']);
                }
                else {
                    
                    this.router.navigate(['/employee/dashboard']);
                }
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    signInWithMicrosoft() {
        if (this.msalGuardConfig.authRequest) {
            this.authService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
        } else {
            this.authService.loginRedirect();
        }
    }

    ngOnDestroy(): void {
        this._destroying$.next(undefined);
        this._destroying$.complete();
    }
}


