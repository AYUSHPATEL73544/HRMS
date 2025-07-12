import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { LeaveService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { LeaveModel } from "src/app/admin/leave/models";
import { FilterModel } from "src/app/shared/models";
import { Constants } from "src/app/utilities";
import { MatDialog } from "@angular/material/dialog";
import { DeleteComponent } from "src/app/shared/dialog";

@Component({
    selector: 'app-leave-balance',
    templateUrl: './leave-balance.component.html',
})

export class LeaveBalanceComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    @BlockUI('leave-balance-blockui') blockUI: NgBlockUI;

    deletedRule = "Deleted rule";
    model = new Array<LeaveModel>();
    columns = ['employeeCode', 'employeeName', 'leaveRule', 'credited', 'applied', 'available', 'total', 'action'];
    filterModel = new FilterModel();
    totalCount: number;
    isModelLoaded: boolean;

    get constants(): typeof Constants { return Constants; }
    constructor(private dialog: MatDialog,
        private service: LeaveService,
        private baseService: BaseService) {
        this.isModelLoaded = false;
        this.filterModel.sort = 'createdOn';
        this.filterModel.order = 'desc';
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.paginator.pageIndex = 0;
            this.getList();
        });
        this.paginator.page.subscribe(() => {
            this.getList();
        })
        this.getList();
    }

    getList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.updateFilterModel();
        if(this.filterModel.filterKey){
            this.paginator.firstPage();
        }
        this.service.pagedList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.totalCount = response.totalCount;
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

    deleteLeave(employeeId: number, ruleId: number): void {
        const dialRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete selected leave?',
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.isModelLoaded = false;
                    this.service.deleteLeave(employeeId, ruleId).subscribe({
                        next: () => {
                            this.baseService.successNotification('Leave rule has been removed successfully from employee.');
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.getList();
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
        this.paginator.firstPage();
        this.getList();
    }

    resetFilters(): void {
        this.filterModel = new FilterModel();
        this.paginator.firstPage();
        this.getList();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;

    }
}