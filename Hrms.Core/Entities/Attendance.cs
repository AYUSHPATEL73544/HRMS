namespace Hrms.Core.Entities
{
    public class Attendance : EntityWithStatusTracking<int>
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; }

        public virtual ICollection<AttendanceLog> AttendanceLogs { get;set; }
    }
}
