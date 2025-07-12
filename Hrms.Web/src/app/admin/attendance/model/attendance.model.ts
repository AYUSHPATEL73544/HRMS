import { AttendanceLogModel } from "./attendance-log.model";
import { AttendanceRuleModel } from "./attendance-rule.model";

export class AttendanceModel {
    id: number;
    employeeId: number;
    date: string;
    status: string;
    logs: Array<AttendanceLogModel>;
    workDuration: string;
    firstClockIn: string;
    lastClockOut: string;
    graceInTime:string;
    employeeName: string;
    employeeCode: string;
    isLateClockIn: boolean;
    isWorkDUrationLess: boolean;
    attendanceRule : AttendanceRuleModel;
    
    constructor() {
        this.logs = new Array<AttendanceLogModel>();
    }
}