export class FilterModel {
  filterKey: string;
  sort: string;
  order: string;
  pageIndex: number;
  pageSize: number;

  constructor() {
    this.filterKey = null;
    this.pageIndex = 0;
    this.order = 'asc';
  }

  toQueryString(): string {
    let queryString = `sort=${this.sort}&order=${this.order}&pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`;

    if (this.filterKey !== '' && this.filterKey !== null && this.filterKey !== undefined) {
      queryString += '&filterKey=' + this.filterKey;
    }

    return queryString;
  }
}