import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { InterviewModel, SelectListItemModel } from "src/app/shared/models";
import { BaseService, InterviewService, UserService } from "src/app/shared/services";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-upsert-interview',
    templateUrl: './upsert-interview.component.html'
})

export class UpsertInterviewComponent {
    @BlockUI('upsert-shortList-blockui') blockUI: NgBlockUI;

    model = new InterviewModel();
    interviewModes = AppUtils.getInterviewModes();
    interviewTypes = AppUtils.getInterviewTypes();
    interviewers = new Array<SelectListItemModel>();
    selectedRating = 0;
    addFeedBack = false;
    createdOn: string;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpsertInterviewComponent>,
        private userService: UserService,
        private baseService: BaseService,
        private service: InterviewService,
        public appUtils: AppUtils) {
        if (data) {
            this.model.id = data.id;
            this.model.candidateId = data.candidateId;
            this.createdOn = data.createdOn;
        }
    }

    ngOnInit(): void {
        this.getInterviewerList();
        if (this.model.id) {
            this.getInterviewDetail();
        }
    }


    getInterviewDetail(): void {
        this.blockUI.start();
        this.service.getDetail(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.model.scheduleTime = AppUtils.getTime(this.model.scheduleTime);
                this.selectedRating = this.model.rating;
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getInterviewerList(): void {
        this.blockUI.start();
        this.userService.getSelectListItem().subscribe({
            next: (response) => {
                this.interviewers = response;
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

    onSliderChange(event: any) {
        this.selectedRating = event.value;
        this.model.rating = this.selectedRating;
    }

    toggleFeedBack(event:any):void{
        this.addFeedBack = event.checked;
    }
    
    changeScheduleDate():void{
        this.model.interviewDate = null ;
    }

    submit(): void {
        this.blockUI.start();
        this.model.scheduleDate = AppUtils.getFormattedDate(this.model.scheduleDate, null);
        this.model.scheduleDate = AppUtils.getDate(this.model.scheduleDate);
        if (this.model.scheduleTime) {
            this.model.scheduleTime = AppUtils.getFormattedLocalTime(this.model.scheduleTime);
        }
        if(this.model.interviewDate){
            this.model.interviewDate = AppUtils.getFormattedDate(this.model.interviewDate, null);
            this.model.interviewDate = AppUtils.getDate(this.model.interviewDate);
        }
        if (this.model.id) {
            this.service.update(this.model).subscribe({
                next: () => {
                    this.blockUI.stop();
                    this.baseService.successNotification('Interview schedule has been updated successfully.');
                    this.dialogRef.close();
                },
                error: (error: any) => {
                    this.blockUI.stop();
                    this.baseService.processErrorResponse(error);
                }
            });
        }
        else {
            this.service.add(this.model).subscribe({
                next: () => {
                    this.blockUI.stop();
                    this.baseService.successNotification('Interview has been schedule successfully.');
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