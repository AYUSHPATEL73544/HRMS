namespace Hrms.Core.Entities
{
    public class Employee : EntityBase<int>
    {
        public string Code { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        
        public int ProbationPeriod { get; set; }
        public int BloodGroup { get; set; }
        public int MaritalStatus { get; set; }
        public int EmployeeType { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set;}
        public string Email { get; set; }
        public string AlternatePhone { get; set; }
        public string AlternateEmail { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime? DateOfLeaving { get; set; }
        public DateTime? ExitDate { get; set; }
        public int? ExitType { get; set; }
        public int NoticePeriod { get; set; }
        public string Note { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection <Leave> Leaves { get; set; }
        public virtual ICollection <LeaveLog> LeaveLogs { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Family> Families { get; set; }
        public virtual ICollection<WorkHistory> WorkHistories { get; set; }
        
        
    }
}
