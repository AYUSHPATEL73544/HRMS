import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AttendanceRuleModel } from 'src/app/employee/attendance/model/index';
import { FilterModel } from 'src/app/shared/models';
import { AttendanceService } from 'src/app/employee/services/attendance.service';
import { BaseService } from 'src/app/shared/services';
import { AppUtils } from 'src/app/utilities';


@Component({
    selector: 'app-attendance-rule-manage',
    templateUrl: './attendance-rule-manage.component.html',
})

export class AttendanceRuleManageComponent implements AfterViewInit {
    @ViewChild(MatSort, { static: false }) sort: MatSort;
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;

    model = new Array<AttendanceRuleModel>();
    filterModel = new FilterModel();
    weekDays = AppUtils.getWeekDaysForDropDown();
    isModelLoaded: boolean;

    columns = ['title', 'startDay', 'endDay', 'action'];

    constructor(
        private baseService: BaseService,
        private service: AttendanceService
    ) {
        this.isModelLoaded = false;
    }

    ngAfterViewInit(): void {
        this.sort.sortChange.subscribe(() => {
            this.getAttendanceRuleList();
        });
        this.getAttendanceRuleList();
    }


    getAttendanceRuleList(): void {
        this.updateFilterModel();
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.getPageList(this.filterModel).subscribe({
            next: (response) => {
                this.model = response.items;
                this.model.forEach(element => {
                    element.weekStartDay = this.weekDays.find(x => x.key == element.startDay).value;
                    element.weekLastDay = this.weekDays.find(x => x.key == element.endDay).value;
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

    resetFilterKey(): void {
        this.filterModel.filterKey = null;
        this.getAttendanceRuleList();
    }

    resetFilter(): void {
        this.filterModel = new FilterModel();
        this.getAttendanceRuleList();
    }

    updateFilterModel(): void {
        this.filterModel.sort = this.sort.active;
        this.filterModel.order = this.sort.direction;
    }


}
