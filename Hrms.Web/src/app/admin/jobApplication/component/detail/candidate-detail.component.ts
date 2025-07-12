import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CandidateChangeStatusModel, CandidateModel } from 'src/app/admin/jobApplication/model';
import { CandidateService, QualificationTypeService } from 'src/app/admin/services';
import { DeleteComponent, DialogConfirmComponent, RejectComponent } from 'src/app/shared/dialog';
import { InterviewModel, SelectListItemModel } from 'src/app/shared/models';
import { BaseService, InterviewService } from 'src/app/shared/services';
import { AppUtils, Constants } from 'src/app/utilities';
import { UpsertInterviewComponent } from '../upsert-interview/upsert-interview.component';
import { UpsertHireComponent } from '../upsert-hire/upsert-hire.component';


@Component({
    selector: 'app-candidate-detail',
    templateUrl: './candidate-detail.component.html'
})
export class CandidateDetailComponent implements OnInit {
    @BlockUI('blockui-candidate-detail') blockUI: NgBlockUI;

    model = new CandidateModel();
    isModelLoaded = false;
    title: string;
    qualifications = new Array<SelectListItemModel>();
    changeStatusModel = new CandidateChangeStatusModel();
    interviewers = new Array<SelectListItemModel>();
    interviewModes = AppUtils.getInterviewModes();
    interviewTypes = AppUtils.getInterviewTypes();
    interviewModel = new Array<InterviewModel>();
    qualification: string;
    eligibleForNextRound: boolean;
    firstElement: boolean;
    isShortlisted: boolean;
    isHired: boolean;
    id: number;

    constructor(private dialog: MatDialog,
        private route: ActivatedRoute,
        private router: Router,
        private service: CandidateService,
        private interviewService: InterviewService,
        private qualificationService: QualificationTypeService,
        private baseService: BaseService) {
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });
    }

    get constants(): typeof Constants {
        return Constants;
    }

    ngOnInit(): void {
        this.getQualificationList();
        this.getDetail();
    }


    getDetail(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.getCandidate(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.qualification = this.qualifications.find(x => x.key == this.model.qualificationTypeId).value;
                this.isShortlisted = this.model.isShortlisted;
                this.isHired = this.model.isHired;
                this.getDetailInterview();
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

    getDetailInterview(): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.interviewService.getListByCandidateId(this.model.id).subscribe({
            next: (response) => {
                this.interviewModel = response;
                this.interviewModel.forEach(element => {
                    this.id = element.id;
                    element.scheduleTime = AppUtils.getTime(element.scheduleTime);
                });
                if (this.interviewModel.length == 0) {
                    this.firstElement = true;
                }
                if (this.interviewModel.length > 0) {
                    const last = this.interviewModel[this.interviewModel.length - 1];
                    this.eligibleForNextRound = last.eligibleForNextRound;
                }
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

    reject(id: number, status: number): void {
        const dialogRef = this.dialog.open(RejectComponent, {
            data: {
                title: `Reject`,
                message: `Are you sure you want to reject the selected candidate?`,
                status: `4`
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result.confirm) {
                if (result.log != null) {
                    this.changeStatusModel.rejectionReason = result.log;
                }
                this.changeStatusModel.id = id;
                this.changeStatusModel.status = status;
                this.changeStatus(this.changeStatusModel);
            }
        });
    }

    shortlist(id: number): void {
        const dialRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: 'Shortlist Candidate',
                message: 'Are you sure you want to shortlist selected candidate?'
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.blockUI.start();
                    this.service.shortlist(id).subscribe({
                        next: () => {
                            this.baseService.successNotification("Candidate has been shortlisted successfully.");
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.getDetail();
                        },
                        error: (error: any) => {
                            this.blockUI.stop();
                            this.isModelLoaded = true;
                            this.baseService.processErrorResponse(error);
                        }
                    });
                }
                this.getDetail();
            }
        );
    }

    hireSure(): void {
        const dialRef = this.dialog.open(DialogConfirmComponent, {
            data: {
                title: 'Hire Candidate',
                message: 'Are you sure you want to hire selected candidate?'
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.hire(this.model.id);
                }
                else{
                    this.isHired = false;
                }
            }
            
        );
    }
    
    hire(id: number): void {
        const dialRef = this.dialog.open(UpsertHireComponent, {
            data: {
               id : id,
               firstName : this.model.firstName,
               lastName : this.model.lastName,
               phone : this.model.phone,
               gender : this.model.gender
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.baseService.successNotification("Candidate has been hired successfully.");
                }
                this.getDetail();
            }
        );
    }

    scheduleInterview(id: number): void {
        const dialRef = this.dialog.open(UpsertInterviewComponent, {
            width: Constants.dialogSize.large,
            data: {
                candidateId: id,
                createdOn: this.model.createdOn

            },
            disableClose: true
        });
        dialRef.afterClosed().subscribe(() => {
            this.getDetail();
        });
    }


    editInterview(): void {
        const dialRef = this.dialog.open(UpsertInterviewComponent, {
            width: Constants.dialogSize.large,
            data: {
                id: this.id,
                createdOn: this.model.createdOn

            },
            disableClose: true
        });
        dialRef.afterClosed().subscribe(() => {
            this.getDetail();
        });
    }

    changeStatus(model: CandidateChangeStatusModel): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.changeStatus(model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                if(model.status === 4){
                    this.baseService.successNotification('Candidate has been rejected successfully.');
                    this.getDetail();
                }
                if (model.status === 5) {
                    this.baseService.successNotification('Candidate has been deleted successfully.');

                }
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    delete(id: number, status: number): void {
        const dialRef = this.dialog.open(DeleteComponent, {
            data: {
                title: 'Delete',
                message: 'Are you sure you want to delete selected candidate?',
                status: '5'
            },
            width: Constants.dialogSize.medium,
            disableClose: true
        });
        dialRef.afterClosed().subscribe(
            (confirm) => {
                if (confirm) {
                    this.changeStatusModel.id = id;
                    this.changeStatusModel.status = status;
                    this.changeStatus(this.changeStatusModel);
                    this.router.navigate(['/admin/candidate']);

                }
            }
        );
    }

    toggleShortList(event: any): void {
        this.isShortlisted = event.checked;
        if (this.isShortlisted) {
            this.shortlist(this.model.id);
        }
    }

    toggleHire(event: any): void {
        this.isHired = event.checked;
        if (this.isHired) {
            this.hireSure();
        }
    }
}