import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { EmployeeService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { EmployeeModel } from 'src/app/admin/directory/models';
import {
  FilterModel,
  MatTableResponseModel,
  SelectListItemModel,
} from 'src/app/shared/models';
import { DeleteComponent } from 'src/app/shared/dialog';
import { UpsertEmployeesDirectoryComponent } from '../upsert/upsert-employees-directory.component';
import { Constants } from 'src/app/utilities';
import { ResetPasswordComponent } from 'src/app/admin/directory/components/reset-password/reset-password.component';
import { MatDrawer } from '@angular/material/sidenav';
import { UpsertNoteComponent } from 'src/app/admin/directory/components/note/upsert/upsert-note.component';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-employee-directory',
  templateUrl: './employees-directory.component.html',
})
export class EmployeesDirectoryComponent implements AfterViewInit {
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild('drawer') drawer: MatDrawer;
  @ViewChild('upsertComponent', { static: false })
  upsertComponent: UpsertEmployeesDirectoryComponent;
  @BlockUI('directory-blockui') blockUI: NgBlockUI;

  departments = new Array<SelectListItemModel>();
  designations = new Array<SelectListItemModel>();

  model = new Array<EmployeeModel>();
  filterModel = new FilterModel();
  response: MatTableResponseModel;
  totalCount: number;
  isModelLoaded: boolean;
  showInactive: boolean = false;
  employeeStatus: number;

  get constants(): typeof Constants {
    return Constants;
  }

  displayedColumns = [
    'code',
    'firstName',
    'department',
    'designation',
    'email',
    'action',
  ];

  constructor(
    private dialog: MatDialog,
    private employeeService: EmployeeService,
    private baseService: BaseService,
    private router: Router
  ) {
    this.isModelLoaded = false;
    this.filterModel.sort = 'createdOn';
    this.filterModel.order = 'desc';
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => {
      this.paginator.pageIndex = 0;
      if (this.showInactive) {
        this.employeeStatus = this.constants.recordStatus.inactive;
        this.getEmployees();
      } else {
        this.employeeStatus = this.constants.recordStatus.active;
        this.getEmployees();
      }
    });
    this.paginator.page.subscribe(() => {
      if (this.showInactive) {
        this.employeeStatus = this.constants.recordStatus.inactive;
        this.getEmployees();
      } else {
        this.employeeStatus = this.constants.recordStatus.active;
        this.getEmployees();
      }
    });
    if (this.showInactive) {
      this.employeeStatus = this.constants.recordStatus.inactive;
      this.getEmployees();
    } else {
      this.employeeStatus = this.constants.recordStatus.active;
      this.getEmployees();
    }
  }

  closeDrawer() {
    this.drawer.close();
    this.getEmployees();
  }

  addNote(employeeId: number): void {
    const dialogRef = this.dialog.open(UpsertNoteComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        employeeId: employeeId,
      },
    });
    dialogRef.afterClosed().subscribe((confirm) => {
      if (confirm) {
        this.router.navigate(['/admin/manager', employeeId], {
          queryParams: { t: 5 },
        });
      } else {
        this.getEmployees();
      }
    });
  }

  viewNote(employeeId: number): void {
    const dialogRef = this.dialog.open(UpsertNoteComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        id: employeeId,
      },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.getEmployees();
    });
  }

  resetPassword(id: number): void {
    const dialRef = this.dialog.open(ResetPasswordComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: { id },
    });
  }

  getEmployees(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.updateFilterModel();
    this.employeeService
      .pageList(this.filterModel, this.employeeStatus)
      .subscribe({
        next: (response) => {
          this.model = response.items;
          this.model.forEach(x=> x.imageDetails != null ?
            x.imageUrl = this.getImageUrl(x.imageDetails.key) : x.imageUrl = null);
          this.totalCount = response.totalCount;
          this.blockUI.stop();
          this.isModelLoaded = true;
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.isModelLoaded = true;
          this.baseService.processErrorResponse(error);
        },
      });
  }

  getImageUrl(key:string):string{
    return environment.apiBaseUrl + '/documents/' + key;
  }

  deleteEmployee(id: number): void {
    const dialRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected employee.',
      },
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dialRef.afterClosed().subscribe((confirm) => {
      if (confirm) {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.employeeService.deleteEmployee(id).subscribe({
          next: () => {
            this.baseService.successNotification(
              'Employee has been deleted successfully.'
            );
            if (this.showInactive) {
              this.employeeStatus = this.constants.recordStatus.inactive;
              this.getEmployees();
            } else {
              this.employeeStatus = this.constants.recordStatus.active;
              this.getEmployees();
            }
            this.blockUI.stop();
            this.isModelLoaded = true;
          },
          error: (error: any) => {
            this.blockUI.stop();
            this.isModelLoaded = true;
            this.baseService.processErrorResponse(error);
          },
        });
      }
    });
  }

  resetFilterKey(): void {
    this.filterModel.filterKey = null;
    this.showInactive = false;
    this.employeeStatus = this.constants.recordStatus.active;
    this.getEmployees();
  }

  searchFilterKey(): void {
    if (this.showInactive) {
      this.employeeStatus = this.constants.recordStatus.inactive;
      this.getEmployees();
      this.paginator.firstPage();
    } else {
      this.employeeStatus = this.constants.recordStatus.active;
      this.getEmployees();
      this.paginator.firstPage();
    }
  }

  resetFilters(): void {
    this.filterModel = new FilterModel();
    this.paginator.firstPage();
  }

  updateFilterModel(): void {
    this.filterModel.sort = this.sort.active;
    this.filterModel.order = this.sort.direction;
    this.filterModel.pageIndex = this.paginator.pageIndex;
    this.filterModel.pageSize = this.paginator.pageSize;
  }

  onCheckboxChange(): void {
    if (this.showInactive) {
      this.employeeStatus = this.constants.recordStatus.inactive;
      this.getEmployees();
    } else {
      this.employeeStatus = this.constants.recordStatus.active;
      this.getEmployees();
    }
  }
}
