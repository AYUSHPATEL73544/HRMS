import { Component, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CompanyService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { CompanyModel } from 'src/app/admin/company/models';
import { Constants } from 'src/app/utilities/app.constants';
import { UpsertCompanyAddressComponent } from '../upsert/upsert-company-address.component';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-company-address',
  templateUrl: './company-address.component.html',
})

export class CompanyAddressComponent implements OnInit {
  @BlockUI('company-blockui') blockUI: NgBlockUI;
  model = new CompanyModel();
  isModelLoaded: boolean;
  get constants(): typeof Constants {
    return Constants
  }

  constructor(private dialog: MatDialog,
    private baseService: BaseService,
    private companyService: CompanyService,
    private appUtils: AppUtils) {
    this.isModelLoaded = false;
  }

  ngOnInit(): void {
    this.getDetail();
  }

  addRegisteredAddress(): void {
    const dialRef = this.dialog.open(UpsertCompanyAddressComponent, {
      width: this.constants.dialogSize.large,
      disableClose: true,
      data: { model: this.model, addressType: this.constants.addressType.registered }
    });
    dialRef.afterClosed().subscribe(() => {
      this.getDetail();
    });
  }

  addCorporateAddress(): void {
    const dialRef = this.dialog.open(UpsertCompanyAddressComponent, {
      width: this.constants.dialogSize.large,
      disableClose: true,
      data: { model: this.model, addressType: this.constants.addressType.corporate }
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