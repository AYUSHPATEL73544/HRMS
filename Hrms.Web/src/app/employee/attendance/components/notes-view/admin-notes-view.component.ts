import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { AttendanceLogService } from 'src/app/employee/services/attendance-log.service';
import { BaseService } from "src/app/shared/services";
import { AttendanceLogModel } from "src/app/employee/attendance/model";
import { AppUtils } from "src/app/utilities";

@Component({
  selector: 'app-admin-notes-view',
  templateUrl: './admin-notes-view.component.html'
})

export class AdminNotesViewComponent implements OnInit {
  @BlockUI('blockui-note-view') blockUI: NgBlockUI;
  model = new AttendanceLogModel();
  id = 0;

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<any>,
    private attendanceLogService: AttendanceLogService,
    private baseService: BaseService,
  ) {
    if (data) {
      this.id = data.id;
    }
  }

  ngOnInit(): void {
    this.getDetail();
  }

  getDetail(): void {
    this.blockUI.start();
    this.attendanceLogService.getDetail(this.id).subscribe({
      next: (response) => {
        this.model = response;
        this.model.inTime = AppUtils.getTime(this.model.inTime);
        this.model.outTime = AppUtils.getTime(this.model.outTime);
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}