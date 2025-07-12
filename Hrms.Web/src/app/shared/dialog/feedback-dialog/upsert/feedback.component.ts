import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { InterviewModel } from 'src/app/shared/models';
import { BaseService, InterviewService } from 'src/app/shared/services';
import { AppUtils } from 'src/app/utilities';

@Component({
    selector: 'app-feedback',
    templateUrl: './feedback.component.html'
})

export class FeedbackComponent {
    @BlockUI('blockui-feedback') blockUI: NgBlockUI;
    title: string;
    selectedRating = 0;
    model = new InterviewModel();
    isModelLoaded = false;
    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private service: InterviewService,
        private dialogRef: MatDialogRef<FeedbackComponent>,
        private baseService: BaseService) {
        this.title = data.title;
        this.model = data.model;
    }

    submit(): void {
        this.model.rating = this.selectedRating ;
        this.model.interviewDate = new Date().toString();
        this.model.interviewDate = AppUtils.getFormattedDate(this.model.interviewDate, null);
        this.model.interviewDate = AppUtils.getDate(this.model.interviewDate);
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.update(this.model).subscribe({
            next: () => {
                this.isModelLoaded = true;
                this.blockUI.stop();
                this.baseService.successNotification('feedback has been added successfully.');
                this.dialogRef.close(false);
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    onSliderChange(event: any) {
        this.selectedRating = event.value;
    }

    cancel(): void {
        this.dialogRef.close(false);
    }
}