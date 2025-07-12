import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AttendanceRuleService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { AttendanceRuleModel } from "src/app/admin/attendance/model/index";
import { AppUtils } from "src/app/utilities/app.util";
import { Constants } from "src/app/utilities";

@Component({
    selector: 'app-upsert-attendance-rule-detail',
    templateUrl: './upsert-attendance-rule.component.html',
})

export class UpsertAttendanceRuleDetailComponent implements OnInit {
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;

    model = new AttendanceRuleModel();
    weekDays = AppUtils.getWeekDaysForDropDown();
    years = AppUtils.getYears();
    isModelLoaded: boolean;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dailogRef: MatDialogRef<UpsertAttendanceRuleDetailComponent>,
        private attendanceService: AttendanceRuleService,
        private baseService: BaseService,
        private appUtils: AppUtils) {
        this.isModelLoaded = false;
        if (data) {
            this.model.id = data.id;
        }
        this.model.companyId = this.appUtils.getCompanyId();
    }

    ngOnInit(): void {
        if (this.model.id) {
            this.getAttendanceDetail(this.model.id);
        }
    }

    cancel(): void {
        this.dailogRef.close();
    }

    getAttendanceDetail(id: number): void {
        this.isModelLoaded = false;
        this.blockUI.start();
        this.attendanceService.getDetail(this.model.id).subscribe({
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


    submit(): void {
        if (this.model.id) {
            this.isModelLoaded = false;
            this.blockUI.start();
            this.attendanceService.updateAttendanceRule(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification('Attendance rule has been updated successfully.');
                    this.dailogRef.close();
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
        else {
            this.isModelLoaded = false;
            this.blockUI.start();
            this.attendanceService.addAttendanceRule(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification('Attendance rule has been added successfully.');
                    this.dailogRef.close();
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
    }
}