import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/employee/services/leave-rule.service';
import { BaseService } from 'src/app/shared/services';
import { LeaveService } from 'src/app/employee/services';
import { LeaveModel } from 'src/app/employee/leave/models';
import { SelectListItemModel } from 'src/app/shared/models/select-list-item.model';
import { UpsertLeavesComponent } from '../upsert/upsert-leaves.component';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-stats-detail',
  templateUrl: './stats-detail.component.html',
})

export class StatsDetailsComponent implements OnInit {

  @BlockUI('stats-blockui') blockUI: NgBlockUI;

  model = new Array<LeaveModel>();
  leaveRules = new Array<SelectListItemModel>();
  avaialable: number;
  maxDate: any;
  minDate: any;
  isModelLoaded: boolean;
  constructor(private service: LeaveService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private leaveRuleService: LeaveRuleService,
    private baseService: BaseService) {
    this.isModelLoaded = false;
  }

  ngOnInit(): void {
    this.getLeaveList();
  }

  getLeaveList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.service.getList().subscribe({
      next: (response) => {
        this.model = response;
        this.model.forEach(element => {
          this.minDate = element.minDate,
          this.maxDate = element.maxDate
        });
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.getLeaveRuleList();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getLeaveRuleList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.leaveRuleService.getSelectListItem().subscribe({
      next: (response) => {
        this.leaveRules = response;
        this.model.forEach(element => {
          if (element.ruleId > 0) {
            const rule = this.leaveRules.find(x => x.key == element.ruleId).value;
            if (rule) {
              element.ruleName = rule;
            }
          }
        });
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

  addLeave(id: number, minDate: any, maxDate: any): void {
    this.blockUI.start();
    const dailRef = this.dialog.open(UpsertLeavesComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        id: id,
        minDate: minDate,
        maxDate: maxDate,
      }
    });
    dailRef.afterClosed().subscribe(() => {
      this.blockUI.stop();
      this.getLeaveList();
    });
  }
}