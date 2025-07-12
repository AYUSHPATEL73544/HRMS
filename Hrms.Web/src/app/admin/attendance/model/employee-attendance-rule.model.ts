export class EmployeeAttendanceModel {
    employeeId: number;
    ruleId: number;
    employeeIds: Array<number>;
    ruleIds: Array<number>
    employeeName: string;
    employeeCode: string;
    department: string;
    ruleAssigned: string
    status: number;
    isLateClockIn: boolean;
    constructor() {
        this.employeeIds = new Array<number>();
        this.ruleIds = new Array<number>();
    }
}