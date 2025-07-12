import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AttendanceLogService, EmployeeService } from "src/app/admin/services";
import { BaseService } from "src/app/shared/services";
import { AttendanceLogChangeStatusModel, AttendanceLogModel, AttendanceModel } from "src/app/admin/attendance/model/index";
import { FilterModel, SelectListItemModel } from "src/app/shared/models";
import { AttendanceFilterModel } from "src/app/admin/attendance/model/attendance-filter.model";
import { AppUtils, Constants } from "src/app/utilities";
import { UpsertAttendanceLogComponent } from "../upsert/upsert-attendance-log.component";
import { DeleteComponent, DialogConfirmComponent, RejectComponent } from "src/app/shared/dialog";

@Component({
  selector: 'app-attendance-log-detail',
  templateUrl: './attendance-log-detail.component.html',
})

export class AttendanceLogDetailComponent implements AfterViewInit {
  @BlockUI('attendance-blockui') blockUI: NgBlockUI;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  filterModel = new AttendanceFilterModel();
  model = new Array<AttendanceLogModel>();
  employees = new Array<SelectListItemModel>();
  changeStatusModel = new AttendanceLogChangeStatusModel();
  totalCount: number;
  isModelLoaded: boolean;

  get constants(): typeof Constants { return Constants }

  columns = ['employeeCode', 'employeeName', 'date', 'inTime', 'outTime', 'workDuration'];

  constructor(
    private dialog: MatDialog,
    private baseService: BaseService,
    private service: AttendanceLogService,
    private employeeService: EmployeeService,
    private appUtils: AppUtils
  ) {
    this.isModelLoaded = false;
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
    this.getEmployeeList();

  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.getAttendanceLogList();
    this.paginator.firstPage();
  }

  resetFilter(): void {
    this.filterModel = new AttendanceFilterModel();
    this.getAttendanceLogList();
    this.paginator.firstPage();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
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
    if (this.filterModel.inTime) {
      this.filterModel.inTime = this.appUtils.getLocalToUtcTime(this.filterModel.inTime);
    }
    if (this.filterModel.filterKey) {
      this.paginator.firstPage();
    }
    this.service.getAttendanceHistoryList(this.filterModel).subscribe({
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
          element.isLateClockIn = element.inTime > element.graceInTime ? true : false;
        });
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

  getEmployeeList() {
    this.employeeService.getSelectListItem().subscribe({
      next: (response) => {
        this.employees = response;
      },
      error: (error) => {
        this.baseService.processErrorResponse(error);
      }
    })
  }


  addLog(): void {
    const dialRef = this.dialog.open(UpsertAttendanceLogComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dialRef.afterClosed().subscribe(() => {
      this.getAttendanceLogList();
    });
  }

  approve(id: number, status: number): void {
    const dialogRef = this.dialog.open(DialogConfirmComponent, {
      data: {
        title: `Approve`,
        message: `Are you sure you want to approve the selected attendance log?`
      }
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.changeStatusModel.id = id;
        this.changeStatusModel.status = status;
        this.changeStatus(this.changeStatusModel);
      }
    });
  }

  reject(id: number, status: number): void {
    const dialogRef = this.dialog.open(RejectComponent, {
      data: {
        title: `Reject`,
        message: `Are you sure you want to reject the selected attendance log?`,
        status: `4`
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.confirm) {
        if (result.log != null) {
          this.changeStatusModel.rejectionReason = result.log;
        }
        this.changeStatusModel.id = id;
        this.changeStatusModel.status = status;
        this.changeStatus(this.changeStatusModel);
      }
    });
  }

  changeStatus(model: AttendanceLogChangeStatusModel): void {
    this.service.changeStatus(model).subscribe({
      next: () => {
        this.baseService.successNotification('Leave log status has been changed successfully.');
        this.getAttendanceLogList();
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  deleteLog(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected Attendance log.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.isModelLoaded = false;
          this.service.deleteAttendance(id).subscribe({
            next: () => {
              this.baseService.successNotification('Attendance log has been deleted successfully.');
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.getAttendanceLogList();
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

}