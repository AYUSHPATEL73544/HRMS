import { FileDetailModel } from "src/app/employee/job-recruitment/model";

export class InterviewModel {
    id: number;
    interviewMode: number;
    interviewType: number;
    interviewDate: string;
    scheduleDate: string;
    scheduleTime: string;
    interviewerId: number;
    candidateId: number;
    rating: number;
    firstName:string;
    lastName:string;
    eligibleForNextRound: boolean;
    remark: string;
    legalName: string;
    email: string;
    phone:string;
    interviewerName:string;
    courseName:string;
    stream:string;
    passingYear: number;
    shortlistedDate: string;
    status: string;
    documentDetails: FileDetailModel;

    constructor(){
        this.eligibleForNextRound = null;
        this.documentDetails = new FileDetailModel();
    }
}