import { Component, OnInit } from "@angular/core";
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CandidateChangeStatusModel, CandidateModel } from "../../model";
import { AppUtils, Constants } from "src/app/utilities";
import { ActivatedRoute } from "@angular/router";
import { DialogConfirmComponent, FeedbackComponent } from "src/app/shared/dialog";
import { MatDialog } from "@angular/material/dialog";
import { InterviewModel, SelectListItemModel } from "src/app/shared/models";
import { IntervieweeService, QualificationTypeService } from "src/app/employee/services";
import { BaseService, InterviewService } from "src/app/shared/services";
import { UpsertIntervieweeInterviewComponent } from "../upsert-interview/upsert-interviewee-interview.component";

@Component({
    selector: 'app-interviewee-detail',
    templateUrl: './interviewee-detail.component.html'
})
export class IntervieweeDetailComponent implements OnInit {
    @BlockUI('blockui-interviewee-detail') blockUI: NgBlockUI;

    model = new CandidateModel();
    isModelLoaded = false;
    qualifications = new Array<SelectListItemModel>();
    changeStatusModel = new CandidateChangeStatusModel();
    interviewModes = AppUtils.getInterviewModes();
    interviewTypes = AppUtils.getInterviewTypes();
    userId = AppUtils.getUserId();
    interviewModel = new Array<InterviewModel>();
    qualification: string;
    id: number;
    eligibleForNextRound: boolean;
    constructor(private dialog: MatDialog,
        private route: ActivatedRoute,
        private service: IntervieweeService,
        private interviewService: InterviewService,
        private qualificationService: QualificationTypeService,
        private baseService: BaseService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }
    ngOnInit(): void {
        this.getQualificationList();
        this.getDetail();
    }

    get constants(): typeof Constants {
        return Constants;
    }

    getDetail(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.getCandidate(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.qualification = this.qualifications.find(x => x.key == this.model.qualificationTypeId).value;
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.getDetailInterview();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getDetailInterview(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.interviewService.getListByCandidateId(this.model.id).subscribe({
            next: (response) => {
                this.interviewModel = response;
                this.interviewModel.forEach(element => {
                    this.id = element.id;
                    element.scheduleTime = AppUtils.getTime(element.scheduleTime);
                    this.eligibleForNextRound = element.eligibleForNextRound;
                });
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getQualificationList(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.qualificationService.getSelectListItem().subscribe({
            next: (response) => {
                Object.assign(this.qualifications, response);
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }


    changeStatus(model: CandidateChangeStatusModel): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.changeStatus(model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.successNotification('Candidate status has been changed successfully');
                this.getDetail();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    addFeedback(id:any): void {
        const interviewModel = this.interviewModel.find(interviewe => interviewe.id === id);
        const dialogRef = this.dialog.open(FeedbackComponent, {
            data: {
                title: `Add Feedback`,
                model: interviewModel
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe((result) => {
            this.getDetail();
        });
    }

    reject(id: number, status: number): void {
        const dialogRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: `Reject`,
                message: `Are you sure you want to reject the selected candidate?`,
                status: `4`
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatus(this.changeStatusModel);
            }
        });
    }

    editInterview(){
        const dialRef = this.dialog.open(UpsertIntervieweeInterviewComponent, {
            width: Constants.dialogSize.large,
            data: { id: this.id },
            disableClose: true
        });
        dialRef.afterClosed().subscribe(() => {
            this.getDetail();
        });
    }
}