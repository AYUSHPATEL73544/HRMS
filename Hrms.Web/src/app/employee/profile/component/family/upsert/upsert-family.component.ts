import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { FamilyServices } from "src/app/employee/services";
import { BaseService, RelationshipService } from "src/app/shared/services";
import { FamilyModel } from "src/app/employee/profile/models/family.model";
import { SelectListItemModel } from "src/app/shared/models";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-upsert-family',
    templateUrl: './upsert-family.component.html',
})

export class UpsertFamilyComponent implements OnInit {
    @BlockUI('blockui-upsert-family') blockUI: NgBlockUI;

    realtionships = new Array<SelectListItemModel>();
    model = new FamilyModel();
    id = 0;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        public appUtils: AppUtils,
        private dialogRef: MatDialogRef<UpsertFamilyComponent>,
        private service: FamilyServices,
        private baseService: BaseService,
        private realtionshipService: RelationshipService
    ) {
        if (data) {
            this.id = data.id;
        }
    }

    ngOnInit(): void {
        if (this.id) {
            this.get();
        }
        this.loadRealtionshipList();
    }

    get(): void {
        this.blockUI.start();
        this.service.getById(this.id).subscribe({
            next: (response) => {
                this.model = response;
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    loadRealtionshipList(): void {
        this.blockUI.start();
        this.realtionshipService.getSelectListItem().subscribe({
            next: (response) => {
                Object.assign(this.realtionships, response);
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }

    submit(): void {
        this.model.dateOfBirth = AppUtils.getFormattedDate(this.model.dateOfBirth, null);
        this.model.dateOfBirth = AppUtils.getDate(this.model.dateOfBirth);
        this.blockUI.start();
        if (this.model.id) {
            this.service.update(this.model).subscribe({
                next: () => {
                    this.blockUI.stop();
                    this.baseService.successNotification('Family member details has been updated successfully');
                    this.dialogRef.close();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
        else {
            this.blockUI.start();
            this.service.add(this.model).subscribe({
                next: () => {
                    this.blockUI.stop();
                    this.baseService.successNotification('Family member details has been added successfully');
                    this.dialogRef.close();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                    this.dialogRef.close();
                }
            });
        }
    }
}