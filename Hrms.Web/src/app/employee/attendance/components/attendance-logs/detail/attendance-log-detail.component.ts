import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AttendanceLogService } from 'src/app/employee/services/attendance-log.service';
import { BaseService } from 'src/app/shared/services';
import { AttendanceFilterModel, AttendanceLogModel, AttendanceRuleModel } from 'src/app/employee/attendance/model/index';
import { AppUtils, Constants } from 'src/app/utilities';


@Component({
  selector: 'app-attendance-detail',
  templateUrl: './attendance-log-detail.component.html',
})

export class AttendanceLogDetailComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @BlockUI('attendance-blockui') blockUI: NgBlockUI;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  filterModel: AttendanceFilterModel = new AttendanceFilterModel();
  model = new Array<AttendanceLogModel>();
  model1: AttendanceRuleModel;
  workDuration: number;
  date: string;
  id: number;
  isModelLoaded: boolean;

  columns = ['id', 'name', 'date', 'firstClockIn', 'lastClockOut', 'workDuration'];

  constructor(
    private baseService: BaseService,
    private service: AttendanceLogService,
    private appUtils: AppUtils
  ) { this.isModelLoaded = false; }

  get constants(): typeof Constants {
    return Constants;
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.getAttendanceLogList();
    });
    this.getAttendanceLogList();
  }

  getAttendanceLogList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.updateFilterModel();
    this.service.pageList(this.filterModel).subscribe({
      next: (response) => {
        this.model = response.items;
        this.model.forEach(element => {
          element.workDuration = AppUtils.getFormatedTimeDifference(element.workDuration);
          element.firstClockIn = this.appUtils.getUtcToLocalTime(element.firstClockIn);
          element.firstClockIn = AppUtils.getTime(element.firstClockIn);
          if (element.lastClockOut) {
            element.lastClockOut = this.appUtils.getUtcToLocalTime(element.lastClockOut);
          }
          element.lastClockOut = AppUtils.getTime(element.lastClockOut);
          this.date = AppUtils.getLocalFormattedDate(element.date);
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

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
  }

  resetFilters(): void {
    this.filterModel = new AttendanceFilterModel();
    this.getAttendanceLogList();
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getAttendanceLogList();
  }
}