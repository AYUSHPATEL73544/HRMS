import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-employee-profile-manage',
  templateUrl: './employee-profile-manage.component.html',
})
export class EmployeeProfileManageComponent implements AfterViewInit {
  @ViewChild('personal') personalComponent: any;
  @ViewChild('work') workComponent: any;
  @ViewChild('team') teamComponent: any;
  @ViewChild('education') educationComponent: any;
  @ViewChild('family') familyComponent: any;
  @ViewChild('note') noteComponent: any;

  selectedTabIndex = 0;

  constructor(private route: ActivatedRoute) {
    this.route.queryParams.subscribe((queryParams) => {
      if (queryParams['t']) {
        this.selectedTabIndex = Number.parseInt(queryParams['t']);
      }
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      switch (this.selectedTabIndex) {
        case 0:
          this.personalComponent.reloadDetails();
          break;
        case 1:
          this.workComponent.reloadDetails();
          break;
        case 2:
          this.teamComponent.reloadDetails();
          break;
        case 3:
          this.educationComponent.reloadDetails();
          break;
        case 4:
          this.familyComponent.reloadDetails();
          break;
        case 5:
          this.noteComponent.reloadDetails();
          break;
      }
    }, 100);
  }

  onTabChanged(event: any): void {
    this.selectedTabIndex = event.index;
    // switch (event.index) {
    //   case 0:
    //     this.personalComponent.reloadDetails();
    //     break;
    //   case 1:
    //     this.workComponent.reloadDetails();
    //     break;
    //   case 2:
    //     this.teamComponent.reloadDetails();
    //     break;
    //   case 3:
    //     this.educationComponent.reloadDetails();
    //     break;
    //   case 4:
    //     this.familyComponent.reloadDetails();
    //     break;
    //   case 5:
    //     this.noteComponent.reloadDetails();
    //     break;
    // }
  }
}
