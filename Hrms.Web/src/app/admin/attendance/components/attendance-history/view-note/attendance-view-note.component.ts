import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AttendanceLogModel } from '../../../model';

@Component({
  selector: 'app-attendance-view-note',
  templateUrl: './attendance-view-note.component.html',
})
export class AttendanceViewNoteComponent {
  @BlockUI('blockui-view-Note-detail') blockUI: NgBlockUI;

  model: AttendanceLogModel;
  title: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<AttendanceViewNoteComponent>
  ) {
    this.model = new AttendanceLogModel();
    this.title = data.title;
    this.model = data.model;
  }
  cancel() {
    this.dialogRef.close({ log: null, confirm: false });
  }
}



