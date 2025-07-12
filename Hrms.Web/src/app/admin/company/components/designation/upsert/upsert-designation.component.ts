import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { DesignationService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { DesignationModel } from '../../../models';

@Component({
  selector: 'app-upsert-designation',
  templateUrl: './upsert-designation.component.html',
})

export class UpsertDesignationComponent implements OnInit {
  @BlockUI('company-blockui') blockUI: NgBlockUI;

  model = new DesignationModel();
  isModelLoaded : boolean;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertDesignationComponent>,
    private baseService: BaseService,
    private designationService: DesignationService
  ) {
    this.isModelLoaded = false;
    if (data) {
      this.model.id = data.id;
    }
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.getDesignation(this.model.id);
    }
  }

  getDesignation(id: number): void {
    this.isModelLoaded = false;
    this.blockUI.start();
    this.designationService.getDesignation(this.model.id).subscribe({
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
    this.dialogRef.close();
  }

  submit(): void {
    if (this.model.id) {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.designationService.editDesignation(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Designation has been updated successfully')
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
      this.isModelLoaded= false;
      this.blockUI.start();
      this.designationService.addDesignation(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Designation has been added successfully')
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