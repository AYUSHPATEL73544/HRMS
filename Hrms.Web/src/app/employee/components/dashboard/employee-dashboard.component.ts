import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ChangePasswordComponent } from "src/app/components";

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html'
})

export class EmployeeDashboardComponent {

  constructor(private dialog: MatDialog) { }

  changePassword() {
    this.dialog.open(ChangePasswordComponent, {
      width: '500px',
      disableClose: true,
    });
  }

}
