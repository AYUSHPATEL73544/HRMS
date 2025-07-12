import { HolidayModel } from "./holiday.model";

export class HolidayGroupModel {
    year : number;
    holidays: Array <HolidayModel>;
    forwardToNextYear : boolean = false;
    
    constructor(){
        this.holidays = new Array<HolidayModel>();
        this.year = new Date().getFullYear();
    }
}