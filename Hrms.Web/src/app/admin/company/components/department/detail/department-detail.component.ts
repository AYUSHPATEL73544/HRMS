import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatPaginator } from '@angular/material/paginator';
import { BaseService } from 'src/app/shared/services';
import { DepartmentService, EmployeeService } from 'src/app/admin/services';
import { FilterModel } from 'src/app/shared/models';
import { DepartmentModel } from 'src/app/admin/company/models';
import { DeleteComponent } from 'src/app/shared/dialog/delete-dialog/delete.component';
import { UpsertDepartmentComponent } from '../upsert/upsert-department.component';
import { Constants } from 'src/app/utilities/app.constants';
import { EmployeeModel } from 'src/app/admin/directory/models';

@Component({
  selector: 'app-department-detail',
  templateUrl: './department-detail.component.html',
})

export class DepartmentDetailComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @BlockUI('company-blockui') blockUI: NgBlockUI;

  model = new Array<DepartmentModel>();
  filterModel = new FilterModel();
  totalCount: number;
  isModelLoaded: boolean;
  employeesModel = new Array<EmployeeModel>();
  employeeNames: string;
  employeeList: string[];

  get constants(): typeof Constants {
    return Constants;
  }

  displayedColumns = ['name', 'description', 'peoples', 'action'];

  constructor(private dialog: MatDialog,
    private baseService: BaseService,
    private departmentService: DepartmentService,
    private employeeService: EmployeeService
  ) {
    this.isModelLoaded = false;
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      this.getDepartmentList();
    });
    this.paginator.page.subscribe(() => {
      this.getDepartmentList();
    });
    this.getDepartmentList();
  }


  // getTooltipContent(names: string[]): string {
  //   let list: string = '';
  //   names.forEach(name => {
  //     list += '• ' + name + '\n';
  //   });
  //   return list;
  // }

  addDepartment(): void {
    const dialRef = this.dialog.open(UpsertDepartmentComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true
    });

    dialRef.afterClosed().subscribe(() => {
      this.getDepartmentList();
    })
  }

  editDepartment(id: number): void {
    const dialRef = this.dialog.open(UpsertDepartmentComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id, }
    });

    dialRef.afterClosed().subscribe(() => {
      this.getDepartmentList();
    })
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.paginator.firstPage();
    this.getDepartmentList();
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage();
    this.getDepartmentList();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }

  getDepartmentList(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.updateFilterModel();
    if (this.filterModel.filterKey) {
      this.paginator.firstPage();
    }
    this.departmentService.pageList(this.filterModel).subscribe({
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

  deleteDepartment(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected department.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true
    });
    dialRef.afterClosed().subscribe(
      (confirm) => {
        if (confirm) {
          this.blockUI.start();
          this.isModelLoaded = false;
          this.departmentService.deleteDepartment(id).subscribe({
            next: () => {
              this.baseService.successNotification('Department has been deleted successfully.');
              this.blockUI.stop();
              this.isModelLoaded = true;
              this.getDepartmentList();
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

  getEmployees(departmentId: number) {
    this.employeeService.getEmployeesByDepartmentId(departmentId).subscribe({
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