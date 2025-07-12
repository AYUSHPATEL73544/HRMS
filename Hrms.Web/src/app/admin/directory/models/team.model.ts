export class TeamModel {
    id: number;
    employeeId: number;
    managerId: number;
    type: number;
    department: string;
    designation: string;
    managerName: string;
    employeeName: string;
    canEdit: boolean;
    typeName: string;
    canDirectEdit: boolean;
}