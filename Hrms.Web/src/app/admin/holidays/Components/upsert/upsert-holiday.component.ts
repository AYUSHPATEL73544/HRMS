import { Component, Inject, } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { BaseService } from "src/app/shared/services";
import { HolidayService } from "src/app/admin/services/holiday.service";
import { HolidayGroupModel, HolidayModel } from "src/app/admin/holidays/models";
import { DashboardComponent } from "src/app/admin/components";
import { AppUtils, } from "src/app/utilities";

@Component({
  selector: 'app-upsert-holiday',
  templateUrl: './upsert-holiday.component.html',
})
export class UpsertHolidayComponent {
  @BlockUI('holiday-blockui') blockUI: NgBlockUI;

  years = AppUtils.getYears();
  model = new HolidayGroupModel();
  isYearSelected = false;
  selectedYear: number;
  selectedDate: Date;
  isNextYearSelected: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<DashboardComponent>,
    private service: HolidayService,
    private baseService: BaseService,
  ) {

    this.addHolidays();
    this.get(this.model.year);
  }

  get(year: number): void {
    this.blockUI.start();
    this.model.forwardToNextYear = false;
    this.isYearSelected = true;
    this.selectedYear = year;
    this.isNextYearSelected = false;
    const firstYearInList = this.years[0].key;
    if (firstYearInList == year) {
      this.isNextYearSelected = true;
    }
    this.service.getByYear(year).subscribe({
      next: (response) => {
        this.model.holidays = response;
        if (this.model.holidays.length == 0) {
          this.model.holidays.push(new HolidayModel());
        }
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  addHolidays(): void {
    this.model.holidays.push(new HolidayModel());
  }

  removeHoliday(i: number): void {
    this.model.holidays.splice(i, 1);
  }

  addFromPrevious(year: number, isChecked: boolean): void {
    this.blockUI.start();
    this.service.getPreviousYear(year, isChecked).subscribe({
      next: (res) => {
        this.model.holidays = res;
        if (!isChecked && this.model.holidays.length === 0) {
          this.addHolidays();
        }
        this.blockUI.stop();
      },
      error: (error) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  submit(): void {
    this.blockUI.start();
    this.model.holidays.forEach(element => {
      element.date = AppUtils.getFormattedDate(element.date, null);
      element.date = AppUtils.getDate(element.date);
    });


    this.service.add(this.model).subscribe({
      next: () => {
        if (this.model.holidays.length != 0) {
          this.baseService.successNotification('Holiday has been added successfully');
        }

        this.dialogRef.close();
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getMinDate(): Date {
    return new Date(this.selectedYear, 0, 1);
  }

  getMaxDate(): Date {
    return new Date(this.selectedYear, 11, 31);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}