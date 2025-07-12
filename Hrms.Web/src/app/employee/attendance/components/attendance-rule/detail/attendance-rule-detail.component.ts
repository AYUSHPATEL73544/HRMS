import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AttendanceService } from 'src/app/employee/services/attendance.service';
import { BaseService } from 'src/app/shared/services';
import { AttendanceRuleModel } from 'src/app/employee/attendance/model/index';
import { AppUtils } from 'src/app/utilities';


@Component({
    selector: 'app-attendance-rule-detail',
    templateUrl: './attendance-rule-detail.component.html',
})


export class AttendanceRulesDetailComponent implements OnInit {
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;
    model = new AttendanceRuleModel();
    model1 = new AttendanceRuleModel();
    weekDays = AppUtils.getWeekDaysForDropDown();
    years = AppUtils.getYears(); 
    isYearSelected = false;

    graceInTime: string;
    graceOutTime: string;
    startDay: string;
    endDay: string;
    isModelLoaded: boolean;

    totalFirstHalfDuration: string;
    totalSecondHalfDuration: string;
    isOverviewEditable = false;
    isTimingEditable = false;
    isWorkDurationEditable = false;
    someSubscription:any;


    constructor(
        private route: ActivatedRoute,
        private attendanceService: AttendanceService,
        private baseService: BaseService,
        private router: Router
    ) {
        this.isModelLoaded = false;
        this.route.params.subscribe((params) => {
            this.model.id = params['id'];
        });

        
        this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
          };
          this.someSubscription = this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
              this.router.navigated = false;
            }
          });
    }

    ngOnInit(): void {
        this.getRuleDetails(this.model.id);
    }


    getRuleDetails(id: number): void {
        this.blockUI.start();
        this.isModelLoaded = false;
        this.attendanceService.getDetail(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.startDay = this.weekDays.find(x => x.key == this.model.startDay).value;
                this.endDay = this.weekDays.find(x => x.key == this.model.endDay).value;
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.getTime();
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });
    }

    submit(): void {
        this.getInputFormattedTime();
        this.blockUI.start();
        this.isModelLoaded = false;
        this.attendanceService.updateAttendanceRule(this.model).subscribe({
            next: () => {
                this.baseService.successNotification('Attendance rule has been added successfully.');
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.isOverviewEditable = false;
                this.isTimingEditable = false;
                this.isWorkDurationEditable = false;
                this.getTime();

            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.baseService.processErrorResponse(error);
            }
        });

    }

    getTime(): void {
        this.model.inTime = AppUtils.getTime(this.model.inTime);
        this.model.outTime = AppUtils.getTime(this.model.outTime);
        this.model.graceInTime = AppUtils.getTime(this.model.graceInTime);
        this.model.graceOutTime = AppUtils.getTime(this.model.graceOutTime);
        this.model.firstHalfStart = AppUtils.getTime(this.model.firstHalfStart);
        this.model.firstHalfEnd = AppUtils.getTime(this.model.firstHalfEnd);
        this.model.secondHalfStart = AppUtils.getTime(this.model.secondHalfStart);
        this.model.secondHalfEnd = AppUtils.getTime(this.model.secondHalfEnd);
        this.model.totalBreakDuration = AppUtils.getDifferenceInHours(this.model.firstHalfEnd, this.model.secondHalfStart);
        this.graceInTime = AppUtils.getDifferenceInMinutes(this.model.inTime, this.model.graceInTime);
        this.graceOutTime = AppUtils.getDifferenceInMinutes(this.model.outTime, this.model.graceOutTime);
        this.totalFirstHalfDuration = AppUtils.getDifferenceInHours(this.model.firstHalfStart, this.model.firstHalfEnd);
        this.totalSecondHalfDuration = AppUtils.getDifferenceInHours(this.model.secondHalfStart, this.model.secondHalfEnd);
    }

    getInputFormattedTime(): void {
        this.model.inTime = AppUtils.getFormattedLocalTime(this.model.inTime);
        this.model.graceInTime = AppUtils.getFormattedLocalTime(this.model.graceInTime);
        this.model.outTime = AppUtils.getFormattedLocalTime(this.model.outTime);
        this.model.graceOutTime = AppUtils.getFormattedLocalTime(this.model.graceOutTime);
        this.model.firstHalfStart = AppUtils.getFormattedLocalTime(this.model.firstHalfStart);
        this.model.firstHalfEnd = AppUtils.getFormattedLocalTime(this.model.firstHalfEnd);
        this.model.secondHalfStart = AppUtils.getFormattedLocalTime(this.model.secondHalfStart);
        this.model.secondHalfEnd = AppUtils.getFormattedLocalTime(this.model.secondHalfEnd);
        this.model.minEffectiveDuration = AppUtils.getFormattedLocalTime(this.model.minEffectiveDuration);
        this.model.totalBreakDuration = AppUtils.getFormattedLocalTime(this.model.totalBreakDuration);
    }

    getRuleByYear(year: number) {
        this.isYearSelected = true;
        this.attendanceService.getbyYear(year).subscribe({
          next: (res: any) => {
            this.model1 = res;
            if (this.model1 == null) {
              this.baseService.errorNotification("No record available of selected year.");
            }
            this.router.navigate(['/employee/attendance-rule-detail',this.model1.id]);
          },
          error: (error: any) => {
            this.isModelLoaded = true;
            this.baseService.processErrorResponse(error);
          }
        })
      }

      ngOnDestroy() {
        if (this.someSubscription) {
          this.someSubscription.unsubscribe();
        }
      }

}