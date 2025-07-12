import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { BaseService } from 'src/app/shared/services';
import { AttendanceLogService } from 'src/app/employee/services/attendance-log.service';
import { AttendanceFilterModel, AttendanceLogModel, } from 'src/app/employee/attendance/model';
import { AdminNotesViewComponent } from '../notes-view/admin-notes-view.component';
import { AppUtils, Constants } from 'src/app/utilities';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-attendance-history',
  templateUrl: './attendance-history.component.html',
})
export class AttendanceHistoryComponent implements AfterViewInit {
  @BlockUI('attendance-history-manage') blockUI: NgBlockUI;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  months = AppUtils.getMonthsForDropDown();
  years = AppUtils.getYears();
  filterModel = new AttendanceFilterModel();
  model = new Array<AttendanceLogModel>();
  isModelLoaded: boolean;
  totalCount: number;
  attendanceId : number;
  id: number;
  displayedColumns = [
    'employeeCode',
    'name',
    'date',
    'inTime',
    'outTime',
    'workDuration',
    'detail'
  ];

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(
    private baseService: BaseService,
    private route: ActivatedRoute,
    private service: AttendanceLogService,
    private dialog: MatDialog,
    private appUtils: AppUtils
  ) {
    this.route.params.subscribe((params) => {
      this.attendanceId = params['id'];
  });
    this.isModelLoaded = false;
  }
  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.getAttendanceLogList();
    });
    this.paginator.page.subscribe(() => {
      this.getAttendanceLogList();
    });
    this.getAttendanceLogList();
  }

  getAttendanceLogList(): void {
    this.updateFilterModel();
    this.blockUI.start();
    this.isModelLoaded = false;
    if (this.filterModel.startDate) {
      this.filterModel.startDate = AppUtils.getFormattedDate(this.filterModel.startDate, null);
      this.filterModel.startDate = AppUtils.getDate(this.filterModel.startDate);
    }
    if (this.filterModel.endDate) {
      this.filterModel.endDate = AppUtils.getFormattedDate(this.filterModel.endDate, null);
      this.filterModel.endDate = AppUtils.getDate(this.filterModel.endDate);
    }
    this.service.getAttendanceHistory(this.filterModel, this.attendanceId).subscribe({
      next: (response) => {
        this.model = response.items;
        this.totalCount = response.totalCount;
        this.model.forEach((element) => {
          element.workDuration = AppUtils.getDifferenceInHours(element.inTime, element.outTime);
          element.inTime = this.appUtils.getUtcToLocalTime(element.inTime);
          element.inTime = AppUtils.getTime(element.inTime);
          if (element.outTime) {
            element.outTime = this.appUtils.getUtcToLocalTime(element.outTime);
          }
          element.outTime = AppUtils.getTime(element.outTime);
          element.date = AppUtils.getLocalFormattedDate(element.date);
        });
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      },
    });
  }

  viewNotes(id: number): void {
    const dialogRef = this.dialog.open(AdminNotesViewComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id: id }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAttendanceLogList();
    });
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
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
