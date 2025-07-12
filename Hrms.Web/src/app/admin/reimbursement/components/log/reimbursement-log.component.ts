import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { ReimbursementModel } from "../../models";
import { MatDialog } from "@angular/material/dialog";
import { DeleteComponent, RejectComponent } from "src/app/shared/dialog";
import { AppUtils, Constants } from "src/app/utilities";
import { ReimbursementService } from "src/app/admin/services/reimbursement.service";
import { BaseService } from "src/app/shared/services";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { FilterModel } from "src/app/shared/models";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { environment } from "src/environments/environment";
import { ReimbursementChangeStatusModel } from "src/app/admin/reimbursement/models/reimbursement-change-status.model";
import { ApproveReimbursementComponent } from "src/app/admin/reimbursement/components/approve-reimbursement/approve-reimbursement.component";


@Component({
    selector: 'app-reimbursement-log',
    templateUrl: './reimbursement-log.component.html'
})
export class ReimbursementLogComponent implements AfterViewInit {

    @BlockUI('reimbursement-log-blockUI') blockUI: NgBlockUI;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    displayedColumns: string[] = ["employeeName", "description", "amount", "date", "paymentDate", "status", "action"]

    model = new Array<ReimbursementModel>();
    changeStatusModel = new ReimbursementChangeStatusModel();
    filterModel = new FilterModel();
    totalCount: number;
    isModelLoaded: boolean;

    get constants(): typeof Constants {
        return Constants;
    }

    constructor(private dialog: MatDialog,
        private reimbursementService: ReimbursementService,
        private baseService: BaseService) {
        this.isModelLoaded = false;
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.filterModel.pageIndex = 0;
            this.getReimbursements();
        });

        this.paginator.page.subscribe(() => {
            this.getReimbursements();
        });

        this.getReimbursements();
    }

    getReimbursements(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterKey();
        this.reimbursementService.getList(this.filterModel).subscribe({
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
        })
    }

    searchFilterKey(): void {
        this.getReimbursements();
        this.paginator.firstPage();
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null; 
        this.sort.direction = "desc";
        this.sort.active = "createdOn";
        this.paginator.firstPage();
        this.getReimbursements();
    }

    updateFilterKey(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
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
                this.baseService.successNotification("Reimbursement status changed successfully.");
                this.getReimbursements();
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
                this.changeStatusModel.id = id
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
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatusModel.remark = res.log;
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
}