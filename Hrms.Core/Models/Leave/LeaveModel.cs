using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Leave
{
    public class LeaveModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int RuleId { get; set; }
        public decimal Total { get; set; }
        public decimal Credited { get; set; }
        public decimal Available { get; set; }
        public decimal Applied { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public Constants.RecordStatus LeaveRuleStatus { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveRule { get; set; }
        public List<LeaveRuleModel> LeaveRules { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
       
        public List<int> EmployeeIds { get; set; }
        public List<int> RuleIds { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime MaxDate { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
