import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import * as moment from "moment";
import { ActivatedRoute, NavigationEnd, Router } from "@angular/router";
import { BaseService } from "src/app/shared/services";
import { AttendanceRuleService } from "src/app/admin/services";
import { AttendanceRuleModel } from "src/app/admin/attendance/model/index";
import { AppUtils } from "src/app/utilities";

@Component({
    selector: 'app-attendance-rule-detail',
    templateUrl: './attendance-rule-detail.component.html',
})


export class AttendanceRuleDetailComponent implements OnInit {
    @BlockUI('attendance-blockui') blockUI: NgBlockUI;
    model = new AttendanceRuleModel();
    model1 = new AttendanceRuleModel();
    weekDays = AppUtils.getWeekDaysForDropDown();
    years = AppUtils.getYears();
    // isYearSelected = false;
    // selectedYear:number;

    graceInTime: string;
    graceOutTime: string;
    startDay: string;
    endDay: string;
    totalBreakDuration: string;
    totalFirstHalfDuration: string;
    totalSecondHalfDuration: string;
    isModelLoaded: boolean;
    isOverviewEditable = false;
    isTimingEditable = false;
    isWorkDurationEditable = false;
    isInvalidGraceInTime: boolean = false;
    isInvalidGraceOutTime: boolean = false;
    someSubscription: any;


    constructor(private dialog: MatDialog,
        private route: ActivatedRoute,
        private attendanceService: AttendanceRuleService,
        private baseService: BaseService,
        private router:Router
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
        localStorage.setItem('companyId', '1');
        this.getRuleDetails(this.model.id);

    }


    checkGraceInTime(): void {
        if (this.model.inTime || this.model.graceInTime) {
            const inTimeFormatted = moment(this.model.inTime, 'h:mm A').format('HH:mm:ss.SSSS');
            const graceInTimeFormatted = moment(this.model.graceInTime, 'h:mm A').format('HH:mm:ss.SSSS');
            this.isInvalidGraceInTime = inTimeFormatted > graceInTimeFormatted;
        } else {
            return;
        }
    }

    checkGraceOutTime(): void {
        const outTimeFormatted = moment(this.model.outTime, 'h:mm A').format('HH:mm:ss.SSSS');
        const graceOutTimeFormatted = moment(this.model.graceOutTime, 'h:mm A').format('HH:mm:ss.SSSS');
        this.isInvalidGraceOutTime = outTimeFormatted > graceOutTimeFormatted;
    }

    cancel(): void {
        this.isOverviewEditable = false;
        this.isTimingEditable = false;
        this.isWorkDurationEditable = false;
        this.getRuleDetails(this.model.id);
    }

    getRuleDetails(id: number): void {
        this.blockUI.start();
        this.isModelLoaded = false;

        this.attendanceService.getDetail(this.model.id).subscribe({
            next: (response) => {
                this.model = response;
                this.getTime();
                this.startDay = this.weekDays.find(x => x.key == this.model.startDay).value;
                this.endDay = this.weekDays.find(x => x.key == this.model.endDay).value;
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

    submit(): void {
        this.getInputFormattedTime();
        this.blockUI.start();
        this.isModelLoaded = false;
        this.attendanceService.updateAttendanceRule(this.model).subscribe({
            next: () => {
                this.baseService.successNotification('Attendance rule has been updated successfully.');
                this.getTime();
                this.startDay = this.weekDays.find(x => x.key == this.model.startDay).value;
                this.endDay = this.weekDays.find(x => x.key == this.model.endDay).value;
                this.isOverviewEditable = false;
                this.isTimingEditable = false;
                this.isWorkDurationEditable = false;
                this.blockUI.stop();
                this.isModelLoaded = true;
            },
            error: (error: any) => {
                this.blockUI.stop();
                this.isModelLoaded = true;
                this.getTime();
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
        this.graceInTime = AppUtils.getDifferenceInMinutes(this.model.inTime, this.model.graceInTime);
        this.graceOutTime = AppUtils.getDifferenceInMinutes(this.model.outTime, this.model.graceOutTime);
        this.totalFirstHalfDuration = AppUtils.getDifferenceInHours(this.model.firstHalfStart, this.model.firstHalfEnd);
        this.totalSecondHalfDuration = AppUtils.getDifferenceInHours(this.model.secondHalfStart, this.model.secondHalfEnd);
        this.totalBreakDuration = AppUtils.getDifferenceInHours(this.model.firstHalfEnd, this.model.secondHalfStart);
        this.model.totalBreakDuration = AppUtils.getTime(this.totalBreakDuration);
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
        this.totalBreakDuration = AppUtils.getDifferenceInHours(this.model.firstHalfEnd, this.model.secondHalfStart);
        this.model.totalBreakDuration = AppUtils.getTime(this.totalBreakDuration);
        this.model.totalBreakDuration = AppUtils.getFormattedLocalTime(this.model.totalBreakDuration);

    }

    timing(): void {
        this.isTimingEditable = true;
        this.model.formType = "timing";
    }

    workDuration(): void {
        this.isWorkDurationEditable = true;
        this.model.formType = "workDuration";
    }


    getByYear(year: number){
        // this.isYearSelected = true;
        // this.selectedYear = year;
     this.attendanceService.getbyYear(year).subscribe({
        next:(res) =>
        { this.model1 = res;
            if (this.model1 == null) {
                this.baseService.errorNotification("No record available of selected year.")
              }
            this.router.navigate(['/admin/attendance-rule-detail',this.model1.id]);
        },
        error:(error:any)=>
        {
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