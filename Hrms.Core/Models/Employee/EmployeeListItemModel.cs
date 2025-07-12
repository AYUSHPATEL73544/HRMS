
using Hrms.Core.Models.Company;
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Employee
{
    public class EmployeeListItemModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodGroup { get; set; }
        public int MaritalStatus { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AlternatePhone { get; set; }
        public string AlternateEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfLeaving { get; set; }

        public Constants.RecordStatus Status { get; set; }

        public AddressModel PermanentAddress { get; set; }
        public AddressModel CurrentAddress { get; set; }
        public DepartmentModel DepartmentName { get; set; }
        public DesignationModel DesignationName { get; set; }
    }
}
