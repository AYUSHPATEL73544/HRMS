import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-attendance-manage',
  templateUrl: './attendance-manage.component.html',
})

export class AttendanceManageComponent {

  selectedTabIndex = 0;

  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe((queryParams) => {
      if (queryParams['t']) {
        this.selectedTabIndex = Number.parseInt(queryParams['t']);
      }
    });
  }



  onTabChanged(index: number): void {
    this.selectedTabIndex = index;
  }
}