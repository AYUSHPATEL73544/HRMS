import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CompanyService } from 'src/app/admin/services/company.service';
import { BaseService } from 'src/app/shared/services';
import { CompanyModel } from 'src/app/admin/company/models';
import { UpsertCompanyOverviewComponent } from '../upsert/upsert-company-overview.component';
import { Constants } from 'src/app/utilities/app.constants';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-company-overview',
  templateUrl: './company-overview.component.html',
})

export class CompanyOverviewComponent implements OnInit {
  @BlockUI('company-blockui') blockUI: NgBlockUI;
  model = new CompanyModel();
  isModelLoaded: boolean;

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(
    private dialog: MatDialog,
    private baseService: BaseService,
    private companyService: CompanyService,
    private appUtils: AppUtils) {
    this.isModelLoaded = false;
  }

  ngOnInit(): void {
    this.getDetail();
  }

  addInfo(): void {
    const dialRef = this.dialog.open(UpsertCompanyOverviewComponent, {
      width: this.constants.dialogSize.large,
      disableClose: true,
    });
    dialRef.afterClosed().subscribe(() => {
      this.getDetail();
    });
  }

  editInfo(): void {
    const dialRef = this.dialog.open(UpsertCompanyOverviewComponent, {
      width: this.constants.dialogSize.large,
      disableClose: true,
      data: {
        model: this.model,
        id: this.model.id
      }
    });
    dialRef.afterClosed().subscribe(() => {
      this.getDetail();
    });
  }

  getDetail(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.companyService.get(this.appUtils.getCompanyId()).subscribe({
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
}