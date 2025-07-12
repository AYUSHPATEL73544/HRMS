import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { MatSort } from "@angular/material/sort";
import { AssignHistoryService } from "src/app/admin/services";
import { AssetAllocationModel } from "src/app/admin/asset/model/asset-allocation.model";


@Component({
    selector: 'app-assign-history',
    templateUrl: './assign-history.component.html',
})

export class AssignHistoryComponent implements OnInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @BlockUI('assign-history-blockui') blockUI: NgBlockUI;

    model = new AssetAllocationModel();
    histories = new Array<AssetAllocationModel>();


    displayedColumns = ['name', 'effectiveFrom', 'effectiveTo', 'status'];
    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<AssignHistoryComponent>,
        private assignHistoryService: AssignHistoryService,

    ) {
        if (data) {
            this.model.assetId = data.id;
        }
    }

    ngOnInit(): void {
        this.assignHistory();
    }

    assignHistory() {
        this.blockUI.start();
        this.assignHistoryService.assignHistory(this.model.assetId).subscribe({
            next: (response) => {
                this.histories = response;
                this.blockUI.stop();
            },
            error(error: any) {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }
}

