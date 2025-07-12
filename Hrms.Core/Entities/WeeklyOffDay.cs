namespace Hrms.Core.Entities
{
    public class WeeklyOffDay
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
