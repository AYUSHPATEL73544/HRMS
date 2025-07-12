import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { EmployeeAttendanceRuleService } from "src/app/admin/services/employee-attendance.rule.service";
import { EmployeeAttendanceModel } from "src/app/admin/attendance/model";
import { FilterModel } from "src/app/shared/models";
import { Constants } from "src/app/utilities";
import { AssignRuleComponent } from "../assign-rule.component";



@Component({
    selector: 'app-assign-rule-detail',
    templateUrl: './assign-rule-detail.component.html',
})
export class AssignRuleDetailComponent implements AfterViewInit {

    @BlockUI('blockui-assign-rule-detail') blockUI: NgBlockUI;
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;


    columns = ['employeeCode', 'employeeName', 'department', 'ruleAssigned'];
    filterModel = new FilterModel();
    isModelLoaded: boolean;
    totalCount: number;
    showInactive: boolean = false;

    model = new Array<EmployeeAttendanceModel>();
    get constants(): typeof Constants { return Constants }
    constructor(private dialog: MatDialog,
        private service: EmployeeAttendanceRuleService,
        private baseService: BaseService) {
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
                this.getDetail();
            }
        });
        this.paginator.page.subscribe(() => {
            if (this.showInactive) {
                this.getInactiveEmployees();
            } else {
                this.getDetail();
            }
        });
        if (this.showInactive) {
            this.getInactiveEmployees();
        } else {
            this.getDetail();
        }
    }


    assignRule(): void {
        const dialRef = this.dialog.open((AssignRuleComponent), {
            width: Constants.dialogSize.medium,
            disableClose: true,
        });
        dialRef.afterClosed().subscribe(() => {
            if (!this.showInactive) {
                this.getDetail();
            }
            else {
                this.getInactiveEmployees();
            }
        });
    }

    getDetail(): void {
        this.blockUI.start();
        this.updateFilterModel();
        this.isModelLoaded = false;
        this.service.pageList(this.filterModel).subscribe({
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
        this.updateFilterModel();
        this.isModelLoaded = false;
        this.service.inActivepageList(this.filterModel).subscribe({
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
            this.getDetail();
        }
        else {
            this.getInactiveEmployees();
        }
    }
    searchFilterKey(): void {
        if (!this.showInactive) {
            this.getDetail();
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
            this.getDetail();
            this.paginator.firstPage();
        }
        else {
            this.getInactiveEmployees();
            this.paginator.firstPage();
        }
    }

    resetFilter(): void {
        this.filterModel = new FilterModel();
        this.showInactive = false;
        this.getDetail();
        this.paginator.firstPage();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
        this.filterModel.pageIndex = this.paginator.pageIndex;
        this.filterModel.pageSize = this.paginator.pageSize;
    }

}