import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { EventMessage, EventType, AuthenticationResult, InteractionStatus } from '@azure/msal-browser';
import { BaseService, UserService } from 'src/app/shared/services';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-aad-login',
    templateUrl: './aad-login.component.html',
})

export class AzureAdLoginComponent implements OnInit, OnDestroy {
    @BlockUI('blockui-aad-login') blockUI: NgBlockUI;
    loginProcessed = false;

    private readonly _destroying$ = new Subject<void>();

    constructor(private authService: MsalService,
        private msalBroadcastService: MsalBroadcastService,
        private userService: UserService,
        private router: Router,
        private baseService: BaseService) { }

    ngOnInit(): void {
        this.blockUI.start();
        this.msalBroadcastService.msalSubject$
            .pipe(
                filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
                takeUntil(this._destroying$)
            )
            .subscribe(
                (result: EventMessage) => {
                    const payload = result.payload as AuthenticationResult;
                    this.loginProcessed = true;
                    this.authService.instance.setActiveAccount(payload.account);
                    localStorage.setItem(Constants.varMsalAuthToken, payload.accessToken)
                    this.login(payload.accessToken);

                });

        this.msalBroadcastService.inProgress$
            .pipe(
                filter((status: InteractionStatus) => status === InteractionStatus.None)
            )
            .subscribe({
                next: () => {
                    this.checkAndSetActiveAccount();
                    if (!this.loginProcessed) {
                        this.login(localStorage.getItem(Constants.varMsalAuthToken));
                    }
                }
            });
    }

    checkAndSetActiveAccount() {
        let activeAccount = this.authService.instance.getActiveAccount();

        if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
            let accounts = this.authService.instance.getAllAccounts();
            this.authService.instance.setActiveAccount(accounts[0]);
        }
    }

    login(accessToken: string) {
        this.userService.adLogin(accessToken).subscribe({
            next: (token: string) => {
                localStorage.setItem(Constants.varAuthToken, token);
                if (AppUtils.getUserRole() === Constants.Roles.Admin) {
                    this.router.navigate(['/admin/dashboard']);
                }
                else if (AppUtils.getUserRole() !== Constants.Roles.Admin && AppUtils.getUserRole() !== '') {
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

    ngOnDestroy(): void {
        this._destroying$.next(undefined);
        this._destroying$.complete();
    }
}
