export class TotalLeaveCountModel {
    ruleId: number;
    startDate: string;
    endDate: string;

    toQueryString(): string {

        let queryString = '';

        if (this.ruleId != undefined && this.ruleId != null) {
            queryString += `&ruleId=${this.ruleId}`;
        }
        if (this.startDate != undefined && this.startDate != null) {
            queryString += `&startDate=${this.startDate}`;
        }
        if (this.endDate != undefined && this.endDate != null) {
            queryString += `&endDate=${this.endDate}`;
        }
        return queryString;
    }
}
