using Hrms.Core.Utilities;

namespace Hrms.Core.Entities
{
    public class LeaveLog : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int RuleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Constants.Half StartHalf { get; set; }
        public Constants.Half EndHalf { get; set; }
        public string Purpose { get; set; }
        public decimal Days { get; set; }
        public string RejectionReason { get; set; }
    }
}
