import { FilterModel } from "src/app/shared/models";

export class ReimbursementFilterModel extends FilterModel {
    startDate: string;
    endDate: string;

    override toQueryString(): string {
        let queryString = super.toQueryString();

        if (this.startDate != null && this.startDate != undefined) {
            queryString += `&startDate=${this.startDate}`;
        }
        if (this.endDate != null && this.endDate != undefined) {
            queryString += `&endDate=${this.endDate}`;
        }

        return queryString;
    }
}