import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/employee/services/leave-rule.service';
import { BaseService } from 'src/app/shared/services';
import { LeaveRuleModel } from 'src/app/employee/leave/models/index';
import { AppUtils } from 'src/app/utilities';


@Component({
  selector: 'app-leave-rule-detail',
  templateUrl: './leave-rule-detail.component.html',
})

export class LeaveRuleDetailComponent implements OnInit {

  @BlockUI('leave-rule-detail-blockui') blockUI: NgBlockUI;

  model = new LeaveRuleModel();
  month: string;
  months = AppUtils.getMonthsForDropDown();

  constructor(
    private route: ActivatedRoute,
    private service: LeaveRuleService,
    private baseService: BaseService) {
    this.route.params.subscribe((param) => {
      this.model.id = param['id'];
    })
  }

  ngOnInit(): void {
    this.getLeaveRule(this.model.id);
  }

  getLeaveRule(id: number): void {
    this.blockUI.start();
    this.service.getRule(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
        if (this.model.applyTillNextYear != 0) {
          this.month = this.months.find(x => x.key == this.model.applyTillNextYear).value;
        }
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

}

