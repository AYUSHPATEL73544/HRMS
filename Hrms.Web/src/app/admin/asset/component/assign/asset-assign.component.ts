import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { EmployeeService, AssetAssignService } from "src/app/admin/services";
import { AssetAllocationModel } from "src/app/admin/asset/model/asset-allocation.model";
import { SelectListItemModel } from "src/app/shared/models";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-asset-assign',
    templateUrl: './asset-assign.component.html',
})

export class AssetAssignComponent implements OnInit {
    @BlockUI('asset-assign-blockui') blockUI: NgBlockUI;

    model = new AssetAllocationModel();
    employees = new Array<SelectListItemModel>();

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<AssetAssignComponent>,
        private employeeService: EmployeeService,
        private assetAssign: AssetAssignService,
        private baseService: BaseService) {
        if (data) {
            this.model.assetId = data.id;
            this.model.purchaseDate = data.purchaseDate;
        }
    }
    ngOnInit(): void {
        this.getEmployeeList();
    }
    getEmployeeList(): void {
        this.blockUI.start();
        this.employeeService.getSelectListItem().subscribe({
            next: (response) => {
                this.employees = response;
                this.blockUI.stop();
            },
            error(error: any) {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }
    submit(): void {
        this.model.effectiveFrom = AppUtils.getFormattedDate(this.model.effectiveFrom, null);
        this.model.effectiveFrom = AppUtils.getDate(this.model.effectiveFrom);
        this.assetAssign.add(this.model).subscribe({
            next: () => {
                this.baseService.successNotification("Asset has been assigned successfully.");
                this.dialogRef.close();
            },
            error: (error: any) => {
                this.baseService.processErrorResponse(error);
            }
        });
    }
    cancel(): void {
        this.dialogRef.close();
    }
}