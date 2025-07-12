using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Attendance
{
    public class EmployeeAttendanceModel
    {
        public int EmployeeId { get; set; }
        public int RuleId { get; set; }
        public List<int> EmployeeIds { get; set; }
        public List<int> RuleIds { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string EmployeeCode { get; set; }
        public string RuleAssigned { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
