import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { DepartmentService, DesignationService, WorkHistoryService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { WorkHistoryModel } from "../../../models";
import { SelectListItemModel } from "src/app/shared/models";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-update-work-detail',
    templateUrl: './update-work-detail.component.html',
})

export class UpdateWorkDetailComponent implements OnInit {
    @BlockUI('update-work-blockui') blockUI: NgBlockUI;

    model = new WorkHistoryModel();
    departments = new Array<SelectListItemModel>();
    designations = new Array<SelectListItemModel>();
    department: string;
    designation: string;
    isModelLoaded: boolean;
    minDate: any;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpdateWorkDetailComponent>,
        private baseService: BaseService,
        private service: WorkHistoryService,
        private departmentService: DepartmentService,
        private designationService: DesignationService,) {
        this.isModelLoaded = false;
        if (data) {
            this.model.id = data.id;
            this.model.dateOfJoining = data.dateOfJoining;
            this.minDate = this.model.dateOfJoining;
        }

    }

    ngOnInit(): void {
        if (this.model.id) {
            this.getWork(this.model.id);
        }

        this.getDesignationList();
        this.getDepartmentList();

    }

    getWork(id: number): void {
        this.isModelLoaded = false;
        this.blockUI.start();
        this.service.getById(id).subscribe({
            next: (response) => {
                this.model = response;
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getDepartmentList(): void {
        this.isModelLoaded = false;
        this.blockUI.start();
        this.departmentService.getSelectListItem().subscribe({
            next: (response) => {
                this.departments = response;
                if (this.model.departmentId > 0) {
                    this.department = this.departments.find(x => x.key == this.model.departmentId).value;
                }
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getDesignationList(): void {
        this.isModelLoaded = false;
        this.blockUI.start();
        this.designationService.getSelectListItem().subscribe({
            next: (response) => {
                this.designations = response;
                if (this.model.designationId > 0) {
                    this.designation = this.designations.find(x => x.key == this.model.designationId).value;
                }
                this.blockUI.stop();
                this.isModelLoaded = true;
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

    submit(): void {
        this.isModelLoaded = false;
        this.blockUI.start();
        this.model.from = AppUtils.getFormattedDate(this.model.from, null);
        this.model.from = AppUtils.getDate(this.model.from);
        if (this.model.to != null) {
            this.model.to = AppUtils.getFormattedDate(this.model.to, null);
            this.model.to = AppUtils.getDate(this.model.to);
        }
        this.service.update(this.model).subscribe({
            next: () => {
                this.baseService.successNotification('Work history has been updated successfully');
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.dialogRef.close();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }
}