import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { SelectListItemModel } from 'src/app/shared/models';
import { DepartmentService, DesignationService, EmployeeService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { WorkHistoryService } from 'src/app/admin/services/work-history.service';
import { EmployeeModel, WorkHistoryModel } from 'src/app/admin/directory/models/index';
import { UpdateWorkDetailComponent } from '../update/update-work-detail.component';
import { EmployeeStatusComponent } from '../Status-dailog/employee-status.component';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-work-detail',
  templateUrl: './work-detail.component.html',
})


export class WorkDetailComponent implements OnInit {
  @BlockUI('work-detail-blockui') blockUI: NgBlockUI;

  displayedColumns = ['department', 'designation', 'from', 'to', 'action'];

  model = new EmployeeModel();

  departments = new Array<SelectListItemModel>();
  designations = new Array<SelectListItemModel>();
  workHistoryModel = new Array<WorkHistoryModel>();
  isBasicInfoEditable = false;
  isWorkInfoEditable = false;
  isWorkHistoryEditable = false;
  isExitEditable = false;
  isModelLoaded: boolean;
  workExperience: string;
  currentDate = Date();
  startedFrom: string;
  dateOfJoining: string;
  lastWorkingDay: string;
  dateOfLeaving: string;
  status: string;
  employeeType: string;
  role: string;
  statusDropDown = AppUtils.statusDropDown();
  typeDropDown = AppUtils.employeeType();
  probationDropDown = AppUtils.probationPeriodDropDown();

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(private dialog: MatDialog,
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private workHistoryService: WorkHistoryService,
    private baseService: BaseService
  ) {
    this.isModelLoaded = false;
    this.route.params.subscribe((params) => {
      this.model.id = params['id'];
    });
  }

  ngOnInit(): void {
    this.getDesignationList();
    this.getDepartmentList();
    this.getWork(this.model.id);
  }

  getWork(id: number): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.employeeService.getEmployee(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
        this.dateOfJoining = AppUtils.getLocalFormattedDate(this.model.dateOfJoining);
        this.currentDate = AppUtils.getLocalFormattedDate(this.currentDate);
        this.lastWorkingDay = AppUtils.getLocalFormattedDate(this.model.dateOfLeaving);
        if (this.model.status == 2) {
          this.workExperience = AppUtils.getDifferenceInYear(this.dateOfJoining, this.currentDate);
        }
        if (this.model.status == 3 && this.model.dateOfLeaving >= Date()) {
          this.workExperience = AppUtils.getDifferenceInYear(this.dateOfJoining, this.currentDate);
        }
        if (this.model.status == 3 && this.model.dateOfLeaving <= Date()) {
          this.workExperience = AppUtils.getDifferenceInYear(this.dateOfJoining, this.lastWorkingDay);
        }
        this.status = this.statusDropDown.find(x => x.key == this.model.status).value;
        if (this.model.employeeType > 0) {
          this.employeeType = this.typeDropDown.find(x => x.key == this.model.employeeType).value;
        }
        this.dateOfLeaving = AppUtils.getLocalFormattedDate(this.model.dateOfLeaving);
        
        if (this.model.exitDate != null && this.model.dateOfLeaving != null) {
          const exitDate = AppUtils.getDateFromString(this.model.exitDate);
          const dateOfLeaving = AppUtils.getDateFromString(this.model.dateOfLeaving);
          this.model.noticePeriod = AppUtils.getDifferenceInDays(exitDate, dateOfLeaving);
        }
        this.getWorkHistory(id);
        this.isModelLoaded = true;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  changeStatus(): void {
    if (this.model.status == 3) {
      const dialogRef = this.dialog.open(EmployeeStatusComponent, {
        data: {
          title: 'Exit Type',
          model: this.model
        },
        width: Constants.dialogSize.medium,
        disableClose: true
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.submit();
        }
        else {
          this.getWork(this.model.id);
        }
      });

    }

  }

  getDepartmentList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.departmentService.getSelectListItem().subscribe({
      next: (response) => {
        this.blockUI.stop();
        this.departments = response;
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getDesignationList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.designationService.getSelectListItem().subscribe({
      next: (response) => {
        this.designations = response;
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getWorkHistory(employeeId: number): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.workHistoryService.get(employeeId).subscribe({
      next: (response) => {
        this.workHistoryModel = response;
        this.workHistoryModel.forEach(element => {
          element.designationName = this.designations.find(x => x.key == element.designationId).value;
          element.departmentName = this.departments.find(x => x.key == element.departmentId).value;
          this.startedFrom = element.from;
        });
        this.isModelLoaded = true;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  submit() {
    this.model.dateOfJoining = AppUtils.getFormattedDate(this.model.dateOfJoining, null);
    this.model.dateOfJoining = AppUtils.getDate(this.model.dateOfJoining);
    if (this.model.exitDate != null) {
      this.model.exitDate = AppUtils.getFormattedDate(this.model.exitDate, null);
      this.model.exitDate = AppUtils.getDate(this.model.exitDate);
    }
    if (this.model.dateOfLeaving != null) {
      this.model.dateOfLeaving = AppUtils.getFormattedDate(this.model.dateOfLeaving, null);
      this.model.dateOfLeaving = AppUtils.getDate(this.model.dateOfLeaving);
    }
    if (this.model.startedFrom != null) {
      this.model.startedFrom = AppUtils.getFormattedDate(this.model.startedFrom, null);
      this.model.startedFrom = AppUtils.getDate(this.model.startedFrom);
    }
    this.employeeService.update(this.model).subscribe({
      next: () => {
        this.baseService.successNotification('Work has been updated successfully');
        this.isBasicInfoEditable = false;
        this.isWorkHistoryEditable = false;
        this.isWorkInfoEditable = false;
        this.isExitEditable = false;
        this.getWork(this.model.id);
        this.getDesignationList();
        this.getDepartmentList();
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  updateWorkHistory(id: number): void {
    const dialRef = this.dialog.open(UpdateWorkDetailComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        id: id,
        dateOfJoining: this.model.dateOfJoining

      }

    });
    dialRef.afterClosed().subscribe(() => {
      this.getDesignationList();
      this.getDepartmentList();
      this.getWork(this.model.id);
      this.getWorkHistory(this.model.id);
    });
  }

  cancel(): void {
    this.isBasicInfoEditable = false;
    this.isWorkInfoEditable = false;
    this.isWorkHistoryEditable = false;
    this.isExitEditable = false;
    this.getWork(this.model.id);
  }

  reloadDetails(): void {
    this.getWork(this.model.id);
  }
}