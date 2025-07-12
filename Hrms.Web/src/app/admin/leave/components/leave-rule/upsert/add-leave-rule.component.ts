import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LeaveRuleService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { LeaveRuleModel } from 'src/app/admin/leave/models/index';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
  selector: 'app-add-leave-rule',
  templateUrl: './add-leave-rule.component.html',
})

export class AddLeaveRuleComponent implements OnInit {
  @BlockUI('leave-blockui') blockUI: NgBlockUI;

  model = new LeaveRuleModel();
  isModelLoaded: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<AddLeaveRuleComponent>,
    private baseService: BaseService,
    private service: LeaveRuleService,
    private appUtils: AppUtils) {
      this.isModelLoaded = false;
    if (data) {
      this.model.id = data.id
    }
    this.model.companyId = this.appUtils.getCompanyId();
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.getRule(this.model.id);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }


  getRule(id: number): void {
    this.isModelLoaded = false;
    this.blockUI.start();
    this.service.getRule(this.model.id).subscribe({
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
    if(this.model.description.length > 50){
      this.baseService.errorNotification("Description length cannot be greater than 50 characters.");
      return;
    }
    if (this.model.id) {
      this.isModelLoaded = false;
      this.blockUI.start();
      this.service.updateRule(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Rule has been updated successfully');
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.dialogRef.close();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.baseService.processErrorResponse(error);
        }
      });
    }
    else {
      this.isModelLoaded = false
      this.blockUI.start();
      this.service.addLeaveRule(this.model).subscribe({
        next: () => {
          this.baseService.successNotification('Rule has been added successfully');
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.dialogRef.close();
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