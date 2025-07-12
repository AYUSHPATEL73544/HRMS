import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { NoteModel } from 'src/app/admin/directory/models';
import { NoteService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';

@Component({
  selector: 'app-note-upsert',
  templateUrl: './upsert-note.component.html',
})
export class UpsertNoteComponent implements OnInit {
  @BlockUI('note-blockUI') blockUI: NgBlockUI;
  model = new NoteModel();
  id = 0;
  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertNoteComponent>,
    private service: NoteService,
    public baseService: BaseService
  ) {
    if (data) {
      this.model.employeeId = data.employeeId;
      this.model.id = data.id;
    }
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.getNote(this.model.id);
    }
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  getNote(id: number): void {
    this.blockUI.start();
    this.service.getNoteById(id).subscribe({
      next: (response) => {
        this.blockUI.stop();
        this.model = response;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      },
    });
  }

  submit(): void {
    if (this.model.id) {
      this.blockUI.start();
      this.service.update(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification(
            'Note has been updated successfully.'
          );
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        },
      });
    } else {
      this.blockUI.start();
      this.service.add(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification(
            'Note has been added successfully.'
          );
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        },
      });
    }
  }
}
