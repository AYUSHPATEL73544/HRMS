import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AppUtils, Constants } from 'src/app/utilities';
import { CurrentUserModel } from 'src/app/shared/models';
import { ProfileImageModel } from '../../models/profile-image.model';
import { BaseService, DocumentService, ListenerService } from '../../services';
import { ChangePasswordComponent } from 'src/app/components';
import { DialogConfirmComponent } from '../../dialog';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-user-dropdown',
    templateUrl: './user-dropdown.component.html'
})

export class UserDropdownComponent implements OnInit {
    @BlockUI('myprofile-blockui') blockUI: NgBlockUI;
    model: CurrentUserModel;
    imageModel: ProfileImageModel;
    userDrop: string;
    genderDrop: string;
    subscriptions = new Array<Subscription>();
    profileUrl = '/';
    logoutUrl = '/account/logout';
    userId = AppUtils.getUserId();
    updateProfileSubscription: Subscription;
    updateProfileSubscriptions = new Array<Subscription>();
    imagePath: string;


    constructor(private dialog: MatDialog,
        public appUtils: AppUtils,
        private router: Router,
        private documentService: DocumentService,
        private baseService: BaseService,
        private listenerService: ListenerService
    ) {
        if (AppUtils.getUserRole() === Constants.Roles.Admin) {
            this.userDrop = Constants.Roles.Admin;
        }
        else if (AppUtils.getUserRole() === Constants.Roles.Employee) {
            this.userDrop = Constants.Roles.Employee;
        }

        this.updateProfileSubscription =
            listenerService.listenProfileUpdate.subscribe(() => {
                this.getUserImage();
            });
    }

    get constants(): typeof Constants {
        return Constants
    }

    ngOnInit(): void {
        if (AppUtils.isUserAuthenticated()) {
            this.loadCurrentUserProfile();
        }

        this.getUserImage();

    }

    loadCurrentUserProfile(): void {

        this.model = AppUtils.getCurrentUserProfile();

        if (this.model.role === Constants.Roles.Admin) {
            this.profileUrl = '/admin/profile';
        }

    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => { s.unsubscribe(); });
        this.updateProfileSubscription.unsubscribe();
    }

    changePassword() {
        this.dialog.open(ChangePasswordComponent, {
            width: '500px',
            disableClose: true,
        });
    }

    logout() {
        const dialogRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: `Log out`,
                message: `Are you sure you want to log out?`,
            },
            width: Constants.dialogSize.medium,
            disableClose: true,
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.router.navigateByUrl('/account/logout');
            }
        });
    }


    getUserImage() {
        this.documentService.getProfileImage().subscribe({
            next: (res) => {
                this.imageModel = res;
                if (this.imageModel != null) {
                    this.imageModel.imageUrl = this.getImageUrl(this.imageModel.key);
                }
            },
            error: (error) => {
                this.baseService.processErrorResponse(error);
            }
        })
    }

    getImageUrl(key: string): string {
        return environment.apiBaseUrl + '/documents/' + key;
    }

}

