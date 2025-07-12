export class AttendanceLogModel {
    id: number;
    attendanceId: number;
    employeeStatus: number;
    inTime: string;
    outTime: string;
    graceInTime: string;
    latitude: number;
    note: string;
    longitude: number;
    status: string;
    firstName: string;
    lastName: string;
    date: string;
    isWorkDurationLess: boolean;
    employeeName: string;
    employeeId: number;
    employeeCode: string;
    totalhours: string;
    workDuration: number;
    isLateClockIn:boolean;
}