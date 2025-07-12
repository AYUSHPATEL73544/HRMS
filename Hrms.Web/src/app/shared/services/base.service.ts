import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { AppUtils, Constants } from 'src/app/utilities';

@Injectable()
export class BaseService {

    private snackBarConfig: MatSnackBarConfig;

    get constants() { return Constants }

    constructor(private snackBar: MatSnackBar,
        private appUtils: AppUtils) {
        this.snackBarConfig = new MatSnackBarConfig();
        this.snackBarConfig.duration = 5000;
        this.snackBarConfig.verticalPosition = 'top';
        this.snackBarConfig.horizontalPosition = 'right';
    }

    processErrorResponse(err: any): void {
        this.snackBarConfig.panelClass = ['snackbar-error'];
        this.appUtils.processErrorResponse(this.snackBar, this.snackBarConfig, err);
    }

    successNotification(message: any) {
        this.snackBarConfig.panelClass = ['snackbar-success'];
        this.snackBar.open(message, 'X',this.snackBarConfig);
    }

    infoNotification(message: any) {
        this.snackBarConfig.panelClass = ['snackbar-info'];
        this.snackBar.open(message, 'X', this.snackBarConfig);
    }

    errorNotification(message: any) {
        this.snackBarConfig.panelClass = ['snackbar-error'];
        this.snackBar.open(message, 'X', this.snackBarConfig);
    }

}