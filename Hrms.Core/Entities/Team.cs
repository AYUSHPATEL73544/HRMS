

namespace Hrms.Core.Entities
{
    public class Team : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int Type { get; set; }
        public int? ManagerId { get; set; }
    }
}
