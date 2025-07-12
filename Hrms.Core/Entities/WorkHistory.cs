
namespace Hrms.Core.Entities
{
    public class WorkHistory : EntityWithStatusTracking<int>
    {
        public int EmployeeId { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}
