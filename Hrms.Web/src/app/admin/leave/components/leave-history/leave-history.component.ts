import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ActivatedRoute } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { LeaveLogService } from 'src/app/admin/services/leave-log.services';
import { BaseService } from 'src/app/shared/services';
import { LeaveLogChangeStatus, LeaveLogFilterModel, LeaveLogModel } from 'src/app/admin/leave/models';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-leave-history',
    templateUrl: './leave-history.component.html',
})

export class LeaveHistoryComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('blockui-leave-history') blockUI: NgBlockUI;

    model = new Array<LeaveLogModel>();
    changeStatusModel = new LeaveLogChangeStatus();
    filterModel = new LeaveLogFilterModel();
    isModelLoaded: boolean;
    totalCount: number;
    id = 0;
    get constants(): typeof Constants { return Constants; }

    constructor(
        private route: ActivatedRoute,
        private baseService: BaseService,
        private service: LeaveLogService,
        private dialog: MatDialog) {
        this.isModelLoaded = false;
        this.route.params.subscribe((params) => {
            this.id = params['id'];
        });
    }

    columns = ['id', 'employeeName', 'leaveType', 'startDate', 'endDate', 'days', 'createdOn', 'status'];

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.paginator.pageIndex = 0;
            this.getLeaveLogs();
        });
        this.paginator.page.subscribe(() => {
            this.getLeaveLogs();
        });
        this.getLeaveLogs();
    }

    getLeaveLogs(): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.updateFilterModel();
        if (this.filterModel.startDate) {
            this.filterModel.startDate = AppUtils.getFormattedDate(this.filterModel.startDate, null);
            this.filterModel.startDate = AppUtils.getDate(this.filterModel.startDate);
        }
        if (this.filterModel.endDate) {
            this.filterModel.endDate = AppUtils.getFormattedDate(this.filterModel.endDate, null);
            this.filterModel.endDate = AppUtils.getDate(this.filterModel.endDate);
        }
        this.service.getPagedListByEmployeeId(this.filterModel, this.id).subscribe({
            next: (response) => {
                this.model = response.items;
                this.totalCount = response.totalCount;
                
                this.model.forEach(element => {
                    element.createdOn = AppUtils.getLocalFormattedDate(element.createdOn);
                    element.startDate = AppUtils.getLocalFormattedDate(element.startDate);
                    element.endDate = AppUtils.getLocalFormattedDate(element.endDate);
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

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.getLeaveLogs();
    }

    resetFilter(): void {
        this.filterModel = new LeaveLogFilterModel();
        this.getLeaveLogs();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;

    }

}
