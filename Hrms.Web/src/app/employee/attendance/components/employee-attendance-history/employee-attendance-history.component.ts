import { AfterViewInit, Component, Input, ViewChild } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AppUtils, Constants } from "src/app/utilities";
import { AttendanceEventFilterModel } from "src/app/shared/models";

@Component({
  selector: 'app-employee-attendance-history',
  templateUrl: './employee-attendance-history.component.html',
})
export class EmployeeAttendanceHistoryComponent implements AfterViewInit {
  @BlockUI('blockui-employee-attendance-history') blockUI: NgBlockUI;
  @Input() selectedYear = AppUtils.getCurrentDate().getFullYear();
  @Input() selectedMonth = AppUtils.getCurrentDate().getMonth() + 1;

  filterModel = new AttendanceEventFilterModel();
  years = AppUtils.getYears();
  months = AppUtils.getMonthsForDropDown();

  get constants(): typeof Constants { return Constants; }
  constructor() {
  }

  ngAfterViewInit(): void { }

  resetFilter(): void {
    this.filterModel = new AttendanceEventFilterModel();
    this.setFilterOptions();
  }

  setFilterOptions() {
    this.selectedMonth = this.filterModel.month;
    this.selectedYear = this.filterModel.year;
  }


}