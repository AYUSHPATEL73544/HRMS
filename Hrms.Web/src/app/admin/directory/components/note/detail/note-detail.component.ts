import { Component, OnInit } from '@angular/core';
import { NoteService } from 'src/app/admin/services';
import { BaseService } from 'src/app/shared/services';
import { NoteModel } from 'src/app/admin/directory/models';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { UpsertNoteComponent } from '../upsert/upsert-note.component';
import { Constants } from 'src/app/utilities';
import { DeleteComponent } from 'src/app/shared/dialog';

@Component({
  selector: 'app-note-detail',
  templateUrl: './note-detail.component.html',
})
export class NoteDetailComponent implements OnInit {
  @BlockUI('note-blockUI') blockUI: NgBlockUI;
  model = new Array<NoteModel>();
  id = 0;
  employeeId = 0;
  displayedColumns = ['note', 'createdOn', 'action'];

  constructor(
    private service: NoteService,
    public baseService: BaseService,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });
  }

  ngOnInit(): void {
    this.getNotes();
  }

  getNotes(): void {
    this.blockUI.start();
    this.service.getNoteList(this.id).subscribe({
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

  addNote(): void {
    const dialogRef = this.dialog.open(UpsertNoteComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        employeeId: this.id,
      },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.getNotes();
    });
  }

  editNote(id: Number): void {
    const dialogRef = this.dialog.open(UpsertNoteComponent, {
      width: Constants.dialogSize.medium,
      disableClose: true,
      data: {
        id: id,
      },
    });
    dialogRef.afterClosed().subscribe(() => {
      this.getNotes();
    });
  }

  deleteNote(id: number): void {
    const dialogRef = this.dialog.open(DeleteComponent, {
      data: {
        title: 'Delete',
        message: 'Are you sure you want to delete selected note?',
      },
      width: Constants.dialogSize.medium,
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((confirm) => {
      if (confirm) {
        this.blockUI.start();
        this.service.delete(id).subscribe({
          next: () => {
            this.blockUI.stop();
            this.baseService.successNotification(
              'Note has been deleted successfully.'
            );
            this.getNotes();
          },
          error: (error: any) => {
            this.blockUI.stop();
            this.baseService.processErrorResponse(error);
          },
        });
      }
      this.getNotes();
    });
  }

  reloadDetails(): void {
    this.getNotes();
  }
}
