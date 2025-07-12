import { Component, Inject } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EducationModel } from 'src/app/employee/profile/models/index';


@Component({
  selector: 'app-education-view-detail',
  templateUrl: './education-view-detail.component.html',
})
export class EducationViewDetailComponent {
  @BlockUI('blockui-education-view-detail') blockUI: NgBlockUI;

  model: EducationModel;
  title: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<EducationViewDetailComponent>
  ) {
    this.model = new EducationModel();
    this.title = data.title;
    this.model = data.model;
  }
  cancel() {
    this.dialogRef.close({ log: null, confirm: false });
  }
}
