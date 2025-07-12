import { FilterModel } from "src/app/shared/models";

export class LeaveLogFilterModel extends FilterModel {
    leaveLogId: number;
    startDate: string;
    endDate: string;

    override toQueryString(): string {
        
        let queryString = super.toQueryString();

        if (this.leaveLogId != undefined && this.leaveLogId != null) {
            queryString += `&leaveLogId=${this.leaveLogId}`;
        }
        if(this.startDate != undefined && this.startDate != null){
            queryString += `&startDate=${this.startDate}`;
        }
        if(this.endDate != undefined && this.endDate != null){
            queryString += `&endDate=${this.endDate}`;
        }
        return queryString;
    }
}