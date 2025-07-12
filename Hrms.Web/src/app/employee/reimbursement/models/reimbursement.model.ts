import { FileDetailModel } from "src/app/admin/jobApplication/model";

export class ReimbursementModel {
    id: number;
    description: string;
    amount: number;
    date: string;
    UpdatedDate: string;
    paymentDate: string;
    createdOn: string;
    rejectionReasion: string;
    status: string;
    documentDetails: FileDetailModel;
    documentFile: File;
    fileUploadPercent: any;

    constructor() {
        this.documentDetails = new FileDetailModel();
    }
}