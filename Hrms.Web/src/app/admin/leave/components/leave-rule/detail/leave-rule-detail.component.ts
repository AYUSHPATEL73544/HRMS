import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { LeaveRuleModel } from 'src/app/admin/leave/models/index';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-leave-rule-detail',
  templateUrl: './leave-rule-detail.component.html',
})

export class LeaveRuleDetailComponent implements OnInit {
  @BlockUI('leave-blockui') blockUI: NgBlockUI;

  model = new LeaveRuleModel();
  isLeaveCountEditable = false;
  isApplicabilityEditable = false;
  isCarryForwardEditable = false;
  isMiscellaneousEditable = false;
  isAccrualEditable = false;
  isModelLoaded: boolean;
  accrualFrequency: string;
  accrualPeriod: string;
  accrualFrequencyDropDown = AppUtils.AccrualFrequency();
  accrualPeriodDropDown = AppUtils.AccrualPeriod();
  months = AppUtils.getMonthsForDropDown();
  month: string;

  @Output() load = new EventEmitter();

  constructor(private dialog: MatDialog,
    private route: ActivatedRoute,
    private leaveService: LeaveRuleService,
    private baseService: BaseService) {
    this.isModelLoaded = false;
    this.route.params.subscribe((param) => {
      this.model.id = param['id'];
    })
  }

  ngOnInit(): void {
    this.getLeaveRule(this.model.id);
  }

  cancel(): void {
    this.load.emit();
    this.isLeaveCountEditable = false;
    this.isApplicabilityEditable = false;
    this.isMiscellaneousEditable = false;
    this.isAccrualEditable = false;
    this.getLeaveRule(this.model.id);
  }

  getLeaveRule(id: number): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.leaveService.getRule(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
        if (this.model.accrualFrequency != 0) {
          this.accrualFrequency = this.accrualFrequencyDropDown.find(x => x.key == this.model.accrualFrequency).value;
        }
        if (this.model.accrualPeriod != 0) {
          this.accrualPeriod = this.accrualPeriodDropDown.find(x => x.key == this.model.accrualPeriod).value;
        }
        if (this.model.applyTillNextYear != 0) {
          this.month = this.months.find(x => x.key == this.model.applyTillNextYear).value;
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

  submit(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.allowLeaves();
    this.leaveService.updateRule(this.model).subscribe({
      next: (res: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.load.emit();
        this.baseService.successNotification('Leave rule details has been updated successfully');
        this.isLeaveCountEditable = false;
        this.isApplicabilityEditable = false;
        this.isMiscellaneousEditable = false;
        this.isAccrualEditable = false;
        this.getLeaveRule(this.model.id);
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      }
    })
  }

  allowLeaves(): void {
    if (!this.model.allowedBackDatedLeaves) {
      this.model.maxBackDatedLeavesAllowed = 0;
    }
    if (!this.model.futureDatedLeavesAllowed) {
      this.model.futureDatedLeavesAllowedUpTo = 0;
    }
    if (!this.model.creditableOnAccrualBasis) {
      this.model.accrualFrequency = 0;
      this.model.accrualPeriod = 0;
    }
  }



}

