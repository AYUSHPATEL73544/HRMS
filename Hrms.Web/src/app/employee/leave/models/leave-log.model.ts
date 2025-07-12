export class LeaveLogModel {
    id: number;
    employeeId: number;
    ruleId: number;
    startDate: string;
    endDate: string;
    startHalf: number;
    endHalf: number;
    createdOn: string;
    days: number;
    purpose: string;
    type: number;
    status: string;
    rejectionReason: string;
}