import { Component, ViewChild } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { FormGroup, NgForm } from "@angular/forms";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService, UserService } from "src/app/shared/services";
import { ChangePasswordModel } from "src/app/shared/models";

@Component({
    selector: 'change-password',
    templateUrl: './change-password.component.html',
})

export class ChangePasswordComponent {
    @BlockUI('change-password-blockui') blockUI: NgBlockUI;
    @ViewChild('f') f: NgForm;

    model = new ChangePasswordModel();
    hidePassword = true;
    hideConfirmPassword = true;
    hideOldPassword = true;

    constructor(private dialogRef: MatDialogRef<ChangePasswordComponent>,
        private baseService: BaseService,
        private service: UserService) {

    }

    closeResetPasswordPopup(): void {
        this.dialogRef.close();
    }

    submit(): void {
        this.blockUI.start();
        this.service.changePassword(this.model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.dialogRef.close();
                this.baseService.successNotification("Password has been changed successfully.");
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    validate(f: FormGroup): void {
        if (this.model.password !== this.model.confirmPassword) {
            f.get('confirmPassword').setErrors({ passwordNotMatch: false });
        } else {
            f.get('confirmPassword').setErrors(null);
        }
    }
}