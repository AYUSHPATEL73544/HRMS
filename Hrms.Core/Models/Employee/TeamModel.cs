using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Employee
{
    public class TeamModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int Type { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
