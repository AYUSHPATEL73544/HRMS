
namespace Hrms.Core.Models.Attendance
{
    public class AttedanceFilterModel: MatDataTableRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? InTime { get; set; }
    }
}
