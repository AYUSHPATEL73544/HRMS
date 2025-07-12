import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { BaseService } from 'src/app/shared/services';
import { AttendanceLogService } from 'src/app/admin/services';
import { AttendanceFilterModel, AttendanceLogModel } from 'src/app/admin/attendance/model';
import { DeleteComponent } from 'src/app/shared/dialog';
import { AppUtils, Constants } from 'src/app/utilities';
import { UpsertAttendanceLogComponent } from '../attendance-logs/upsert/upsert-attendance-log.component';
import { AttendanceViewNoteComponent } from './view-note/attendance-view-note.component';


@Component({
  selector: 'app-attendance-history',
  templateUrl: './attendance-history.component.html',
})


export class AttendanceHistoryComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @BlockUI('blockui-attendance-history') blockUI: NgBlockUI;


  model = new Array<AttendanceLogModel>();
  filterModel = new AttendanceFilterModel();
  totalCount: number;
  formattedInTime:string;
  isModelLoaded: boolean;
  displayedColumns = ['employeeCode', 'name', 'date', 'inTime', 'outTime', 'workDuration', 'action'];
  id = 0;
  get constants(): typeof Constants { return Constants }

  constructor(
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private baseService: BaseService,
    private service: AttendanceLogService,
    private appUtils: AppUtils
  ) {
    this.isModelLoaded = false;

    this.route.params.subscribe((params) => {
      this.id = params['id'];
    })
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.getAttendanceLogList();
    });
    this.paginator.page.subscribe(() => {
      this.getAttendanceLogList();
    })
    this.getAttendanceLogList();

  }

  getAttendanceLogList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.updateFilterModel();
    if (this.filterModel.startDate) {
      this.filterModel.startDate = AppUtils.getFormattedDate(this.filterModel.startDate, null);
      this.filterModel.startDate = AppUtils.getDate(this.filterModel.startDate);
    }
    if (this.filterModel.endDate) {
      this.filterModel.endDate = AppUtils.getFormattedDate(this.filterModel.endDate, null);
      this.filterModel.endDate = AppUtils.getDate(this.filterModel.endDate);
    }
    if(this.filterModel.inTime){
      this.filterModel.inTime = this.appUtils.getLocalToUtcTime(this.filterModel.inTime);
    }

    this.service.getAttendanceHistory(this.filterModel, this.id).subscribe({
      next: (response) => {
        this.model = response.items;
        this.totalCount = response.totalCount;
        this.model.forEach(element => {
          element.workDuration = (AppUtils.getDifferenceInHours(element.inTime, element.outTime));
          element.inTime = this.appUtils.getUtcToLocalTime(element.inTime);
          element.inTime = AppUtils.getTime(element.inTime);
          if (element.outTime) {
            element.outTime = this.appUtils.getUtcToLocalTime(element.outTime);
          }
          element.outTime = AppUtils.getTime(element.outTime);
          element.date = AppUtils.getLocalFormattedDate(element.date);
          element.graceInTime = AppUtils.getTime(element.graceInTime);
          element.isLateClockIn = element.inTime>element.graceInTime ? true: false;
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

  editLog(id: number): void {
    const dialRef = this.dialog.open(UpsertAttendanceLogComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id }
    });
    dialRef.afterClosed().subscribe(() => {
      this.getAttendanceLogList();
    });
  }

  deleteLog(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected AttendanceLog.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.isModelLoaded = false;

          this.service.deleteLog(id).subscribe({
            next: () => {
              this.baseService.successNotification('Attendance log has been deleted successfully.');
              this.getAttendanceLogList();
              this.blockUI.stop();
              this.isModelLoaded = true;
            },
            error: (error: any) => {
              this.blockUI.stop();
              this.baseService.processErrorResponse(error);
            }
          });
        }
      }
    );
  }
  details(element: AttendanceLogModel): void {
    const dailRef = this.dialog.open(AttendanceViewNoteComponent, {

      data: {
        title: 'Educational Details',
        model: element
      },
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dailRef.afterClosed().subscribe(() => {
      this.getAttendanceLogList();
    });

  }
  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getAttendanceLogList();
  }

  resetFilter(): void {
    this.filterModel = new AttendanceFilterModel();
    this.getAttendanceLogList();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }
}
