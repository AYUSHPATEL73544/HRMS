import { Component, Inject } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { HireModel } from "../../model";
import { AppUtils } from "src/app/utilities";
import { SelectListItemModel } from "src/app/shared/models";
import { CandidateService, DepartmentService, DesignationService } from "src/app/employee/services";
import { BaseService } from "src/app/shared/services";

@Component({
    selector:'app-upsert-hire',
    templateUrl: './upsert-hire.component.html'
})

export class UpsertHireComponent{
    @BlockUI('upsert-shortList-blockui') blockUI: NgBlockUI;

    model = new HireModel();
    isModelLoaded: boolean;
    departments = new Array<SelectListItemModel>();
    designations = new Array<SelectListItemModel>();

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpsertHireComponent>,
        private service: CandidateService,
        private departmentService: DepartmentService,
        private designationService: DesignationService,
        private baseService: BaseService,
        public appUtils: AppUtils
    ) {
        this.isModelLoaded = false;
        if (data) {
           this.model.id = data.id,
           this.model.firstName = data.firstName,
           this.model.lastName = data.lastName,
           this.model.phone = data.phone,
           this.model.gender = data.gender
        }
        this.model.companyId = this.appUtils.getCompanyId();
    }

    ngOnInit(): void {
        this.getDesignationList();
        this.getDepartmentList();
    }

    getDesignationList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.designationService.getSelectListItem().subscribe({
            next: (response) => {
                this.designations = response;
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getDepartmentList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.departmentService.getSelectListItem().subscribe({
            next: (response) => {
                this.blockUI.stop();
                this.departments = response;
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    submit(): void {
        this.model.dateOfJoining = AppUtils.getFormattedDate(this.model.dateOfJoining, null);
        this.model.dateOfJoining = AppUtils.getDate(this.model.dateOfJoining);
        this.service.hire(this.model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.successNotification('Candidate has been hired successfully.');
                this.dialogRef.close();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }

}