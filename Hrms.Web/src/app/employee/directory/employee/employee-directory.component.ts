import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { EmployeeService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { EmployeeModel } from 'src/app/employee/profile/models';
import { FilterModel } from 'src/app/shared/models';
import { Constants } from 'src/app/utilities';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'employee-directory',
  templateUrl: './employee-directory.component.html',
})

export class EmployeeDirectoryComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @BlockUI('employee-blockui') blockUI: NgBlockUI;

  model = new Array<EmployeeModel>();
  filterModel = new FilterModel();
  designation: string;
  department: string;
  totalCount: number;
  isModelLoaded: boolean;
  employeeStatus: number;
  get constants(): typeof Constants { return Constants; }

  displayedColumns = ['code', 'firstName', 'department', 'designation', 'email', 'manager'];

  constructor(
    private employeeService: EmployeeService,
    private baseService: BaseService
  ) {
    this.isModelLoaded = false;
    this.employeeStatus = this.constants.recordStatus.active;
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.getEmployees();
    });
    this.paginator.page.subscribe(() => {
      this.getEmployees();
    });
    this.getEmployees();

  }


  getEmployees(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.updateFilterModel();
    if(this.filterModel.filterKey){
      this.paginator.firstPage();
    }
    this.employeeService.pageList(this.filterModel, this.employeeStatus).subscribe({
      next: (response) => {
        this.model = response.items;
         this.model.forEach(x => x.imageDetails != null ?
          x.imageUrl = this.getImageUrl(x.imageDetails.key):x.imageUrl=null);
        this.totalCount = response.totalCount;
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

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.paginator.firstPage();
    this.getEmployees();
  }

  resetFilter(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage(); 
    this.getEmployees();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }

  getImageUrl(key:string):string{
    return environment.apiBaseUrl + '/documents/' + key;
  }
}