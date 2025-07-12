import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveLogModel } from '../../models';

@Component({
    selector: 'app-leave-log-detail',
    templateUrl: './leave-log-details.component.html',
})
export class LeaveLogDetailsComponent {
    @BlockUI('blockui-Details') 

    blockUI: NgBlockUI;
    title: string;
    message: string;
    status: number;
    model: LeaveLogModel;

    constructor(
        @Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<LeaveLogDetailsComponent>
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
