namespace Hrms.Core.Entities
{
    public class EmployeeAttendanceRule : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int AttendanceRuleId { get; set; }
    }
}
