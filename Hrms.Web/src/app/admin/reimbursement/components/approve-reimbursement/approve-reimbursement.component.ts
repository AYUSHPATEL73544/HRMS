import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ReimbursementChangeStatusModel } from 'src/app/admin/reimbursement/models/reimbursement-change-status.model';

@Component({
    selector: 'app-approve-reimbursement',
    templateUrl: './approve-reimbursement.component.html'
})

export class ApproveReimbursementComponent {
    @BlockUI('blockui-approve-reimbursement') blockUI: NgBlockUI;

    model = new ReimbursementChangeStatusModel();

    constructor(private dialogRef: MatDialogRef<ApproveReimbursementComponent>) { }

    submit(): void {
        this.dialogRef.close({ remark: this.model.remark, paymentDate: this.model.paymentDate, confirm: true });
    }
    cancel(): void {
        this.dialogRef.close({ remark: null, paymentDate: null, confirm: false });
    }
}