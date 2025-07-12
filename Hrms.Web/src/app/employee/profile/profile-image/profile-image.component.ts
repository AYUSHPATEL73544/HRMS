import { Component, EventEmitter, Inject, Output } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Guid } from "guid-typescript";
import { AppUtils } from "src/app/utilities";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { EmployeeModel } from "../models/employee.model";
import { FileDetailModel } from "../models";
import { BaseService, DocumentService, ListenerService, StorageService } from "src/app/shared/services";

@Component({
    selector: 'app-profile-image',
    templateUrl: './profile-image.component.html'
})
export class ProfileImageComponent {
    @BlockUI('employee-blockui') blockUI: NgBlockUI;
    @Output() reloadProfile = new EventEmitter();

    employeeModel = new EmployeeModel();
    fileModel = new FileDetailModel();
    selectedImage: any;
    title: string;
    userId = AppUtils.getUserId();
    fileType: any;

    constructor(@Inject(MAT_DIALOG_DATA) data: any,
        private dialogRef: MatDialogRef<ProfileImageComponent>,
        private storageService: StorageService,
        private baseService: BaseService,
        private documentService: DocumentService,
        private listenerService: ListenerService
    ) {
        this.title = data.title;
        this.employeeModel = data.model;
    }


    ngOninit() {
        //this.employeeModel.imageDetails.id = 0;
        this.fileModel.id = 0;
    }

    onImageSelect(event: any) {
        this.blockUI.start();
        if (event.target.files && event.target.files[0]) {
            var reader = new FileReader();
            reader.readAsDataURL(event.target.files[0]);
            reader.onload = (event) => {
                this.selectedImage = event.target.result;
            }
        }
        this.blockUI.stop();

        this.fileModel.imageFile = event.target.files.item(0);
        this.fileType = this.fileModel.imageFile.type;

        const validImageTypes = ['image/jpeg', 'image/png'];
        if (!validImageTypes.includes(this.fileType)) {
            this.baseService.errorNotification("Not a valid image type. Please upload Png or Jpeg image.");
            return;
        }

        this.fileModel.id = 1;
        this.fileModel.name = this.fileModel.imageFile.name;
        this.fileModel.key = `${Guid.create()}.${event.target.files.item(0).name.split('.').pop()}`;

        this.blockUI.start();
        this.storageService.uploadSingleFile(this.fileModel.imageFile, this.fileModel.key).subscribe({
            next: () => {
                this.baseService.successNotification("Image loaded.");
                this.blockUI.stop();
            },
            error: (error: any) => {
                this.baseService.processErrorResponse(error);
                this.blockUI.stop();
            }
        })
    }


    add() {
        this.blockUI.start();
        this.documentService.addImage(this.fileModel).subscribe({
            next: () => {
                this.blockUI.stop();
                this.baseService.successNotification("Image added successfully.");
                this.listenerService.profileUpdateListener.next(null);
                this.dialogRef.close();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }

    update() {
        this.blockUI.start();
        this.documentService.updateImage(this.fileModel).subscribe({
            next: () => {
                this.blockUI.stop();
                this.baseService.successNotification("Image updated successfully.");
                this.listenerService.profileUpdateListener.next(null);
                this.dialogRef.close();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }


    submit() {
        if (this.employeeModel.imageDetails != null) {
            this.update();
        }
        else {
            this.add();
        }
    }

    deleteFile(): void {
        this.blockUI.start();
        this.storageService.deleteSingleFile(this.fileModel.key).subscribe({
            next: () => {
                this.blockUI.stop();
                this.baseService.successNotification("Image removed.");
                this.fileModel.id = 0;
                this.fileModel.name = null;
                this.fileModel.key = null;
                this.selectedImage = null;
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    cancel(): void {
        this.dialogRef.close(false);
    }
}