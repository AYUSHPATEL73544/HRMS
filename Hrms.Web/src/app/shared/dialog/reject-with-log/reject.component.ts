import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { RejectWithLogModel } from "src/app/shared/models/reject-with-log.model";

@Component({
    selector: 'app-reject',
    templateUrl: './reject.component.html'
})

export class RejectComponent {
    @BlockUI('blockui-reject') blockUI: NgBlockUI;
    title: string;
    message: string;
    status: number;
    model = new RejectWithLogModel();
    rejectWithLog: boolean;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<RejectComponent>) {
        this.title = data.title;
        this.message = data.message;
        this.status = data.status; 
    }

    submit(): void {
        this.dialogRef.close({ log: this.model.rejectionLog, confirm: true });
    }

    cancel(): void {
        this.dialogRef.close({ log: null, confirm: false });
    }
}