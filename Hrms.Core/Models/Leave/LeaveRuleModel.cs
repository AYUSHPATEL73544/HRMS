
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Leave
{
    public class LeaveRuleModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxAllowedInYear { get; set; }
        public int MaxAllowedInMonth { get; set; }
        public int MaxAllowedContinues { get; set; }

        public bool CountWeekendAsLeave { get; set; }
        public bool CountHolidayAsLeave { get; set; }

        public bool CreditableOnAccrualBasis { get; set; }
        public int AccrualFrequency { get; set; }
        public int AccrualPeriod { get; set; }

        public bool AllowedBackDatedLeaves { get; set; }
        public int MaxBackDatedLeavesAllowed { get; set; }
        public bool AllowedUnderProbation { get; set; }
        public bool AllowedNegative { get; set; }
        public bool AllowedCarryForward { get; set; }
        public bool AllowedDonation { get; set; }
        public int ApplyTillNextYear { get; set; }

        public int FutureDatedLeavesAllowedUpTo { get; set; }
        public bool FutureDatedLeavesAllowed { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public Constants.RecordStatus LeaveRuleStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public int People { get; set; }

    }
}
