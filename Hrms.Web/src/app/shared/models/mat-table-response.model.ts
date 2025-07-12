export class MatTableResponseModel {
    totalCount: number;
    items: Array<any>;

    constructor() {
        this.totalCount = 0;
        this.items = new Array<any>();
    }
}
