export class LeaveModel {
    id: number;
    employeeId: number;
    ruleId: number;
    total: number;
    credited: number;
    available: number;
    applied: number;
    employeeIds: Array<number>;
    ruleIds: Array<number>;
    employeeName: string;
    employeeCode: string;
    leaveRule: string
    department: string;
    status: number;
    leaveRuleStatus: number;
    leaveRules: Array<string>;
    createdOn: string;
    constructor() {
        this.employeeIds = new Array<number>();
        this.ruleIds = new Array<number>();
        this.leaveRules = new Array<string>();
    }
}