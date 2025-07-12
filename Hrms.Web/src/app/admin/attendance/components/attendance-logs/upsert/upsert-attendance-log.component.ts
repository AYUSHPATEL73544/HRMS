import { Component, Inject, OnInit } from "@angular/core"
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import * as moment from "moment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { AttendanceLogService, EmployeeService } from "src/app/admin/services";
import { AttendanceLogModel } from "src/app/admin/attendance/model/index";
import { SelectListItemModel } from "src/app/shared/models";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-upsert-attendance-log',
    templateUrl: './upsert-attendance-log.component.html',
})

export class UpsertAttendanceLogComponent implements OnInit {
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;

    model = new AttendanceLogModel();
    longitudeReadOnly = false;
    employees = new Array<SelectListItemModel>();
    employee: string;
    isInvalidInTime: boolean = false;
    isInvalidOutTime: boolean = false;


    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpsertAttendanceLogComponent>,
        private attendanceLogService: AttendanceLogService,
        private employeeService: EmployeeService,
        private baseService: BaseService,
        private appUtils: AppUtils
    ) {
        if (data) {
            this.model.id = data.id;
        }
    }

    ngOnInit(): void {
        this.getEmployeeList();
        if (this.model.id) {
            this.getAttendanceLogDetail(this.model.id);
        }
    }

    getAttendanceLogDetail(id: number): void {
        this.blockUI.start();
        this.attendanceLogService.getDetail(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.model.inTime = this.appUtils.getUtcToLocalTime(this.model.inTime);
                this.model.inTime = AppUtils.getTime(this.model.inTime);
                if (this.model.outTime) {
                    this.model.outTime = this.appUtils.getUtcToLocalTime(this.model.outTime);
                }
                this.model.outTime = AppUtils.getTime(this.model.outTime);
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }

    cancel(): void {
        this.dialogRef.close();
    }

    submit(): void {
        this.model.inTime = AppUtils.getFormattedLocalTime(this.model.inTime);
        if (this.model.outTime) {
            this.model.outTime = AppUtils.getFormattedLocalTime(this.model.outTime);
        }
        if (this.model.id) {
            this.blockUI.start();
            this.model.inTime = this.appUtils.getLocalToUtcTime(this.model.inTime);
            if (this.model.outTime) {
                this.model.outTime = this.appUtils.getLocalToUtcTime(this.model.outTime);
            }
            this.attendanceLogService.updateAttendanceLog(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification('Attendance log has been updated successfully.');
                    this.getAttendanceLogDetail(this.model.id);
                    this.dialogRef.close();
                    this.blockUI.stop();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
        else {
            this.blockUI.start();
            this.model.inTime = this.appUtils.getLocalToUtcTime(this.model.inTime);
            if (this.model.outTime) {
                this.model.outTime = this.appUtils.getLocalToUtcTime(this.model.outTime);
            }
            this.model.date = AppUtils.getFormattedDate(this.model.date, null);
            this.model.date = AppUtils.getDate(this.model.date);
            this.attendanceLogService.addAttendanceLog(this.model).subscribe({
                next: () => {
                    this.baseService.successNotification('Attendance log has been added successfully.');
                    this.dialogRef.close();
                    this.blockUI.stop();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
    }

    getEmployeeList(): void {
        this.blockUI.start();
        this.employeeService.getSelectListItem().subscribe({
            next: (response) => {
                this.employees = response;
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    checkInTime(): void {
        const inTimeFormatted = moment(this.model.inTime, 'h:mm A').format('HH:mm:ss.SSSS');
        const graceInTimeFormatted = moment(this.model.inTime, 'h:mm A').format('HH:mm:ss.SSSS');
        this.isInvalidInTime = inTimeFormatted > graceInTimeFormatted;
    }

    checkOutTime(): void {
        const outTimeFormatted = moment(this.model.outTime, 'h:mm A').format('HH:mm:ss.SSSS');
        const graceOutTimeFormatted = moment(this.model.outTime, 'h:mm A').format('HH:mm:ss.SSSS');
        this.isInvalidOutTime = outTimeFormatted > graceOutTimeFormatted;
    }
}