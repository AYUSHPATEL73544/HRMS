import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatPaginator } from "@angular/material/paginator";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { MatSort } from "@angular/material/sort";
import { BaseService } from "src/app/shared/services";
import { LeaveService } from "src/app/admin/services";
import { LeaveModel } from "src/app/admin/leave/models";
import { FilterModel } from "src/app/shared/models";
import { UpsertLeaveCountComponent } from "src/app/admin/leave/components/upsert/upsert-leave-count.component";
import { Constants } from "src/app/utilities";

@Component({
    selector: 'app-assign-leave-rule-detail',
    templateUrl: './assign-leave-rule-detail.component.html',
})
export class AssignLeaveRuleDetailComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @BlockUI('assign-leave-blockui') blockUI: NgBlockUI;

    columns = ['employeeCode', 'employeeName', 'department', 'leaveRule'];

    deletedRule = "Deleted rule";
    model = new Array<LeaveModel>();
    filterModel = new FilterModel();
    totalCount: number;
    isModelLoaded: boolean;
    showInactive: boolean = false;

    get constants(): typeof Constants { return Constants; }

    constructor(private dialog: MatDialog,
        private service: LeaveService,
        private baseService: BaseService,
    ) {
        this.isModelLoaded = false;
        this.filterModel.sort = 'createdOn';
        this.filterModel.order = 'desc';
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.paginator.pageIndex = 0;
            if (this.showInactive) {
                this.getInactiveEmployees();
            } else {
                this.assignRuleList();
            }
        });
        this.paginator.page.subscribe(() => {
            if (this.showInactive) {
                this.getInactiveEmployees();
            } else {
                this.assignRuleList();
            }
        });
        if (this.showInactive) {
            this.getInactiveEmployees();
        } else {
            this.assignRuleList();
        }
    }

    assignRuleList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.updateFilterModel();
        this.service.AssignRuleList(this.filterModel).subscribe({
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
        })
    }

    getInactiveEmployees(): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.updateFilterModel();
        this.service.inActivepagedList(this.filterModel).subscribe({
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
        })
    }



    onCheckboxChange(): void {
        if (!this.showInactive) {
            this.assignRuleList();
        }
        else {
            this.getInactiveEmployees();
        }
    }

    assignRule(): void {
        const dialRef = this.dialog.open((UpsertLeaveCountComponent), {
            width: Constants.dialogSize.medium,
            disableClose: true,
        });
        dialRef.afterClosed().subscribe(() => {
            if (!this.showInactive) {
                this.assignRuleList();
            }
            else {
                this.getInactiveEmployees();
            }
        })
    }

    searchFilterKey(): void {
        if (!this.showInactive) {
            this.assignRuleList();
            this.paginator.firstPage();
        }
        else {
            this.getInactiveEmployees();
            this.paginator.firstPage();
        }
    }

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        if (!this.showInactive) {
            this.assignRuleList();
            this.paginator.firstPage();
        }
        else {
            this.getInactiveEmployees();
            this.paginator.firstPage();
        }
    }

    resetFilters(): void {
        this.filterModel = new FilterModel();
        this.showInactive = false;
        this.assignRuleList();
        this.paginator.firstPage();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }
}
