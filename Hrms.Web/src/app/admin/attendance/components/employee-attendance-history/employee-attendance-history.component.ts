import { AfterViewInit, Component, Input } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AppUtils } from "src/app/utilities";
import { AttendanceEventFilterModel, SelectListItemModel } from "src/app/shared/models";
import { BaseService } from "src/app/shared/services";
import { EmployeeService } from "src/app/admin/services";

@Component({
    selector: 'app-employee-attendance-history',
    templateUrl: './employee-attendance-history.component.html',
})

export class EmployeeAttendanceHistoryComponent implements AfterViewInit {
    @BlockUI('blockui-employee-attendance-history') blockUI: NgBlockUI;
    // @Input() employeeId = 1;
    // @Input() selectedYear = AppUtils.getCurrentDate().getFullYear();
    // @Input() selectedMonth = AppUtils.getCurrentDate().getMonth() + 1;
    @Input() employeeId: any;
    @Input() selectedYear: any;
    @Input() selectedMonth: any;


    filterModel = new AttendanceEventFilterModel();
    isModelLoaded: boolean;
    employees = new Array<SelectListItemModel>();
    years = AppUtils.getYears();
    months = AppUtils.getMonthsForDropDown();
    firstIndex: any;

    constructor(private baseService: BaseService,
        private employeeService: EmployeeService,
    ) {
        this.isModelLoaded = false;
    }

    ngAfterViewInit(): void {
        this.getEmployeeList();
        
    }

    getEmployeeList(): void {
        this.blockUI.start();
        this.employeeService.getSelectListItem().subscribe({
            next: (response) => {
                this.employees = response;
                this.filterModel.employeeId = this.employees[0].key;
                this.setFilterOptions();
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }

    resetFilter(): void {
        this.filterModel = new AttendanceEventFilterModel();
        this.filterModel.employeeId = this.employees[0].key;
        this.setFilterOptions();
    }

    setFilterOptions() {
        this.employeeId = this.filterModel.employeeId;
        this.selectedMonth = this.filterModel.month;
        this.selectedYear = this.filterModel.year;
    }

}