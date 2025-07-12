import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html'
})

export class DeleteComponent {
  @BlockUI('blockui-delete') blockUI: NgBlockUI;
  title: string;
  message: string;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<DeleteComponent>) {
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
