namespace Hrms.Core.Entities
{
    public class Designation : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<WorkHistory> WorkHistories { get; set; }
    }
}
