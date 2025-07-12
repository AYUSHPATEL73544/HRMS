using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Employee
{
    public class TeamReportessModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int Type { get; set; }
        public int? ManagerId { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int teamId { get; set; }
    }
}
