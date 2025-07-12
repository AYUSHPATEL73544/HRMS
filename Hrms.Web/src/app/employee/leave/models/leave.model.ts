export class LeaveModel {
  id: number;
  employeeId: number;
  ruleId: number;
  total: number;
  credited: number;
  available: number;
  applied: number;
  startDate: string;
  endDate: string;
  startHalf: number;
  endHalf: number;
  purpose: string;
  ruleName: string;
  maxDate: string;
  minDate: string;
}