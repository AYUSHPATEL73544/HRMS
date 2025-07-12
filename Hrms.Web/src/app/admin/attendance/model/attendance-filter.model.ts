import { FilterModel } from "src/app/shared/models";

export class AttendanceFilterModel extends FilterModel {
    attendanceLogId: number;
    startDate: string;
    inTime: string;
    endDate: string;

    override toQueryString(): string {
        let queryString = super.toQueryString();

        if (this.attendanceLogId != null || this.attendanceLogId != undefined) {
            queryString += `&attendanceLogId=${this.attendanceLogId}`;
        }
        if(this.startDate != null || this.startDate != undefined){
            queryString += `&startDate=${this.startDate}`;
        }
        if(this.endDate != null || this.endDate != undefined){
            queryString += `&endDate=${this.endDate}`;
        }
        if(this.inTime != null || this.inTime != undefined){
            queryString += `&inTime=${this.inTime}`;
        }

        return queryString;
    }
}