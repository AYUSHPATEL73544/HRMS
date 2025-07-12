import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Constants } from 'src/app/utilities';
import { UpsertLeaveCountComponent } from '../upsert/upsert-leave-count.component';

@Component({
  selector: 'app-leaves-manage',
  templateUrl: './leaves-manage.component.html',
})

export class LeavesManageComponent {
  @ViewChild('logs') leaveLogsComponent: any;
  @ViewChild('leaveRules') leaveRulesComponent: any;
  @ViewChild('leaveBalance') leaveBalanceComponent: any;
  @ViewChild('asignLeaveRule') asignLeaveRuleComponent: any;

  selectedTabIndex = 0;

  constructor(private route: ActivatedRoute,
    private dialog: MatDialog) {
    this.route.queryParams.subscribe((queryParams) => {
      if (queryParams['t']) {
        this.selectedTabIndex = Number.parseInt(queryParams['t']);
      }
    });
  }

  onTabChanged(event: any): void {
    this.selectedTabIndex = event;

    switch (event.index) {
      case 0:
        this.leaveLogsComponent.getLeaveLogs();
        break;
      case 1:
        this.leaveRulesComponent.getLeaveRule();
        break;
      case 3:
        this.leaveBalanceComponent.getList();
        break;
      case 4:
        this.asignLeaveRuleComponent.getDetail();
        break;
    }

  }

  addLeaveCount(): void {
    const dialRef = this.dialog.open((UpsertLeaveCountComponent), {
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
  }
}