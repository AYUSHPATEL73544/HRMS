import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { FilterModel } from 'src/app/shared/models';
import { LeaveRuleModel } from 'src/app/admin/leave/models';
import { AddLeaveRuleComponent } from 'src/app/admin/leave/components/leave-rule/upsert/add-leave-rule.component';
import { DeleteComponent } from 'src/app/shared/dialog/delete-dialog/delete.component';
import { Constants } from 'src/app/utilities';

@Component({
  selector: 'app-leave-rule-manage',
  templateUrl: './leave-rule-manage.component.html',
})

export class LeaveRuleManageComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @BlockUI('leave-blockui') blockUI: NgBlockUI;

  model = new Array<LeaveRuleModel>();
  filterModel = new FilterModel();
  isModelLoaded: boolean = false;
  columns = ['title', 'people', 'action'];

  constructor(
    private dialog: MatDialog,
    private baseService: BaseService,
    private service: LeaveRuleService
  ) {
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.getLeaveRuleList();
    });
    this.getLeaveRuleList();
  }

  addRule(): void {
    const dialRef = this.dialog.open(AddLeaveRuleComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(() => {
      this.getLeaveRuleList();
    })
  }

  editRule(id: number): void {
    const dialRef = this.dialog.open(AddLeaveRuleComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id }
    });
    dialRef.afterClosed().subscribe(() => {
      this.getLeaveRuleList();
    })
  }

  getLeaveRuleList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.updateFilterModel();
    this.service.getPagedList(this.filterModel).subscribe({
      next: (response) => {
        this.model = response.items;
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
        this.blockUI.stop();
        this.isModelLoaded = true;
      }
    });
  }

  deleteRule(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected rule.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.isModelLoaded = false;

          this.service.deleteRule(id).subscribe({
            next: () => {
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.baseService.successNotification('Rule has been deleted successfully.');
              this.getLeaveRuleList();
            },
            error: (error: any) => {
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.baseService.processErrorResponse(error);
            }
          });
        }
      }
    );
  }


  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getLeaveRuleList();
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
    this.filterModel.sort = 'createdOn';
    this.getLeaveRuleList();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
  }

}