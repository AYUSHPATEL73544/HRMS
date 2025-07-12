import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CompanyService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { CompanyModel } from 'src/app/admin/company/models';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-upsert-company-overview',
  templateUrl: './upsert-company-overview.component.html',
})

export class UpsertCompanyOverviewComponent {
  @BlockUI('company-blockui') blockUI: NgBlockUI;

  model = new CompanyModel();
  isModelLoaded: boolean;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private baseService: BaseService,
    private dialogRef: MatDialogRef<UpsertCompanyOverviewComponent>,
    private companyService: CompanyService,
    public appUtils: AppUtils
  ) {
    this.isModelLoaded = false;
    this.model = data.model;
    this.model.id = data.id;
  }

  cancel(): void {
    this.dialogRef.close();
  }

  submit(): void {
    this.isModelLoaded = false;
    this.blockUI.start();
    this.companyService.update(this.model).subscribe({
      next: () => {
        this.baseService.successNotification('Details has been updated successfully');
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