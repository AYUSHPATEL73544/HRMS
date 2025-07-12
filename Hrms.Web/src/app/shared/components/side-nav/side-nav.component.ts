import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChangePasswordComponent } from 'src/app/components';
import { AppUtils, Constants } from 'src/app/utilities';
import { DialogConfirmComponent } from '../../dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
})
export class SideNavComponent {
  sidenavRole: string;
  isHrRole: false;
  get constants(): typeof Constants {
    return Constants;
  }

  constructor(
    public appUtils: AppUtils,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.sidenavRole = AppUtils.getUserRole();
  }

  changePassword() {
    this.dialog.open(ChangePasswordComponent, {
      width: '500px',
      disableClose: true,
    });
  }

  getRole() {
    const role = AppUtils.getUserRole(); 
  }

  logout() {
    const dialogRef = this.dialog.open(DialogConfirmComponent, {
      data: {
        title: `Log out`,
        message: `Are you sure you want to log out?`,
      },
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.router.navigateByUrl('/account/logout');
      }
    });
  }
}
