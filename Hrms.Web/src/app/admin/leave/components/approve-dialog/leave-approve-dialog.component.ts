import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AppUtils } from "src/app/utilities";
import { LeaveLogModel } from "../../models";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { LeaveLogService } from "src/app/admin/services";


@Component({
    selector: 'app-leave-approve-dialog',
    templateUrl: './leave-approve-dialog.component.html',
    
})
export class LeaveApproveDialogComponent {
    @BlockUI() blockUI: NgBlockUI;

    title: string;
    message: string;
    employees = new Array<LeaveLogModel>();
    model: LeaveLogModel;

    constructor(@Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<LeaveApproveDialogComponent>,
        private service: LeaveLogService,
        private baseService: BaseService
    ) {
        this.title = data.title;
        this.message = data.message;
        this.model = data.model;
    }

    ngOnInit() {
        this.getList();
    }

    getList() {
        this.model.startDate = AppUtils.getFormattedDate(this.model.startDate, null);
        this.model.startDate = AppUtils.getDate(this.model.startDate);
        this.model.endDate = AppUtils.getFormattedDate(this.model.endDate, null);
        this.model.endDate = AppUtils.getDate(this.model.endDate);
        this.blockUI.start("Loading...");
        this.service.getLeaveLogs(this.model.startDate, this.model.endDate).subscribe({
            next: (response) => {
                this.employees = response;
                this.employees.forEach(element => {
                    element.createdOn = AppUtils.getLocalFormattedDate(element.createdOn);
                    element.startDate = AppUtils.getLocalFormattedDate(element.startDate);
                    element.endDate = AppUtils.getLocalFormattedDate(element.endDate);
                });
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }

    submit() {
        this.dialogRef.close(true);
    }
    
    cancel() {
        this.dialogRef.close(false);
    }

}