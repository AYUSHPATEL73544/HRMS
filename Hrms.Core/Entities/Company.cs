namespace Hrms.Core.Entities
{
    public class Company : EntityBase<int>
    {
        public int UserId { get; set; }
        public string RegisteredName { get; set; }
        public string BrandName { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        
        public virtual ICollection<AttendanceRule> AttendanceRules { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<LeaveRule> LeaveRules { get; set; }

    }
}
