import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DepartmentService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { DepartmentModel } from 'src/app/admin/company/models';

@Component({
  selector: 'app-upsert-department',
  templateUrl: './upsert-department.component.html',
})

export class UpsertDepartmentComponent implements OnInit {
  @BlockUI('company-blockui') blockUI: NgBlockUI;

  model = new DepartmentModel();
  isModelLoaded: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertDepartmentComponent>,
    private baseService: BaseService,
    private departmentService: DepartmentService
  ) {
    this.isModelLoaded = false;

    if (data) {
      this.model.id = data.id;
    }
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.getDepartment(this.model.id);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }

  getDepartment(id: number): void {
    this.isModelLoaded = false;
    this.blockUI.start();
    this.departmentService.getDepartment(id).subscribe({
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

  submit(): void {
    if (this.model.id) {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.departmentService.editDepartment(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Department has been updated successfully');
          this.dialogRef.close();
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
    else {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.departmentService.addDepartment(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Department has been added successfully');
          this.dialogRef.close();
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
  }
}