import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeService, LeaveService, } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { LeaveRuleService } from 'src/app/admin/services/leave-rule.service';
import { LeaveModel } from 'src/app/admin/leave/models';
import { SelectListItemModel } from 'src/app/shared/models';

@Component({
  selector: 'app-upsert-leave-count',
  templateUrl: './upsert-leave-count.component.html',
})
export class UpsertLeaveCountComponent implements OnInit {

  model = new LeaveModel();
  employees = new Array<SelectListItemModel>();
  leaveRules = new Array<SelectListItemModel>();
  employee: string;
  leaveRule: string;
  ruleId: number;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private employeeService: EmployeeService,
    private service: LeaveService,
    private leaveRuleService: LeaveRuleService,
    private baseService: BaseService,
    private dialogRef: MatDialogRef<UpsertLeaveCountComponent>) { }

  ngOnInit(): void {
    this.getLeaveRuleList();
  }

  getEmployeeList(): void {
    this.employeeService.getEmployeeSelectListItem(this.ruleId).subscribe({
      next: (response) => {
        this.employees = response;
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  onLeaveRuleChange(): void {
    this.ruleId = this.model.ruleId;
    this.getEmployeeList();
  }

  getLeaveRuleList(): void {
    this.leaveRuleService.getSelectListItem().subscribe({
      next: (response) => {
        this.leaveRules = response;
        this.onLeaveRuleChange();
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  submit(): void {
    this.service.addLeaveRule(this.model).subscribe({
      next: () => {
        this.baseService.successNotification('Rule has been assigned successfully');
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
