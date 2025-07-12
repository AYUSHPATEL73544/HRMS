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
    isPursuing: boolean;
    skillIds: Array<number>;
    remark: string;
    isShortlisted: boolean;
    shortlistedDate: string;
    status: number;
    scheduleDate: string;
    document: any;
    scheduleTime: string;
    candidateId: number;
    marketingChannel: string;
    other: string;
    skillNames: Array<string>;
    constructor() {
        this.skillIds = new Array<number>();
        this.skillNames = new Array<string>();
    }
}