import { Component, Inject, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { LeaveLogService, LeaveService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { LeaveRuleService } from 'src/app/employee/services/leave-rule.service';
import { LeaveLogModel, LeaveModel, LeaveRuleModel, TotalLeaveCountModel } from 'src/app/employee/leave/models/index';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-upsert-leaves',
  templateUrl: './upsert-leaves.component.html',
})

export class UpsertLeavesComponent implements OnInit {
  @BlockUI('apply-leaves-blockui') blockUI: NgBlockUI;

  model = new LeaveLogModel();
  leaveModel = new LeaveModel();
  leaveRuleModel = new LeaveRuleModel();
  totalLeaveCountModel = new TotalLeaveCountModel();
  totalLeavesCount: number;
  id = 0;
  logId = 0;
  showError = false;
  daysDifference: number;
  ruleId: number;
  isSameDate: any;
  appliedForHalfDay = false;
  weeekendDays = AppUtils.getWeekDaysForDropDown();
  minDate: any;
  maxDate: any;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertLeavesComponent>,
    private leaveLogService: LeaveLogService,
    private leaveService: LeaveService,
    private leaveRuleService: LeaveRuleService,
    private leavelogService: LeaveLogService,
    private baseService: BaseService) {
    if (data) {
      this.id = data.id;
      this.logId = data.logId;
      this.minDate = data.minDate;
      this.maxDate = data.maxDate;
    }
  }


  ngOnInit(): void {
    if (this.id) {
      this.getRule();
    }
    if (this.logId) {
      this.getLeaveLog();
    }
  }

  getRule(): void {
    this.blockUI.start();
    this.leaveRuleService.getRule(this.id).subscribe({
      next: (response) => {
        this.leaveRuleModel = response;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getLeaveLog(): void {
    this.blockUI.start();
    this.leaveLogService.getDetail(this.logId).subscribe({
      next: (response) => {
        this.model = response;
        this.blockUI.stop();
        this.getLeave();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  checkContinuousDays(): void {
    if (this.leaveRuleModel.countWeekendAsLeave) {
      this.daysDifference = AppUtils.getDifferenceInDays(this.model.startDate, this.model.endDate) + 1;
    }
    else {
      let startDay = new Date(this.model.startDate);
      let endDay = new Date(this.model.endDate);
      let daysDifference = 0;
      while (startDay <= endDay) {
        if (startDay.getDay() !== 0 && startDay.getDay() !== 6) {
          daysDifference++;
        }
        startDay.setDate(startDay.getDate() + 1);
      }
      this.daysDifference = daysDifference;
    }
    if (this.daysDifference > this.leaveRuleModel.maxAllowedContinues) {
      this.showError = true;
    }
    else {
      this.showError = false;
    }
  }

  isEndDateSame(): boolean {
    const start = new Date(this.model.startDate);
    const end = new Date(this.model.endDate);
    if (start.getTime() === end.getTime()) {
      this.isSameDate = true;
    } else {
      this.isSameDate = false;
    }
    this.checkContinuousDays();
    return this.model.startDate !== null && this.model.endDate !== null && this.model.startDate == this.model.endDate;
  }


  getLeave(): void {
    this.blockUI.start();
    this.leaveService.getByRuleId(this.model.ruleId).subscribe({
      next: (response) => {
        this.leaveModel = response;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  totalLeaveCount(): number {
    if (this.model.startDate && this.model.endDate) {
      this.blockUI.start();

      this.totalLeaveCountModel.ruleId = this.id ? this.id : this.leaveModel.ruleId;
      if (this.model.startDate) {
        this.totalLeaveCountModel.startDate = AppUtils.getFormattedDate(this.model.startDate, null);
        this.totalLeaveCountModel.startDate = AppUtils.getDate(this.totalLeaveCountModel.startDate);
      }
      if (this.model.endDate) {
        this.totalLeaveCountModel.endDate = AppUtils.getFormattedDate(this.model.endDate, null);
        this.totalLeaveCountModel.endDate = AppUtils.getDate(this.totalLeaveCountModel.endDate);
      }
      this.leaveLogService.getTotalLeaveCount(this.totalLeaveCountModel).subscribe({
        next: (response) => {
          this.totalLeavesCount = response;
          this.blockUI.stop();
        },
        error: (error) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
    return 0;
  }

  submit(): void {
    this.blockUI.start();
    this.model.startDate = AppUtils.getFormattedDate(this.model.startDate, null);
    this.model.startDate = AppUtils.getDate(this.model.startDate);
    this.model.endDate = AppUtils.getFormattedDate(this.model.endDate, null);
    this.model.endDate = AppUtils.getDate(this.model.endDate);

    this.daysDifference = AppUtils.getDifferenceInDays(this.model.startDate, this.model.endDate) + 1;
    if (this.leaveModel.available < this.daysDifference) {
      this.baseService.errorNotification('Insufficient Leave Balance, please check and re-apply');
    }
    else {
      if (this.model.id) {
        this.leavelogService.update(this.model).subscribe({
          next: () => {
            this.blockUI.stop();
            this.baseService.successNotification('Leave has been updated successfully');
            this.dialogRef.close();
          },
          error: (error: any) => {
            this.blockUI.stop();
            this.baseService.processErrorResponse(error);
          }
        });
      }
      else {
        this.model.ruleId = this.id;
        this.leavelogService.addLeave(this.model).subscribe({
          next: () => {
            this.blockUI.stop();
            this.baseService.successNotification('Leave has been applied successfully.');
            this.dialogRef.close();
          },
          error: (error: any) => {
            this.blockUI.stop();
            this.baseService.processErrorResponse(error);
          }
        });
      }
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

}