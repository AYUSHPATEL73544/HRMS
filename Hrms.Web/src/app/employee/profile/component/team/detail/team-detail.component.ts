import { Component, OnInit } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { TeamService } from 'src/app/employee/services';
import { BaseService } from 'src/app/shared/services';
import { TeamModel, TeamReportessModel } from 'src/app/employee/profile/models/index';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
})


export class TeamDetailComponent implements OnInit {

  @BlockUI('blockui-team') blockUI: NgBlockUI;

  displayedColumns = ['name', 'type', 'department', 'designation'];
  model = new Array<TeamModel>();
  reporteesModel = new Array<TeamReportessModel>();
  typeDropDown = AppUtils.getEmployeeTypeDropDown();

  constructor(
    private service: TeamService,
    private baseService: BaseService
  ) { }

  ngOnInit(): void {
    this.get();
    this.getReportessList();
  }

  get(): void {
    this.blockUI.start();
    this.service.get().subscribe({
      next: (response) => {
        this.model = response;
        this.model.forEach(element => {
          element.typeName = this.typeDropDown.find(x => x.key === element.type).value;
        });
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getReportessList(): void {
    this.blockUI.start();
    this.service.getReportessList().subscribe({
      next: (response) => {
        this.reporteesModel = response;
        this.reporteesModel.forEach(element => {
          element.typeName = this.typeDropDown.find(x => x.key === element.type).value;
        });
        this.blockUI.stop();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    })
  }
}