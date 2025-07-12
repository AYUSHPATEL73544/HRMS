import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/employee/services/leave-rule.service';
import { BaseService } from 'src/app/shared/services';
import { Constants } from 'src/app/utilities';
import { FilterModel } from 'src/app/shared/models';
import { LeaveRuleModel } from 'src/app/admin/leave/models';
import { MatPaginator } from '@angular/material/paginator';


@Component({
  selector: 'app-leave-rule',
  templateUrl: './leave-rule.component.html',
})

export class LeaveRuleComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;  
  @BlockUI('leave-blockui') blockUI: NgBlockUI;

  model = new Array<LeaveRuleModel>();
  filterModel = new FilterModel();
  isModelLoaded: boolean;
  totalCount: number;

  get constants(): typeof Constants { return Constants; }

  columns = ['title', 'action'];

  constructor(
    private baseService: BaseService,
    private leaveService: LeaveRuleService
  ) {
    this.isModelLoaded = false;
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.getLeaveRuleList();
    });
    this.getLeaveRuleList();
  }

  getLeaveRuleList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.updateFilterModel();
    if(this.filterModel.filterKey){
      this.paginator.firstPage();
    }
    this.leaveService.getPageList(this.filterModel).subscribe({
      next: (response) => {
        this.totalCount = response.totalCount;
        this.model = response.items;
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

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.paginator.firstPage();
    this.getLeaveRuleList();
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage();
    this.getLeaveRuleList();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
  }
}