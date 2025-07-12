import { Component, OnInit } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CompanyService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { CompanyModel } from 'src/app/employee/company/models';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-company-manage',
  templateUrl: './company-manage.component.html',
})

export class CompanyManagerComponent implements OnInit {
  @BlockUI('company-manage-blockui') blockUI: NgBlockUI;

  model = new CompanyModel();

  constructor(private service: CompanyService,
    private baseService: BaseService,
    private appUtils: AppUtils) {

  }

  ngOnInit(): void {
    this.getCompanyDteail();
  }

  getCompanyDteail(): void {
    this.blockUI.start();
    this.service.get(this.appUtils.getCompanyId()).subscribe({
      next: (response) => {
        this.model = response;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }
}