import { Component } from "@angular/core";

@Component({
    selector: 'app-reimbursement',
    templateUrl: './reimbursement-manage.component.html'
})

export class ReimbursementComponent {
    selectedTabIndex = 0;

    onTableChanged(event: any): void {
        this.selectedTabIndex = event;
    }
}