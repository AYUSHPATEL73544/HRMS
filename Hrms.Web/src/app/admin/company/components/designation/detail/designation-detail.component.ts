import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatPaginator } from '@angular/material/paginator';
import { BaseService } from 'src/app/shared/services';
import { DesignationService, EmployeeService } from 'src/app/admin/services';
import { DesignationModel } from 'src/app/admin/company/models';
import { FilterModel } from 'src/app/shared/models';
import { UpsertDesignationComponent } from '../upsert/upsert-designation.component';
import { DeleteComponent } from 'src/app/shared/dialog/delete-dialog/delete.component';
import { Constants } from 'src/app/utilities/app.constants';
import { EmployeeModel } from 'src/app/admin/directory/models';

@Component({
  selector: 'app-designation-detail',
  templateUrl: './designation-detail.component.html',
})

export class DesignationDetailComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @BlockUI('company-blockui') blockUI: NgBlockUI;

  model = new Array<DesignationModel>();
  filterModel = new FilterModel();
  totalCount: number;
  isModelLoaded: boolean;
  employeesModel = new Array<EmployeeModel>();
  employeeNames: string;
  employeeList: string[];


  get constants(): typeof Constants { return Constants; }
  displayedColumns = ['name', 'description', 'peoples', 'action'];

  constructor(private dialog: MatDialog,
    private baseService: BaseService,
    private designationService: DesignationService,
    private employeeService: EmployeeService
  ) {
    this.isModelLoaded = false;
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.getDesignationList();
    });
    this.paginator.page.subscribe(() => {
      this.getDesignationList();
    })
    this.getDesignationList();
  }

  addDesignation(): void {
    const dialRef = this.dialog.open(UpsertDesignationComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(() => {
      this.getDesignationList();
    });
  }

  editDesignation(id: number): void {
    const dialRef = this.dialog.open(UpsertDesignationComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id, }
    });
    dialRef.afterClosed().subscribe(() => {
      this.getDesignationList();
    });
  }

  getDesignationList(): void {
    this.updateFilterModel();
    this.blockUI.start();
    this.isModelLoaded = false;
    if (this.filterModel.filterKey) {
      this.paginator.firstPage();
    }
    this.designationService.pageList(this.filterModel).subscribe({
      next: (response) => {
        this.model = response.items;
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

  deleteDesignation(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected Designation.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.isModelLoaded = false;

          this.designationService.deleteDesignation(id).subscribe({
            next: () => {
              this.baseService.successNotification('Designation has been deleted successfully.');
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.getDesignationList();
            },
            error: (error: any) => {
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.baseService.processErrorResponse(error);
            }
          });
        }
      }
    );
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.paginator.firstPage();
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage();
    this.getDesignationList();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }



  getEmployees(designationId: number) {
    this.employeeService.getEmployeesByDesignationId(designationId).subscribe({
      next: (res) => {
        this.employeesModel = res;

        //get comma saparated strings
        this.employeeNames = this.employeesModel.map(function (employee) {
          return employee.employeeName;
        }).join(',');
        //end

        // split  name string in array of strings
        this.employeeList = this.employeeNames.split(',');
      },
      error: (error) => {
        this.baseService.processErrorResponse(error);
      }
    })
  }


}
