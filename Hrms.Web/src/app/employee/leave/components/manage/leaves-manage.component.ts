import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-leaves-manage',
  templateUrl: './leaves-manage.component.html',
})

export class LeavesManagerComponent {

  @BlockUI('blockui-leave-manage') blockUI: NgBlockUI;
  selectedTabIndex = 0;
  userRole: string;
  isManager = false;
  
  get constants(): typeof Constants {
    return Constants;
  }
  constructor(private route: ActivatedRoute,
    public appUtils: AppUtils,) {
    this.route.queryParams.subscribe((queryParams) => {
      if (queryParams['t']) {
        this.selectedTabIndex = Number.parseInt(queryParams['t']);
      }
    });
    this.userRole = AppUtils.getUserRole();
  }

  onTabChanged(index: number): void {
    this.selectedTabIndex = index;
  }
}