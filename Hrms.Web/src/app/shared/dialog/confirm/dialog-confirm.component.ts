import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
    selector: 'app-dialog-confirm',
    templateUrl: './dialog-confirm.component.html'
})

export class DialogConfirmComponent {
    title: string;
    message: string;
   
    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<DialogConfirmComponent>) {
        this.title = data.title;
        this.message = data.message;
    }

    submit(): void {
        this.dialogRef.close(true);
    }

    cancel(): void {
        this.dialogRef.close(false);
    }
}
