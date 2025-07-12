import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { CandidateModel } from 'src/app/admin/jobApplication/model';
import { BaseService, CandidateService } from 'src/app/shared/services';
import { AppUtils, Constants } from 'src/app/utilities';

@Component({
    selector: 'app-interview-detail',
    templateUrl: './interview-detail.component.html'
})
export class InterviewDetailComponent implements OnInit{
    @BlockUI('blockui-interview-detail') blockUI: NgBlockUI;

    model = new CandidateModel();
    isModelLoaded = false;
    title: string;
    id: number;
    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private service: CandidateService,
        private dialogRef: MatDialogRef<InterviewDetailComponent>,
        private baseService: BaseService) {
        this.title = data.title;
        this.id = data.id;
    }

    get constants(): typeof Constants {
        return Constants;
    }
    
    ngOnInit(): void {
        this.getDetail();
    }

  
    getDetail():void{
        this.blockUI.start();
        this.isModelLoaded = false;
        this.service.getDetail(this.id).subscribe({
            next:(response) =>{
                this.model = response;
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

    cancel(): void {
        this.dialogRef.close(false);
    }
}