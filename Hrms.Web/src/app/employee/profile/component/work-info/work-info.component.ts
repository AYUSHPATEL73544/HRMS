import { Component, EventEmitter, Input, Output, OnInit, AfterViewInit } from "@angular/core";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { EmployeeModel, WorkHistoryModel } from "src/app/employee/profile/models/index";
import { SelectListItemModel } from "src/app/shared/models";
import { WorkHistoryService } from "src/app/employee/services";
import { BaseService } from "src/app/shared/services";
import { Constants } from "src/app/utilities";

@Component({
  selector: 'app-work-info',
  templateUrl: './work-info.component.html',
})


export class WorkInfoComponent implements OnInit, AfterViewInit {
  @BlockUI('blockui-work') blockUI: NgBlockUI;

  @Input() model: EmployeeModel;
  @Output() reloadProfile = new EventEmitter();

  displayedColumns = ['department', 'designation', 'from', 'to'];

  workHistoryModel = new Array<WorkHistoryModel>();
  isBasicInfoEditable = false;
  isWorkHistoryEditable = false;
  isExitEditable = false;
  currentDate = Date();
  employeeType: string;
  isModelLoaded: boolean;

  get constants(): typeof Constants {
    return Constants;
  }

  constructor(private workHistoryService: WorkHistoryService,
    private baseService: BaseService
  ) {
    this.isModelLoaded = false;
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.getWork();
  }

  getWork(): void {
    this.blockUI.start();
    this.isModelLoaded = false;
    this.workHistoryService.get().subscribe({
      next: (response) => {
        this.workHistoryModel = response;
        this.blockUI.stop();
        this.isModelLoaded = true;
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.isModelLoaded = true;
        this.baseService.processErrorResponse(error);
      }
    });
  }

  cancel(): void {
    this.isBasicInfoEditable = false;
    this.isWorkHistoryEditable = false;
    this.isExitEditable = false;
  }
}