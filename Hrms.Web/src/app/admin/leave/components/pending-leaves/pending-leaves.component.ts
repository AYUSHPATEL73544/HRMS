import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AppUtils, Constants } from 'src/app/utilities';
import { LeaveLogChangeStatus, LeaveLogFilterModel, LeaveLogModel } from 'src/app/admin/leave/models';
import { LeaveLogService } from 'src/app/admin/services/leave-log.services';
import { BaseService } from 'src/app/shared/services';
import { RejectComponent } from 'src/app/shared/dialog/reject-with-log/reject.component';
import { LeaveApproveDialogComponent } from '../approve-dialog/leave-approve-dialog.component';
import { LeaveLogDetailsComponent } from '../leave-log-details/leave-log-details.component';

@Component({
    selector: 'app-leave-pending-leaves',
    templateUrl: './pending-leaves.component.html',
})

export class PendingLeavesComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('leave-log-blockui') blockUI: NgBlockUI;

    model = new Array<LeaveLogModel>();
    changeStatusModel = new LeaveLogChangeStatus();
    filterModel = new LeaveLogFilterModel();
    isModelLoaded: boolean;
    totalCount: number;

    get constants(): typeof Constants { return Constants; }

    constructor(
        private baseService: BaseService,
        private service: LeaveLogService,
        private dialog: MatDialog) {
        this.isModelLoaded = false;
    }

    columns = ['id', 'employeeName', 'leaveType', 'startDate', 'endDate', 'days', 'createdOn', 'status', 'action'];

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
        if(this.filterModel.filterKey){
            this.paginator.firstPage();
        }
        this.service.getPendingLeavesList(this.filterModel).subscribe({
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

    approve(id: number, status: number, element : LeaveLogModel): void {
        const dialogRef = this.dialog.open(LeaveApproveDialogComponent, {
            data: {
                title: `Approve`,
                message: `Are you sure you want to approve the selected leave log?`,
                model:element,
            }
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatus(this.changeStatusModel);
            }
        });
    }

    reject(id: number, status: number): void {
        const dialogRef = this.dialog.open(RejectComponent, {
            data: {
                title: `Reject`,
                message: `Are you sure you want to reject the selected leave log?`,
                status: `4`
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result.confirm) {
                if (result.log != null) {
                    this.changeStatusModel.rejectionReason = result.log;
                }
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatus(this.changeStatusModel);
            }
        });
    }

    viewDetails(element: LeaveLogModel): void {
        const dialogRef = this.dialog.open(LeaveLogDetailsComponent, {
            data: {
                title: 'Leave Details',
                status: '2',
                model: element
            },
            width: Constants.dialogSize.medium
        });
        dialogRef.afterClosed().subscribe(() => {
            this.getLeaveLogs();
        });
    }

    changeStatus(model: LeaveLogChangeStatus): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.service.changeStatus(model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.successNotification('Leave log status has been changed successfully.');
                this.getLeaveLogs();
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
        this.paginator.firstPage();
    }

    resetFilter(): void {
        this.filterModel = new LeaveLogFilterModel();
        this.getLeaveLogs();
        this.paginator.firstPage();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }

}

