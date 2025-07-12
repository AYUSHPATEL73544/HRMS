import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { LeaveLogService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { LeaveLogFilterModel, LeaveLogModel } from 'src/app/employee/leave/models';
import { UpsertLeavesComponent } from '../stats/upsert/upsert-leaves.component';
import { LeaveLogRejectComponent } from '../log-reject-details/leave-log-reject.component';
import { DeleteComponent } from 'src/app/shared/dialog';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-leave-logs',
    templateUrl: './leave-logs.component.html',
})

export class LeaveLogsComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('leave-logs-blockui') blockUI: NgBlockUI;

    model = new Array<LeaveLogModel>();
    days: string;
    filterModel = new LeaveLogFilterModel();
    totalCount: number;
    isModelLoaded: boolean;
    maxDate: any;
    minDate: any;

    get constants(): typeof Constants { return Constants; }

    columns = ['leaveType', 'createdOn', 'startDate', 'endDate', 'days', 'status', 'action'];

    constructor(private baseService: BaseService,
        private dialog: MatDialog,
        private service: LeaveLogService) {
        this.isModelLoaded = false;
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.paginator.pageIndex = 0;
            this.getLeaveLogList();
        });
        this.paginator.page.subscribe(() => {
            this.getLeaveLogList();
        });
        this.getLeaveLogList();
    }

    getLeaveLogList(): void {
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
        if(this.filterModel.filterKey){
            this.paginator.firstPage();
        }
        this.service.getPageList(this.filterModel).subscribe({
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
        })
    }

    deleteLog(id: number): void {
        const dialRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete the selected leave.',
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.service.deleteLog(id).subscribe({
                        next: () => {
                            this.blockUI.stop();
                            this.baseService.successNotification('Leave log has been deleted successfully.');
                            this.getLeaveLogList();
                        },
                        error: (error: any) => {
                            this.blockUI.stop();
                            this.baseService.processErrorResponse(error);
                        }
                    });
                }
            }
        );
    }

    viewRemark(element: LeaveLogModel): void {
        const dialogRef = this.dialog.open(LeaveLogRejectComponent, {
            data: {
                title: 'Log Details',
                status: '4',
                model: element
            },
            width: Constants.dialogSize.medium
        });
        dialogRef.afterClosed().subscribe(() => {
            this.getLeaveLogList();
        });
    }

    editLog(logId: number, minDate: any, maxDate: any): void {
        const dailRef = this.dialog.open(UpsertLeavesComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true,
            data: {
                logId: logId,
                minDate: minDate,
                maxDate: maxDate
            }
        });
        dailRef.afterClosed().subscribe(() => {
            this.getLeaveLogList();
        });
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.paginator.firstPage();
        this.getLeaveLogList();
    }

    resetFilter(): void {
        this.filterModel = new LeaveLogFilterModel();
        this.paginator.firstPage();
        this.getLeaveLogList();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }
}