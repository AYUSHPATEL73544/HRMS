import { FileDetailModel } from "./file-detail.model";

export class CandidateModel {
    id: number;
    name: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    qualificationTypeId: number;
    courseTypeId: number;
    courseName: string;
    stream: string;
    passingYear: number;
    createdOn: string;
    isPursuing: boolean = false;
    skillIds: Array<number>;
    skillNames: Array<string>;
    remark: string;
    gender: number;
    isHired:boolean;
    isShortlisted: boolean;
    shortlistedDate: string;
    documentDetails: FileDetailModel;
    document: any;
    fileUploadPercent: any;
    interviewType: number;
    interviewMode: number;
    interviewerName: string;
    interviewId : number;
    documentFile: File;
    status: number;
    marketingChannel: string;
    marketingChannelType: string;
    other: string;
    
    constructor() {
        this.documentDetails = new FileDetailModel();
        this.skillIds = new Array<number>();
        this.skillNames = new Array<string>();
        this.isPursuing = false;
    }
}