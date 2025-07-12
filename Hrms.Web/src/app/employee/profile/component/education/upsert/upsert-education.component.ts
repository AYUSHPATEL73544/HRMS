import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { BaseService } from 'src/app/shared/services';
import { CourseTypeService, QualificationTypeService } from 'src/app/employee/services';
import { EducationService } from 'src/app/employee/services/education.service';
import { EducationModel } from 'src/app/employee/profile/models/index';
import { SelectListItemModel } from 'src/app/shared/models';
import { UpsertTeamComponent } from 'src/app/admin/directory/components';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-upsert-education',
  templateUrl: './upsert-education.component.html',
})

export class UpsertEducationComponent implements OnInit {

  @BlockUI('blockui-upsert-education') blockUI: NgBlockUI;

  model = new EducationModel();
  qualifications = new Array<SelectListItemModel>();
  courseTypes = new Array<SelectListItemModel>();

  constructor(@Inject(MAT_DIALOG_DATA) data: any,
    private dialogRef: MatDialogRef<UpsertTeamComponent>,
    private service: EducationService,
    private courseService: CourseTypeService,
    private qualificationService: QualificationTypeService,
    private baseService: BaseService) {
    if (data) {
      this.model.id = data.id;
    }
  }

  ngOnInit(): void {
    if (this.model.id) {
      this.get();
    }
    this.getQualificationList();
    this.getCourseList();
  }


  cancel(): void {
    this.dialogRef.close();
  }

  get(): void {
    this.blockUI.start();
    this.service.getById(this.model.id).subscribe({
      next: (response) => {
        this.model = response;
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getQualificationList(): void {
    this.qualificationService.getSelectListItem().subscribe({
      next: (response) => {
        Object.assign(this.qualifications, response);
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getCourseList(): void {
    this.courseService.getSelectListItem().subscribe({
      next: (response) => {
        Object.assign(this.courseTypes, response);
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  submit(): void {
    this.model.start = AppUtils.getFormattedDate(this.model.start, null);
    this.model.start = AppUtils.getDate(this.model.start);
    if (this.model.end !== null) {
      this.model.end = AppUtils.getFormattedDate(this.model.end, null);
      this.model.end = AppUtils.getDate(this.model.end);
    }
    this.blockUI.start();
    if (this.model.id) {
      this.service.update(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification("Education details has been updated successfully.");
          this.dialogRef.close();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
    else {
      this.blockUI.start();
      this.service.add(this.model).subscribe({
        next: () => {
          this.blockUI.stop();
          this.baseService.successNotification("Education details has been added successfully.");
          this.dialogRef.close();
        },
        error: (error: any) => {
          this.blockUI.stop();
          this.baseService.processErrorResponse(error);
        }
      });
    }
  }
}