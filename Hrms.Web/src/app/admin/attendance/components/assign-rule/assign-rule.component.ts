import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { AttendanceRuleService, EmployeeService } from "src/app/admin/services";
import { EmployeeAttendanceRuleService } from "src/app/admin/services/employee-attendance.rule.service";
import { EmployeeAttendanceModel } from "src/app/admin/attendance/model";
import { SelectListItemModel } from "src/app/shared/models";

@Component({
    selector: 'app-assign-rule',
    templateUrl: './assign-rule.component.html',
})
export class AssignRuleComponent implements OnInit {

    @BlockUI('blockui-assign-rule') blockUI: NgBlockUI;

    model = new EmployeeAttendanceModel();
    employees = new Array<SelectListItemModel>();
    attendanceRules = new Array<SelectListItemModel>();
    isModelLoaded: boolean;
    employee: string;
    rule: string;
    ruleId: number;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private service: EmployeeAttendanceRuleService,
        private employeeService: EmployeeService,
        private ruleService: AttendanceRuleService,
        private baseService: BaseService,
        private dialogRef: MatDialogRef<AssignRuleComponent>) {
        this.isModelLoaded = false;

    }
    ngOnInit(): void {
        this.getAttendanceRuleList();
    }

    getEmployeeList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.employeeService.getSelectListItemByAttendanceRuleId(this.ruleId).subscribe({
            next: (response) => {
                this.employees = response;
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getAttendanceRuleList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.ruleService.getSelectListItem().subscribe({
            next: (response) => {
                this.attendanceRules = response;
                this.onRuleIdChange();
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

    onRuleIdChange(): void {
        this.ruleId = this.model.ruleId;
        this.getEmployeeList();
    }

    cancel(): void {
        this.dialogRef.close();
    }

    submit(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.add(this.model).subscribe({
            next: () => {
                this.baseService.successNotification('Rule has been assigned successfully');
                this.dialogRef.close();
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
