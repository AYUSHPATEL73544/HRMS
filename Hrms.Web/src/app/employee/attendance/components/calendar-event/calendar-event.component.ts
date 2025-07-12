import { Component, Input, SimpleChanges, ViewChild } from "@angular/core";
import { FullCalendarComponent } from "@fullcalendar/angular";
import { CalendarOptions } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import * as moment from "moment";
import { BlockUI, NgBlockUI } from "ng-block-ui";
import { HolidayModel } from "src/app/admin/holidays/models";
import { CalendarService } from "src/app/employee/services";
import { CompanyEventModel } from "src/app/shared/components/calender-event/model";
import { AttendanceEventModel } from "src/app/shared/models";
import { BaseService } from "src/app/shared/services";
import { AppUtils, Constants } from "src/app/utilities";

@Component({
    selector: 'app-calendar-event',
    templateUrl: './calendar-event.component.html'
})

export class CalendarEventComponent {
    @BlockUI('calendar-blockUi') blockUI: NgBlockUI;
    @ViewChild('calendar') calendarComponent: FullCalendarComponent;

    @Input() selectedYear: any;
    @Input() selectedMonth: any;

    model = new Array<AttendanceEventModel>();
    absentModel = new Array<AttendanceEventModel>();
    holidayModel = new Array<HolidayModel>();
    leaveModel = new Array<CompanyEventModel>();

    calendarApi: any;
    calendarOptions: CalendarOptions = {
        plugins: [dayGridPlugin],
        initialView: 'dayGridMonth',
    }

    constructor(private baseService: BaseService,
        private service: CalendarService,
        private appUtiils: AppUtils
    ) {
    }

    ngAfterViewInit(): void {
        this.calendarOptions.plugins = [dayGridPlugin];
        this.calendarOptions.displayEventTime = false;
        this.calendarOptions.eventDisplay = 'block';
        this.calendarOptions.height = 'auto';
        this.calendarOptions.customButtons = {
            next: {
                click: this.nextMonth.bind(this),
            },
            prev: {
                click: this.prevMonth.bind(this)
            },
            today: {
                text: 'Today',
                click: this.currentMonth.bind(this),
            }
        }

        this.calendarApi = this.calendarComponent.getApi();
        let currentDate = this.calendarApi.getDate();
        this.getCalendarEvent(
            currentDate.getFullYear(),
            currentDate.getMonth() + 1
        );
    }

    ngOnChanges(changes: SimpleChanges) {
        this.getBySelectedYearOrMonth();
    }

    getBySelectedYearOrMonth(): void {
        this.calendarApi = this.calendarComponent.getApi();
        let selectedDate = new Date(`${this.selectedYear}-${this.selectedMonth}-01`);
        this.calendarApi.gotoDate(AppUtils.getLocalDate(selectedDate));
        this.getCalendarEvent(this.selectedYear, this.selectedMonth);
    }

    nextMonth(): void {
        this.calendarApi.next();
        const currentDate = this.calendarApi.view.currentStart;
    }

    currentMonth(): void {
        this.calendarApi.today();
        let currentDate = this.calendarApi.getDate();
    }

    prevMonth(): void {
        this.calendarApi.prev();
        const currentDate = this.calendarApi.view.currentStart;
    }

    getCalendarEvent(year: number, month: number) {
        this.blockUI.start();
        this.service.getEmployeeAtteandanceLog(year, month).subscribe({
            next: (response) => {
                this.model = response;
                this.getHolidayEvent(year, month);
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        })
    }

    getHolidayEvent(year: number, month: number): void {
        this.blockUI.start();
        this.service.getHolidayEvent(year, month).subscribe({
            next: (response) => {
                this.holidayModel = response;
                this.getAbsentEvent(year, month);
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getAbsentEvent(year: number, month: number) {
        this.blockUI.start();
        this.service.getEmployeeAbsentEvents(year, month).subscribe({
            next: (response) => {
                this.absentModel = response;
                this.getLeaveLogEvents(year, month);
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }

    getLeaveLogEvents(year: number, month: number) {
        this.blockUI.start();
        this.service.getEmployeeLeaveCalendarEvent(year, month).subscribe({
            next: (response) => {
                this.leaveModel = response;
                this.initializeEvents();
                this.blockUI.stop();
            },
            error: (error) => {
                this.blockUI.stop();
                this.baseService.processErrorResponse(error);
            }
        });
    }


    initializeEvents() {
        var events = new Array<any>();
        this.model.forEach((event) => {
            event.firstClockIn = this.appUtiils.getUtcToLocalTime(event.firstClockIn);
            event.firstClockIn = AppUtils.getTime(event.firstClockIn);
            event.lastClockOut = this.appUtiils.getUtcToLocalTime(event.lastClockOut);
            event.lastClockOut = AppUtils.getTime(event.lastClockOut);
            event.workDuration = this.formatDuration(event.workDuration);
            if (event.workDuration != "NaN h and NaN m") {
                events.push({
                    title: event.workDuration,
                    start: event.date,
                    className: 'fc-event-fullwidth',
                    color: '#28a745',
                    textColor: '#ffffff'
                });
            }
            if (event.firstClockIn != "Invalid date") {
                events.push({
                    title: event.firstClockIn,
                    start: event.date,
                    className: 'fc-event-fullwidth',
                    color: '#007bff',
                    textColor: '#ffffff'
                });
            }
            if (event.lastClockOut != "Invalid date") {
                events.push({
                    title: event.lastClockOut,
                    start: event.date,
                    className: 'fc-event-fullwidth',
                    color: '#ffa500',
                    textColor: '#ffffff'
                });
            }
        });

        this.holidayModel.forEach((event) => {
            events.push({
                title: event.name,
                start: event.date,
                className: 'fc-event-fullwidth',
                color: '#FC6736',
                textColor: '#ffffff'
            })
        });

        this.absentModel.forEach((event) => {
            this.model.forEach(x => {
                if (event.date != null && x.attendanceRuleId > 0) {
                    events.push({
                        title: 'Absent',
                        start: event.date,
                        className: 'fc-event-fullwidth',
                        color: '#7A7777',
                        textColor: '#fffff'
                    });
                }
            })
           
        });

        this.leaveModel.forEach((event) => {
            if (event.status == 6) {
                if (event.isHalfDay) {
                    events.push({
                        title: 'Half Day Pending Leave',
                        start: event.start,
                        end: event.end,
                        className: 'fc-event-halfwidth',
                        color: '#7A7777',
                        textColor: '#ffffff',
                        dayGridPlugin: 'half',
                    });
                } else {
                    events.push({
                        title: 'Pending Leave',
                        start: event.start,
                        end: event.end,
                        className: 'pending-leave-event',
                        color: '#7A7777',
                        textColor: '#ffffff',

                    });
                }
            }
            if (event.status == 7) {
                if (event.isHalfDay) {
                    events.push({
                        title: 'Half Day Leave',
                        start: event.start,
                        end: event.end,
                        className: 'fc-event-halfwidth',
                        color: '#f44336',
                        textColor: '#ffffff',

                    });
                } else {
                    events.push({
                        title: 'Leave',
                        start: event.start,
                        end: event.end,
                        className: 'leave-event',
                        color: '#f44336',
                        textColor: '#ffffff',
                    });
                }
            }
        });

        this.calendarComponent.events = events;
    }


    formatDuration(durationString: string): string {
        const duration = moment.duration(durationString);
        const hours = Math.floor(duration.asHours());
        const minutes = duration.minutes();
        if (hours > 0) {
            return `${hours} hrs ${minutes} mins `;
        }
        return `${minutes} mins`;
    }
}