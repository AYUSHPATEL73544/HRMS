import { SelectListItemModel } from "src/app/shared/models";

export class AttendanceRuleModel {
    id: number;
    companyId: number;
    title: string;
    inTime: string;
    outTime: string;
    firstHalfStart: string;
    firstHalfEnd: string;
    firstDayOfWeek: string;
    lastDayOfWeek: string;
    secondHalfStart: string;
    secondHalfEnd: string;
    graceInTime: string;
    graceOutTime: string;
    totalBreakDuration: string;
    numberOfBreak: number;
    minEffectiveDuration: string;
    autoLeaveDeduction: string;
    minAnomaliesForFirstHalfDeducation: string;
    minAnomaliesForFullyDayDeduction: string;
    startDay: number;
    endDay: number;
    description: string;
    formType: string;
    weekStartDay: SelectListItemModel;
    weekLastDay: SelectListItemModel;
    peopeles : number;
    year:number;
    forwardToNextYear = false;
    constructor() {
        this.weekStartDay = new SelectListItemModel();
        this.weekStartDay = new SelectListItemModel();
    }
}
