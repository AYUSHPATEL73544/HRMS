import { BlockUI, NgBlockUI } from "ng-block-ui";
import { EmployeeModel } from "../../../models";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Component, Inject } from "@angular/core";
import { AppUtils } from "src/app/utilities";

@Component({
  selector: 'app-employee-status',
  templateUrl: './employee-status.component.html',
})

export class EmployeeStatusComponent {
  @BlockUI('employee-status-blockui') blockUI: NgBlockUI;

  isResignationSelected = false;
  isTerminationSelected = false;
  title: string;
  message: string;
  model: EmployeeModel;
  exitTypeOptions = AppUtils.exitType();

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<EmployeeStatusComponent>
  ) {
    this.model = new EmployeeModel();
    this.title = data.title;
    this.model = data.model;

  }

  submit() {
    this.dialogRef.close(this.model);
  }

  selectChange(selectedOption: number) {
    if (selectedOption === 1) {
      this.selectTermination();
    }
    else if (selectedOption === 2) {
      this.selectResignation();
    }
  }
  selectResignation() {
    this.isResignationSelected = true;
    this.isTerminationSelected = false;
  }
  selectTermination() {
    this.isTerminationSelected = true;
    this.isResignationSelected = false;
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}