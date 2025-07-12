import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { UpsertHolidayComponent } from '../../holidays/Components/upsert/upsert-holiday.component';
import { Constants } from 'src/app/utilities';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './dashboard.component.html'
})

export class DashboardComponent {
    @BlockUI('dashboard-blockUI') blockUI: NgBlockUI;

    isModelLoaded: boolean;
    constructor(private dialog: MatDialog) {
        this.isModelLoaded = false;
    }

    manageHoliday(): void {
        this.dialog.open(UpsertHolidayComponent, {
            width: Constants.dialogSize.medium,
            disableClose: true
        });
    }
}

