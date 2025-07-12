export class LeaveRuleModel {
  id: number;
  companyId: number;
  title: string;
  description: string;
  maxAllowedInYear: number;
  maxAllowedInMonth: number;
  maxAllowedContinues: number;
  maxBackDatedLeavesAllowed: number;
  countWeekendAsLeave: boolean;
  countHolidayAsLeave: boolean;
  creditableOnAccrualBasis: boolean;
  accrualFrequency: number;
  accrualPeriod: number;
  allowedBackDatedLeaves: boolean;
  allowedUnderProbation: boolean;
  allowedNegative: boolean;
  allowedCarryForward: boolean;
  allowedDonation: boolean;
  futureDatedLeavesAllowed: boolean;
  futureDatedLeavesAllowedUpTo: number;
  applyTillNextYear: number;
  people: number;
  createdOn: string;
}