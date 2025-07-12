import { Component, EventEmitter, Inject, OnInit, Output, ViewChild } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { DepartmentService, DesignationService, EmployeeService } from "src/app/admin/services";
import { SelectListItemModel } from "src/app/shared/models";
import { BaseService } from "src/app/shared/services";
import { EmployeeModel } from "src/app/admin/directory/models";
import { AppUtils } from "src/app/utilities";
import { NgForm } from "@angular/forms";

@Component({
  selector: 'app-upsert-employee-directory',
  templateUrl: './upsert-employees-directory.component.html',
})

export class UpsertEmployeesDirectoryComponent implements OnInit {
  @BlockUI('employee-blockui') blockUI: NgBlockUI;
  @ViewChild('f') form: NgForm;
  @Output() closeDrawer: EventEmitter<void> = new EventEmitter<void>();

  model = new EmployeeModel();
  isModelLoaded: boolean;
  genderDropDown = AppUtils.getGenderForDropDown();
  departments = new Array<SelectListItemModel>();
  designations = new Array<SelectListItemModel>();
  department: string;
  designation: string;
  constructor(
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private employeeService: EmployeeService,
    private baseService: BaseService,
    public appUtils: AppUtils
  ) {
    this.isModelLoaded = false;

    this.model.companyId = this.appUtils.getCompanyId();
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.getEmployee(this.model.id);
    }
    this.getDepartmentList();
    this.getDesignationList();
  }

  getEmployee(id: number): void {
    this.isModelLoaded = false;
    this.blockUI.start();
    this.employeeService.getEmployee(id).subscribe({
      next: (response) => {
        this.model = response;
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

  cancel(): void {
    this.closeDrawer.emit();
    this.form.resetForm();
    this.model = new EmployeeModel();
    this.model.companyId = this.appUtils.getCompanyId();
  }

  getDepartmentList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.departmentService.getSelectListItem().subscribe({
      next: (response) => {
        this.departments = response;
        if (this.model.departmentId > 0) {
          this.department = this.departments.find(x => x.key == this.model.departmentId).value;
        }
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

  getDesignationList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;

    this.designationService.getSelectListItem().subscribe({
      next: (response) => {
        this.designations = response;
        if (this.model.designationId > 0) {
          this.designation = this.designations.find(x => x.key == this.model.designationId).value;
        }
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

  submit(): void {
    this.model.dateOfJoining = AppUtils.getFormattedDate(this.model.dateOfJoining, null);
    this.model.dateOfJoining = AppUtils.getDate(this.model.dateOfJoining);
    this.model.currentAddress = null;
    this.model.permanentAddress = null;
    this.model.departmentId = null;
    this.model.designationId = null;
    this.model.department = null;
    this.model.designation = null;
    if (this.model.id) {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.employeeService.update(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Employee has been updated successfully.');
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.cancel();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.baseService.processErrorResponse(error);
        }
      });
    }
    else {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.employeeService.add(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Employee has been added successfully.');
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.cancel();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.baseService.processErrorResponse(error);
        }
      });
    }
  }
}