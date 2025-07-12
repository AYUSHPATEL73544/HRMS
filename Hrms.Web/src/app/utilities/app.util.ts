import jwt_decode from 'jwt-decode';
import * as moment from 'moment';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { Constants } from './app.constants';
import { CurrentUserModel } from 'src/app/shared/models/current-user.model';
import { SelectListItemModel } from '../shared/models';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class AppUtils {
    constructor(private router: Router) { }

    public emailRegexPattern = '[a-zA-Z0-9.-_]{1,}@[a-zA-Z0-9.-]{2,}[.]{1}[a-zA-Z]{2,4}';
    public urlRegexPattern = '^(http|https):\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}';
    public phoneMask = '000-000-0000';
    public WarrantyPeriodMask = '00';

    public processErrorResponse(snackBar: MatSnackBar, snackBarConfig: MatSnackBarConfig, response: any, customErrorMessage = ''): void {
        const error = response.error;

        if (response.status === 400) {
            if (error instanceof Array) {
                error.forEach((item) => {
                    snackBar.open(item, 'X', snackBarConfig);
                });
            } else {
                snackBar.open(error, 'X', snackBarConfig);
            }
        } else if (response.status === 401) {
            snackBar.open('Please login to perform this action.', 'X', snackBarConfig);
            this.router.navigate(['/account/logout']);
        } else if (response.status === 403) {
            snackBar.open('You are not allowed to perform this operation.', 'X', snackBarConfig);
        } else if (response.status === 404) {
            snackBar.open('Something went wrong. Please try refreshing the page.', 'X', snackBarConfig);
        } else if (response.status === 0) {
            snackBar.open('Unable to connect with API Gateway.', 'X', snackBarConfig);
        } else {
            if (customErrorMessage === '') {
                snackBar.open('Internal server error.', 'X', snackBarConfig);
            } else {
                snackBar.open(customErrorMessage, 'X', snackBarConfig);
            }
        }
    }

    public static isNullOrZero(input: number): boolean {
        return input === null || input === undefined || input.toString() === '0';
    }

    public static isNullOrWhiteSpace(input: any): boolean {
        return input === null || input === undefined || input.toString() === '';
    }

    public static getFormattedDate(date: string, format: string): string {
        if (!format) {
            format = 'MM/DD/YYYY HH:mm:ss';
        }
        return date ? moment(date).format(format) : '';
    }

    public static getFormattedLocalTime(inputTime): string {
        return moment(inputTime, "HH:mm a").format('HH:mm:ss');
    }

    public static getDate(date: string): any {
        return (date ? moment(date) : moment()).format('YYYY-MM-DDTHH:mm:ss');
    }

    public static getLocalDate(date: any): any {
        return (date ? moment(date) : moment()).format('YYYY-MM-DD');
    }
    
    public static getLocalFormattedDate(date: string): any {
        const momentDate = moment.utc(date).local();
        const formattedDate = momentDate.format("MM/DD/YYYY");
        return formattedDate;
    }

    public static getDateFromString(date: string): any {
        return date ? moment(date).toDate() : null;
    }

    public static getTime(time: string): any {
        return time ? moment(time, 'HH:mm:ss.SSSS').format('h:mm A') : null;
    }

    public static getDifferenceInMinutes(startTime: string, endTime: string): any {
        const start = moment(startTime, 'hh:mm a');
        const end = moment(endTime, 'hh:mm a');
        const diff = end.diff(start, 'minutes');
        return diff;
    }

    public static getDifferenceInHours(startTime: string, endTime: string): any {
        const start = moment(startTime, 'HH:mm:ss A');
        const end = moment(endTime, 'HH:mm:ss A');
        const hours = end.diff(start, 'hours');
        const minutes = end.diff(start, 'minutes') % 60;
        if (hours == 0 && minutes == 0) {
            const time = '0 m';
            return time;
        }
        else if (hours == 0) {
            return minutes + " " + 'm';
        }
        const time = hours + " " + 'h and ' + minutes + " " + 'm';

        return time;
    }

    public static getCurrentDate(): Date {
        return new Date();
    }

    public static getDifferenceInYear(startDate: any, endDate: any): any {
        const start = moment(startDate, 'MM/DD/yyyy');
        const end = moment(endDate, 'MM/DD/yyyy');
        const year = end.diff(start, 'years');
        const month = end.diff(start, 'months') % 12;
        if (year > 1 && month > 1) {
            return year + " Years and " + month + " Months";
        }
        else if (year < 1 && month > 1) {
            return month + " Months";
        }
        else if (year < 1 && month <= 1) {
            return month + " Month";
        }
        else {
            return year + " Year and " + month + " Months";
        }
    }


    public static getFormatedTimeDifference(time: string): any {
        const duration = moment.duration(time);
        const hours = duration.hours();
        const minutes = duration.minutes();
        if (hours == 0 && minutes == 0) {
            const formattedTime = '-'
            return formattedTime;
        }
        else if (hours == 0) {
            const formattedTime = minutes + " " + 'm';
            return formattedTime;
        }
        const formattedTime = hours + " " + 'h and ' + minutes + " " + 'm';
        return formattedTime;
    }


    public static getDifferenceInDays(startDay: any, endDay: any): any {
        const start = moment(startDay, 'MM/DD/yyyy');
        const end = moment(endDay, 'MM/DD/yyyy');
        const diff = end.diff(start, 'days');
        return diff;
    }

    public static isUserAuthenticated(): boolean {
        const authToken = localStorage.getItem(Constants.varAuthToken);
        return authToken !== null && authToken !== typeof undefined && authToken !== '';
    }

    public static getUserRole(): string {
        const authToken = localStorage.getItem(Constants.varAuthToken);
        if (!authToken) {
            return '';
        }
        const decodedToken: any = jwt_decode(authToken);
        return decodedToken.role.toString();
    }

    public static getUserId(): string {
        const authToken = localStorage.getItem(Constants.varAuthToken);
        if (!authToken) {
            return '';
        }
        const decodedToken: any = jwt_decode(authToken);
        return decodedToken.nameid.toString();
    }

    public static getCurrentUserProfile(): CurrentUserModel {
        const model = new CurrentUserModel();
        const authToken: any = localStorage.getItem(Constants.varAuthToken);
        if (!authToken) {
            return model;
        }

        const decodedToken: any = jwt_decode(authToken);

        model.id = decodedToken.nameid.toString();
        model.firstName = decodedToken.given_name.toString();
        model.role = decodedToken.role.toString();

        return model;
    }

    public static isNullOrUndefined(input: any): boolean {
        return input === null || input === undefined;
    }

    public static isNullOrWhitespace(input: string): boolean {
        return input === null || input === undefined || input === '';
    }

    public static isNumeric(s): boolean {
        return !isNaN(Number(s));
    }

    public static getWeekDaysForDropDown(): any {
        return [
            { key: 1, value: 'Monday' },
            { key: 2, value: 'Tuesday' },
            { key: 3, value: 'Wednesday' },
            { key: 4, value: 'Thursday' },
            { key: 5, value: 'Friday' },
            { key: 6, value: 'Saturday' },
            { key: 7, value: 'Sunday' }
        ];
    }

    public static getGenderForDropDown(): any {
        return [
            { key: 1, value: 'Male' },
            { key: 2, value: 'Female' }
        ];
    }

    public static getYears(): Array<SelectListItemModel> {
        const start = 2020;
        const end = moment().year() + 1;
        return Array(end - start + 1)
            .fill(start)
            .map((year, index) => {
                const item = new SelectListItemModel();
                item.key = year + index;
                item.value = year + index;
                return item;
            }).reverse();
    }

    public static getMaritalStatusForDropDown(): any {
        return [
            { key: 1, value: 'Married' },
            { key: 2, value: 'Unmarried' }
        ];
    }

    public static getEmployeeTypeDropDown(): any {
        return [
            { key: 1, value: 'Primary' },
            { key: 2, value: 'Secondary' }
        ];
    }

    public static getBloodGroupForDropDown(): any {
        return [
            { key: 1, value: 'A+' },
            { key: 2, value: 'A-' },
            { key: 3, value: 'B+' },
            { key: 4, value: 'B-' },
            { key: 5, value: 'O+' },
            { key: 6, value: 'O-' },
            { key: 7, value: 'AB+' },
            { key: 8, value: 'AB-' }
        ];
    }

    public static getMonthsForDropDown(): any {
        return [
            { key: 1, value: 'January' },
            { key: 2, value: 'February' },
            { key: 3, value: 'March' },
            { key: 4, value: 'April' },
            { key: 5, value: 'May' },
            { key: 6, value: 'June' },
            { key: 7, value: 'July' },
            { key: 8, value: 'August' },
            { key: 9, value: 'September' },
            { key: 10, value: 'October' },
            { key: 11, value: 'November' },
            { key: 12, value: 'December' }
        ]
    }

    public static QualificationTypes(): any {
        return [
            { key: 1, value: 'Certification' },
            { key: 2, value: 'Diploma' },
            { key: 3, value: 'Doctorate' },
            { key: 4, value: 'Graduation' },
            { key: 5, value: 'Other Education' },
            { key: 6, value: 'Post Graduation' },
            { key: 7, value: 'Pre University' }
        ]
    }

    public static AccrualFrequency(): any {
        return [
            { key: 1, value: 'Yearly' },
            { key: 2, value: 'HalfYearly' },
            { key: 3, value: 'Quarterly' },
            { key: 4, value: 'Monthly' }
        ]
    }

    public static AccrualPeriod(): any {
        return [
            { key: 1, value: 'Start' },
            { key: 2, value: 'End' }
        ]
    }

    public static statusDropDown(): any {
        return [
            { key: 2, value: 'Active' },
            { key: 3, value: 'Inactive' }
        ];
    }


    public static employeeType(): any {
        return [
            { key: 1, value: 'Full Time' },
            { key: 2, value: 'Part Time' },
            { key: 3, value: 'Intern' },
            { key: 4, value: 'On Contract' },
        ];
    }

    public static role(): any {
        return [
            { key: 2, value: 'Employee' },
            { key: 3, value: 'HR Manager' },
            { key: 4, value: 'HR Executive' },
            { key: 5, value: 'Repoting Manager' },
            { key: 6, value: 'Interviewer' },
        ];
    }

    public static exitType(): any {
        return [
            { key: 1, value: 'Termination' },
            { key: 2, value: 'Resignation' }
        ];
    }


    public static probationPeriodDropDown(): any {
        return [
            { key: 0, value: '0' },
            { key: 15, value: '15' },
            { key: 30, value: '30' },
            { key: 45, value: '45' },
            { key: 60, value: '60' },
            { key: 75, value: '75' },
            { key: 90, value: '90' },
            { key: 105, value: '105' },
            { key: 120, value: '120' },
            { key: 135, value: '135' },
            { key: 150, value: '150' },
            { key: 165, value: '165' },
            { key: 180, value: '180' },
        ]
    }

    public static getInterviewModes(): any{
        return [
            { key: 1, value: 'Online' },
            { key: 2, value: 'Offline'}
        ];
    }
    public static getInterviewTypes(): any{
        return [
            { key: 1, value: 'Technical' },
            { key: 2, value: 'HR'}
        ];
    }

    public static getYearsForDropDownByRange(range: number): any {
        const currentDate = new Date();
        const years = [currentDate.getFullYear()];
        for (let i = 1; i <= range; i++) {
            years.push(currentDate.getFullYear() - i);
        }
        return years;

    }

    public getUtcToLocalTime(time: string) {
        return moment.utc(time, 'HH:mm:ss').local().format('HH:mm:ss');
    }

    public getLocalToUtcTime(time: string) {
        return moment(time, 'HH:mm:ss').utc().format('HH:mm:ss');
    }

    public getCompanyId(): number {
        const authToken: any = localStorage.getItem(Constants.varAuthToken);
        if (!authToken) {
            return null;
        }

        const decodedToken: any = jwt_decode(authToken);

        const companyId = decodedToken.company_id;

        if (!companyId) {
            this.router.navigate(['/account/logout']);
        }
        return <number>companyId;
    }
}

