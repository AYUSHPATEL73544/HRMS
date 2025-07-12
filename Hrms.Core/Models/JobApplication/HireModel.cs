

namespace Hrms.Core.Models.JobApplication
{
    public class HireModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
