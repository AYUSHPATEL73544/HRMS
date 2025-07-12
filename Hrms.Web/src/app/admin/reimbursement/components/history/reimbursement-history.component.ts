import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ReimbursementService } from 'src/app/admin/services/reimbursement.service';
import { ReimbursementModel } from 'src/app/admin/reimbursement/models/reimbursement.model';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { BaseService } from 'src/app/shared/services';
import { AppUtils, Constants } from 'src/app/utilities';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ReimbursementChangeStatusModel } from 'src/app/admin/reimbursement/models/reimbursement-change-status.model';
import { MatDialog } from '@angular/material/dialog';
import { DeleteComponent, DialogConfirmComponent, RejectComponent } from 'src/app/shared/dialog';
import { ReimbursementFilterModel } from 'src/app/admin/reimbursement/models/reimbursement-filter.model';
import { EmployeeService } from 'src/app/admin/services';
import { ApproveReimbursementComponent } from 'src/app/admin/reimbursement/components/approve-reimbursement/approve-reimbursement.component';

@Component({
    selector: 'app-reimbursement-history',
    templateUrl: './reimbursement-history.component.html'
})

export class ReimbursementHistoryComponent implements AfterViewInit {
    @BlockUI('reimbursement-history-blockui') blockUI: NgBlockUI;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    model = new Array<ReimbursementModel>();
    filterModel = new ReimbursementFilterModel();
    changeStatusModel = new ReimbursementChangeStatusModel();
    totalCount: number;
    isModelLoaded: boolean;
    employeeId: number;
    employeeName: string;

    displayedColumns: string[] = ["description", "amount", "date", "paymentDate", "status", "action"]

    get constants(): typeof Constants {
        return Constants;
    }

    constructor(private activatedRoute: ActivatedRoute,
        private reimbursementService: ReimbursementService,
        private baseService: BaseService,
        private employeeService: EmployeeService,
        private dialog: MatDialog) {
        this.isModelLoaded = false;
        this.employeeId = activatedRoute.snapshot.params['id'];
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.filterModel.pageIndex = 0;
            this.getName();
            this.getAllHistory();
        });

        this.paginator.page.subscribe(() => {
            this.getName();
            this.getAllHistory();
        });

        this.getName();
        this.getAllHistory();
    }

    getName(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.employeeService.getNameById(this.employeeId).subscribe({
            next: (res) => {
                this.blockUI.stop();
                this.employeeName = res;
                this.isModelLoaded = true;
            },
            error: (error) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getAllHistory(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterKey();
        if (this.filterModel.startDate) {
            this.filterModel.startDate = AppUtils.getFormattedDate(this.filterModel.startDate, null);
            this.filterModel.startDate = AppUtils.getDate(this.filterModel.startDate);
        }
        if (this.filterModel.endDate) {
            this.filterModel.endDate = AppUtils.getFormattedDate(this.filterModel.endDate, null);
            this.filterModel.endDate = AppUtils.getDate(this.filterModel.endDate);
        }
        this.reimbursementService.getPageListById(this.employeeId, this.filterModel).subscribe({
            next: (res) => {
                this.totalCount = res.totalCount;
                this.model = res.items;
                this.isModelLoaded = true;
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    viewReceipt(key: string): void {
        window.open(environment.apiBaseUrl + '/documents/' + key, '_blank');
    }

    toggleStatus(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.reimbursementService.toggleStatus(this.changeStatusModel).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.getAllHistory();
                this.baseService.successNotification("Reimbursement status changed successfully.");
            },
            error: (error) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    approve(id: number, status: number): void {
        const dialogRef = this.dialog.open(ApproveReimbursementComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe(res => {
            if (res.confirm) {
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatusModel.remark = res.remark;
                this.changeStatusModel.paymentDate = AppUtils.getFormattedDate(res.paymentDate, null);
                this.changeStatusModel.paymentDate = AppUtils.getDate(res.paymentDate);
                this.toggleStatus();
            }
        });
    }

    reject(id: number, status: number): void {
        const dialogRef = this.dialog.open(RejectComponent, {
            data: {
                title: 'Reject',
                message: 'Are you sure you want to reject the selected Reimbursement?',
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe(res => {
            if (res.confirm) {
                if (res.log != null) {
                    this.changeStatusModel.remark = res.log;
                }
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.toggleStatus();
            }
        });
    }

    delete(id: number, status: number): void {
        const dialogRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete the selected Reimbursement?'
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.toggleStatus();
            }
        });
    }

    searchFilterKey(): void {
        this.getAllHistory();
        this.paginator.firstPage();
    }

    resetFilterKey(): void {
        this.filterModel = new ReimbursementFilterModel();
        this.sort.active = "createdOn";
        this.sort.direction = "desc";
        this.paginator.firstPage();
        this.getAllHistory();
    }

    updateFilterKey(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }
}