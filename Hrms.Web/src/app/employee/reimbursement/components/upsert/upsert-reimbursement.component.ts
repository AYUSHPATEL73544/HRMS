import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ReimbursementModel } from '../../models';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Guid } from 'guid-typescript';
import { BaseService, StorageService } from 'src/app/shared/services';
import { environment } from 'src/environments/environment';
import { AppUtils } from 'src/app/utilities';
import { ReimbursementService } from 'src/app/employee/services/reimbursement.service';

@Component({
    selector: 'app-upsert-reimbursement',
    templateUrl: './upsert-reimbursement.component.html'
})

export class UpsertReimbursementComponent implements OnInit {
    model: ReimbursementModel = new ReimbursementModel();
    @BlockUI('reimbursement-blockui') blockUI: NgBlockUI;
    title: string;
    id: number;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<UpsertReimbursementComponent>,
        private storageService: StorageService,
        private baseService: BaseService,
        private reimbursementService: ReimbursementService) {
        this.title = data.title;
        this.id = data.id;
    }

    ngOnInit(): void {
        if (this.id) {
            this.getReimbursement();
        }
    }

    onFileSelected(event: any): void {
        this.blockUI.start();
        if (!event.target.files || event.target.files.length === 0) {
            this.deleteFile();
            return;
        }
        const selectedFile = event.target.files.item(0);
        this.model.documentDetails.id = 1;
        this.model.documentDetails.name = selectedFile.name;
        this.model.documentDetails.key = `${Guid.create()}.${event.target.files.item(0).name.split('.').pop()}`;
        this.storageService.uploadSingleFile(selectedFile, this.model.documentDetails.key).subscribe({
            next: () => {
                this.blockUI.stop();
                this.baseService.successNotification("Document has been uploaded successfully.");
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    deleteFile(): void {
        this.blockUI.start();
        this.storageService.deleteSingleFile(this.model.documentDetails.key).subscribe({
            next: () => {
                this.blockUI.stop();
                this.baseService.successNotification("Document has been deleted successfully.");
                this.model.documentDetails.id = 0;
                this.model.documentDetails.name = null;
                this.model.documentDetails.key = null;
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    viewDocument(fileKey: string): void {
        window.open(environment.apiBaseUrl + '/documents/' + fileKey, '_blank');
    }

    cancel(): void {
        this.dialogRef.close(false);
    }

    submit(): void {
        if (this.id) { this.edit(); }
        else { this.add(); }
    }

    add(): void {
        this.blockUI.start();
        this.model.date = AppUtils.getLocalFormattedDate(this.model.date);
        this.model.date = AppUtils.getDate(this.model.date);
        this.reimbursementService.addReimbursement(this.model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.dialogRef.close(true);
                this.baseService.successNotification("Detail has been submitted successfully.");
            },
            error: (error) => {
                this.blockUI.stop();
                this.dialogRef.close(false);
                this.baseService.errorNotification(error);
            }
        });
    }

    edit(): void {
        this.blockUI.start();
        this.model.id = this.id;
        this.reimbursementService.update(this.model).subscribe({
            next: () => {
                this.blockUI.stop();
                this.dialogRef.close(true);
                this.baseService.successNotification("Updated Successfully.");
            },
            error: (error) => {
                this.blockUI.stop();
                this.dialogRef.close(false);
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getReimbursement(): void {
        this.blockUI.start();
        this.reimbursementService.getById(this.id).subscribe({
            next: (res) => {
                this.blockUI.stop();
                this.model.description = res['description'];
                this.model.amount = res['amount'];
                this.model.date = res['date'];
                this.model.documentDetails.id = 1;
                this.model.documentDetails.name = res['documentDetails'].name;
                this.model.documentDetails.key = res['documentDetails'].key; 
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }
}