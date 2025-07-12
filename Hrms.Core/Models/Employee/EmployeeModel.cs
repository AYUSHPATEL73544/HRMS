using Hrms.Core.Models.Company;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Employee
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public int BloodGroup { get; set; }
        public int MaritalStatus { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AlternatePhone { get; set; }
        public string AlternateEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfLeaving { get; set; }
        public DateTime? StartedFrom { get; set; }
        public Constants.RecordStatus Status { get; set; }

        public AddressModel PermanentAddress { get; set; }
        public AddressModel CurrentAddress { get; set; }
        public string  Department { get; set; }
        public string Designation { get; set; }

        public int ProbationPeriod { get; set; }
        public int EmployeeType { get; set; }
        public int RoleId { get; set; }
        public DateTime? ExitDate { get; set; }
        public int? ExitType { get; set; }
        public int NoticePeriod { get; set; }
        public string Note { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public FileDetailModel ImageDetails { get; set; }

    }
}
