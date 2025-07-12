import { Component, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormGroup, NgForm } from "@angular/forms";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService, UserService } from "src/app/shared/services";
import { ChangePasswordModel } from "src/app/shared/models";

@Component({
    selector: 'reset-password',
    templateUrl: './reset-password.component.html',
})

export class ResetPasswordComponent {
    @BlockUI('reset-password-blockui') blockUI: NgBlockUI;
    @ViewChild('f') f: NgForm;

    model = new ChangePasswordModel();
    hidePassword = true;
    hideConfirmPassword = true;
    //hideOldPassword = true;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<ResetPasswordComponent>,
        private baseService: BaseService,
        private service: UserService) {
        if (data) {
            this.model.employeeId = data.id;
        }
    }

    closeResetPasswordPopup(): void {
        this.dialogRef.close();
    }

    submit(): void {
        this.blockUI.start();
        this.service.resetPassword(this.model).subscribe({
            next: () => {
                this.dialogRef.close();
                this.baseService.successNotification("Password has been reset successfully.");
                this.blockUI.stop();
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