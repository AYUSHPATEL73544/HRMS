import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveLogModel } from 'src/app/employee/leave/models';

@Component({
    selector: 'app-leave-log-reject',
    templateUrl: './leave-log-reject.component.html',
})
export class LeaveLogRejectComponent {
    @BlockUI('blockui-LogReject') 

    blockUI: NgBlockUI;
    title: string;
    message: string;
    status: number;
    model: LeaveLogModel;

    constructor(
        @Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<LeaveLogRejectComponent>
    ) {
        this.model = new LeaveLogModel();
        this.title = data.title;
        this.message = data.message;
        this.status = data.status;
        this.model = data.model;
        
    }

    cancel(): void {
        this.dialogRef.close({ log: null, confirm:false });
    }
}