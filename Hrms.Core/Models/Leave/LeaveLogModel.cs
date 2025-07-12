using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Leave
{
    public class LeaveLogModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int RuleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Constants.Half StartHalf { get; set; }
        public Constants.Half EndHalf { get; set; }
        public Constants.RecordStatus EmployeeStatus { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string Purpose { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LeaveType { get; set; }
        public decimal Days { get;set; }
        public string RejectionReason { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime MinDate { get; set; }

    }
}
