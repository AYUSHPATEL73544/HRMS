import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseService } from 'src/app/shared/services';
import { EmployeeService, TeamService } from 'src/app/admin/services';
import { SelectListItemModel } from 'src/app/shared/models';
import { TeamModel } from 'src/app/admin/directory/models';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-upsert-team',
  templateUrl: './upsert-team.component.html',
})
export class UpsertTeamComponent implements OnInit {
  model = new TeamModel();
  employees = new Array<SelectListItemModel>();
  employee: string;
  typeDropDown = AppUtils.getEmployeeTypeDropDown();

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private employeeService: EmployeeService,
    private service: TeamService,
    private baseService: BaseService,
    private dialogRef: MatDialogRef<UpsertTeamComponent>
  ) {
    if (data) {
      this.model.id = data.id;
      this.model.employeeId = data.employeeId;
      this.model.managerId = data.managerId;
    }
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.get(this.model.id);
    }
    this.getManagerList();
  }

  get(id: number): void {
    this.service.getById(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      },
    });
  }

  getManagerList(): void {
    this.employeeService.getManagerSelectListItem().subscribe({
      next: (response) => {
        this.employees = response;
        if (this.model.employeeId > 0) {
          this.employee = this.employees.find(
            (x) => x.key == this.model.employeeId
          ).value;
        }
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      },
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }

  submit(): void {
    if (this.model.id) {
      this.service.update(this.model).subscribe({
        next: () => {
          this.baseService.successNotification(
            'Manager details has been updated successfully.'
          );
          this.dialogRef.close();
        },
        error: (error: any) => {
          this.baseService.processErrorResponse(error);
        },
      });
    } else {
      this.service.add(this.model).subscribe({
        next: () => {
          this.baseService.successNotification(
            'Manager details has been added successfully.'
          );
          this.dialogRef.close();
        },
        error: (error: any) => {
          this.baseService.processErrorResponse(error);
        },
      });
    }
  }
}
