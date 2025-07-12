import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import * as moment from 'moment';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { AttendanceLogService } from 'src/app/employee/services/attendance-log.service';
import { BaseService, ListenerService } from 'src/app/shared/services';
import { EmployeeService } from 'src/app/employee/services';
import { AttendanceLogModel } from 'src/app/employee/attendance/model';
import { EmployeeModel } from 'src/app/employee/profile/models';
import { AppUtils } from 'src/app/utilities';

@Component({
  selector: 'app-employee-header',
  templateUrl: './employee-header.component.html',
})

export class EmployeeHeaderComponent implements OnInit {
  @BlockUI('header-blockui') blockUI: NgBlockUI;

  @Output() toggleLayout = new EventEmitter();

  model = new AttendanceLogModel();
  employeeModel = new EmployeeModel();
  timer: string = "";
  minutes = 0;
  hour = 0;
  seconds = 0;

  clockValue: Date;

  isStart: boolean = false;
  timerInterval: any;

  currentDate: any;
  currentTime: any;
  year: any;
  month: any;
  date: any;
  day: any;
  updateProfileSubscription: any;

  constructor(private baseService: BaseService,
    private attendanceLogService: AttendanceLogService,
    private employeeService: EmployeeService,
    private listenerService: ListenerService,
    private appUtils: AppUtils) {
    this.currentDate = moment().local().add('month');
    this.year = this.currentDate.format('YYYY');
    this.month = this.currentDate.format('MMM');
    this.date = this.currentDate.format('DD');
    this.day = this.currentDate.format('ddd');

    this.updateProfileSubscription =
      listenerService.listenProfileUpdate.subscribe(() => {
        this.getEmployee();
      });
  }

  ngOnInit(): void {
    this.currentTime = moment().local().format('HH:mm:ss');
    this.getDetail();
    this.getEmployee();
  }

  toggleSideBar(): void {
    this.toggleLayout.next('');
  }

  getTimer(): void {
    let startTime = moment(this.model.inTime, 'HH:mm:ss');
    let difference = moment(this.currentTime, 'HH:mm:ss').diff(startTime);
    this.seconds = difference / 1000;
    this.minutes = Math.trunc((this.seconds / 60));
    this.hour = Math.trunc(this.minutes / 60);
    if (this.hour > 0) {
      this.minutes = this.minutes % (this.hour * 60);
    }
    this.seconds = this.seconds % 60;
    this.startTimer(startTime);
  }

  startTimer(startTime: moment.Moment): void {
    this.isStart = true;
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
    let elapsedTime = moment.duration(moment().diff(startTime));
    this.hour = elapsedTime.hours();
    this.minutes = elapsedTime.minutes();
    this.seconds = elapsedTime.seconds();

    this.timerInterval = setInterval(() => {
      if (this.minutes > 59) {
        this.hour = this.hour + 1;
        this.minutes = 0;
        this.seconds = 0;
      }
      if (this.seconds > 59) {
        this.minutes = this.minutes + 1;
        this.seconds = 0;
      }

      this.seconds = this.seconds + 1;

      const formattedHour = this.hour.toString().padStart(2, '0');
      const formattedMinutes = this.minutes.toString().padStart(2, '0');
      const formattedSeconds = this.seconds.toString().padStart(2, '0')
      this.timer = `${formattedHour}:${formattedMinutes}:${formattedSeconds}`;
    }, 1000);

    document.addEventListener("visibilitychange", () => {
      if (document.visibilityState === "hidden") {
        clearInterval(this.timerInterval);
      } else if (document.visibilityState === "visible" && this.isStart) {
        let elapsed = moment.duration(moment().diff(startTime));
        this.hour = elapsed.hours();
        this.minutes = elapsed.minutes();
        this.seconds = elapsed.seconds();
        this.startTimer(startTime);
      }
    });
  }

  ngOnDestroy(): void {
    this.updateProfileSubscription.unsubscribe();
  }
  userClockOut(): void {
    this.isStart = false;
    clearInterval(this.timerInterval);
    this.minutes = 0;
    this.hour = 0;
    this.seconds = 0;
    this.attendanceLogService.clockOut(this.model).subscribe({
      next: () => {
        this.baseService.successNotification("Clocked Out Successfully");
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }


  userClockIn(): void {
    this.model.date = this.currentDate;
    this.model.inTime = AppUtils.getFormattedLocalTime(new Date());
    this.attendanceLogService.clockIn(this.model).subscribe({
      next: () => {
        this.baseService.successNotification("Clocked In Successfully");
        this.model.inTime = AppUtils.getFormattedLocalTime(new Date());
        this.startTimer(moment(this.model.inTime, 'HH:mm:ss'));
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getDetail(): void {
    this.blockUI.start();
    this.attendanceLogService.getDetails().subscribe({
      next: (response) => {
        if (response) {
          this.model = response;
          this.model.inTime = this.appUtils.getUtcToLocalTime(this.model.inTime);
          this.getTimer();
          this.isStart = true;
        }
        this.blockUI.stop();

      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseService.processErrorResponse(error);
      }
    });
  }

  getEmployee(): void {
    this.employeeService.getByUserId().subscribe({
      next: (response) => {
        this.employeeModel = response;
      },
      error: (error: any) => {
        this.baseService.processErrorResponse(error);
      }
    });
  }
}
