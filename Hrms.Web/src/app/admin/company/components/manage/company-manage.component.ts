import { Component, ViewChild } from '@angular/core';

@Component({
  selector: 'app-company-manage',
  templateUrl: './company-manage.component.html',
})

export class CompanyManageComponent {
  @ViewChild('overview') overViewComponent: any;
  @ViewChild('address') addressComponent: any;
  @ViewChild('department') departmentComponent: any;
  @ViewChild('designation') designationComponent: any;


  onTabChanged(event: any): void {
    
    switch (event.index) {
      case 0:
        this.overViewComponent.getDetail();
        break;
      case 1:
        this.addressComponent.getDetail();
        break;
      case 2:
        this.departmentComponent.getDepartmentList();
        break;
      case 3:
        this.designationComponent.getDesignationList();
        break;
    }
  }
}