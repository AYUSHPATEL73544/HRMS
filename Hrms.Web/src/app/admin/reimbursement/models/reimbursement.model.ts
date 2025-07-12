import { FileDetailModel } from "../../jobApplication/model";

export class ReimbursementModel {
    id: number;
    employeeId: number;
    employeeName: string;
    description: string;
    amount: number;
    remark: string;
    date: string;
    paymentDate: string;
    createdOn: string;
    status: number;
    documentDetails: FileDetailModel;
}