import { AppUtils } from "src/app/utilities";

export class AttendanceEventFilterModel {
    employeeId: number;
    year: number;
    month: number;

    constructor() {
        //this.employeeId = 1;
        this.year = AppUtils.getCurrentDate().getFullYear();
        this.month = AppUtils.getCurrentDate().getMonth() + 1;
    }
}