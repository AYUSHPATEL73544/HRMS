import { Component, OnInit } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import * as moment from 'moment';
import { EmployeeService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { EmployeeModel } from 'src/app/employee/profile/models/index';
import { AppUtils } from 'src/app/utilities';
import { en } from '@fullcalendar/core/internal-common';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-employee-profile-manage',
  templateUrl: './profile-manage.component.html',
})

export class ProfileManageComponent implements OnInit {
  @BlockUI('blockui-profile') blockUI: NgBlockUI;

  model = new EmployeeModel();
  selectedTabIndex = 0;
  statusDropDown = AppUtils.statusDropDown();
  typeDropDown = AppUtils.employeeType();
  constructor(private employeeService: EmployeeService,
    private baseService: BaseService) {

  }

  ngOnInit(): void {
    this.getEmployeeDetail();
  }

  getEmployeeDetail(): void {
    this.blockUI.start();
    this.employeeService.getByUserId().subscribe({
      next: (response) => {
        this.model = response;
        if (this.model.gender) {
          this.model.genderName = AppUtils.getGenderForDropDown().find(x => x.key == this.model.gender).value;
        }
        if (this.model.maritalStatus > 0) {
          this.model.martialStatus = AppUtils.getMaritalStatusForDropDown().find(x => x.key == this.model.maritalStatus).value;
        }
        if (this.model.bloodGroup > 0) {
          this.model.bloodGroupName = AppUtils.getBloodGroupForDropDown().find(x => x.key == this.model.bloodGroup).value;
        }

        this.model.workExperience = AppUtils.getDifferenceInYear(AppUtils.getLocalFormattedDate(this.model.dateOfJoining), moment());
        this.model.employeeStatus = this.statusDropDown.find(x => x.key == this.model.status).value;
        if (this.model.employeeType > 0) {
          this.model.type = this.typeDropDown.find(x => x.key == this.model.employeeType).value;
        }

        if(this.model.imageDetails != null){
          this.model.imageUrl = environment.apiBaseUrl + '/documents/' + this.model.imageDetails.key;
        }

        this.blockUI.stop();
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  onTabChanged(index: number): void {
    this.selectedTabIndex = index;
  }

}