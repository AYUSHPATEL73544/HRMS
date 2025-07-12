import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import { CompanyEventModel } from './model/company-event.model';
import { BaseService, CalendarService } from 'src/app/shared/services';
import { AppUtils } from 'src/app/utilities';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HolidayModel } from 'src/app/admin/holidays/models';

@Component({
  selector: 'app-event-calendar',
  templateUrl: './calender-event.component.html',
})
export class CalendarEventComponent implements AfterViewInit {
  @BlockUI('calendar-blockUi') blockUI: NgBlockUI;
  @ViewChild('calendar') calendarComponent: FullCalendarComponent;
  calendarApi: any;
  nextYearFormattedDate: any;
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin],
    initialView: 'dayGridMonth',
  };

  model = new Array<CompanyEventModel>();
  holidayModel = new Array<HolidayModel>();

  constructor(
    private service: CalendarService,
    private baseServcice: BaseService,
  ) { }

  ngAfterViewInit(): void {
    this.calendarOptions.plugins = [dayGridPlugin, timeGridPlugin];
    this.calendarOptions.displayEventTime = false;
    this.calendarOptions.eventDisplay = 'block';
    this.calendarOptions.height = 'auto';
    this.calendarOptions.customButtons = {
      next: {
        click: this.nextMonth.bind(this),
      },
      prev: {
        click: this.prevMonth.bind(this),
      },
      today: {
        text: 'Today',
        click: this.currentMonth.bind(this),
      },
    };
    this.calendarApi = this.calendarComponent.getApi();
    let currentDate = this.calendarApi.getDate();
    this.getCalendarEvent(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1
    );
  }

  nextMonth(): void {
    this.calendarApi.next();
    const currentDate = this.calendarApi.view.currentStart;
    this.getCalendarEvent(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1
    );
  }

  currentMonth(): void {
    this.calendarApi.today();
    let currentDate = this.calendarApi.getDate();
    this.getCalendarEvent(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1
    );
  }

  prevMonth(): void {
    this.calendarApi.prev();
    const currentDate = this.calendarApi.view.currentStart;
    this.getCalendarEvent(
      currentDate.getFullYear(),
      currentDate.getMonth() + 1
    );
  }

  getCalendarEvent(year: number, month: number) {
    this.blockUI.start();
    this.service.getCalendarEvent(year, month).subscribe({
      next: (response) => {
        this.model = response;
        this.blockUI.stop();
        this.getHolidayEvent(year, month);
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseServcice.processErrorResponse(error);
      },
    });
  }

  getHolidayEvent(year: number, month: number) {
    this.blockUI.start();
    this.service.getHolidayEvent(year, month).subscribe({
      next: (response) => {
        this.holidayModel = response;
        this.blockUI.stop();
        this.initializeEvents();
      },
      error: (error: any) => {
        this.blockUI.stop();
        this.baseServcice.processErrorResponse(error);
      },
    });
  }

  initializeEvents() {
    const currentYear = new Date().getFullYear();
    let events = new Array<any>();
    const uniqueLeaveLogs = new Map<string, string>(); // Track unique leave logs
    const uniqueBirthdays = new Set<string>(); // Track unique birthdays
    this.model.forEach((event) => {
      if (event.dateOfBirth != null) {
        const birthdayDate = new Date(event.dateOfBirth);
        const formattedDate =
          currentYear +
          '-' +
          (birthdayDate.getMonth() + 1) +
          '-' +
          birthdayDate.getDate();
        event.dateOfBirth = AppUtils.getDate(formattedDate);

        const nextYearBirthdayDate = new Date(event.dateOfBirth);
        nextYearBirthdayDate.setFullYear(currentYear + 1);
        this.nextYearFormattedDate =
          nextYearBirthdayDate.getFullYear() +
          '-' +
          (nextYearBirthdayDate.getMonth() + 1) +
          '-' +
          nextYearBirthdayDate.getDate();
      }
      const leaveLog = `${event.title}-${event.start}-${event.end}`;
      if (!uniqueLeaveLogs.has(leaveLog)) {
        uniqueLeaveLogs.set(leaveLog, event.title);
        if (event.status == 7) {
          if (event.isHalfDay) {
          events.push({
            title: event.title,
            start: event.start,
            end: event.end,
            className: 'fc-event-halfwidth',
            color: '#f44336',
            textColor: '#ffffff',
          });
          }else{
            events.push({
              title: event.title,
              start: event.start,
              end: event.end,
              className: 'leave-event',
              color: '#f44336',
              textColor: '#ffffff',
            });
          }
        }
        if (event.status == 6) { 
          if (event.isHalfDay) {
            events.push({
              title: event.title,
              start: event.start,
              end: event.end,
              className: 'fc-event-halfwidth',
              color: '#7A7777',
              textColor: '#ffffff',
              dayGridPlugin: 'half',
            });
          }
          else{
            events.push({
              title: event.title,
              start: event.start,
              end: event.end,
              className: 'pending-leave-event',
              color: '#7A7777',
              textColor: '#ffffff',
              dayGridPlugin: 'half',
            });
          }
        }
      }

      if (!uniqueBirthdays.has(event.title) && event.dateOfBirth != null) {
        uniqueBirthdays.add(event.title);
        events.push({
          title: event.title,
          start: event.dateOfBirth,
          className: 'birthday-event',
          color: '#17B169',
          textColor: '#ffffff',
        });
        events.push({
          title: event.title,
          start: AppUtils.getDate(this.nextYearFormattedDate),
          className: 'birthday-event',
          color: '#17B169',
          textColor: '#ffffff',
        });
      }
    });

    events.forEach((event) => {
      if (event.className === 'leave-event') {
        event.end = new Date(event.end).setHours(23, 59, 59, 999);
      }
      if (event.className === 'pending-leave-event') {
        event.end = new Date(event.end).setHours(23, 59, 59, 999);
      }
    });

    this.holidayModel.forEach((event) => {
      events.push({
        title: event.name,
        start: event.date,
        className: 'holidayEvent',
        color: '#FFA500',
        textColor: '#ffffff',
      });
    });

    this.calendarOptions.events = events;
  }
}
