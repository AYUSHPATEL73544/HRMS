import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeleteComponent } from 'src/app/shared/dialog';
import { Constants } from 'src/app/utilities';
import { UpsertReimbursementComponent } from '../upsert/upsert-reimbursement.component';
import { ReimbursementService } from 'src/app/employee/services/reimbursement.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { BaseService } from 'src/app/shared/services';
import { ReimbursementModel } from 'src/app/employee/reimbursement/models';
import { FilterModel } from 'src/app/shared/models';
import { environment } from 'src/environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ReimbursementChangeStatusModel } from 'src/app/employee/reimbursement/models/reimbursement-change-status.model';

@Component({
    selector: 'app-reimbursement',
    templateUrl: './reimbursement-manage.component.html'
})

export class ReimbursementComponent implements AfterViewInit {
    @BlockUI('reimbursement-blockui') blockUI: NgBlockUI;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    model = new Array<ReimbursementModel>();
    filterModel = new FilterModel();
    changeStatusModel = new ReimbursementChangeStatusModel();
    isModelLoaded: boolean;
    totalCount: number;

    displayedColumns: string[] = ['description', 'amount', 'date', 'status', 'paymentDate', 'action'];

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

    getReimbursements() {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterKey();
        this.reimbursementService.getPageList(this.filterModel).subscribe({
            next: (res) => {
                this.model = res.items;
                this.totalCount = res.totalCount;
                this.isModelLoaded = true;
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.errorNotification(error);
            }
        });
    }

    updateFilterKey(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }

    searchFilterKey(): void {
        this.getReimbursements();
        this.paginator.firstPage();
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.sort.direction = "desc";
        this.sort.active = "createdOn"
        this.paginator.firstPage();
        this.getReimbursements();
    }

    viewReceipt(key: string): void {
        window.open(environment.apiBaseUrl + '/documents/' + key, '_blank');
    }

    add() {
        const dialogRef = this.dialog.open(UpsertReimbursementComponent, {
            data: {
                title: "Add Reimbursement",
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe(() => {
            this.getReimbursements();
        });
    }

    edit(id: number): void {
        const dialogRef = this.dialog.open(UpsertReimbursementComponent, {
            data: {
                id: id,
                title: "Edit Reimbursement",
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });

        dialogRef.afterClosed().subscribe(() => {
            this.getReimbursements();
        });
    }

    delete(id: number, status: number): void {
        const dialogRef = this.dialog.open(DeleteComponent, {
            data: {
                title: "Delete",
                message: "Are you sure you want to delete the selected Reimbursement?"
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });

        dialogRef.afterClosed().subscribe(res => {
            if (res) {
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatus();
            }
        });
    }

    changeStatus(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.reimbursementService.toggleStatus(this.changeStatusModel).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.successNotification("Deleted successfully.");
                this.getReimbursements();
            },
            error: (error) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }
}