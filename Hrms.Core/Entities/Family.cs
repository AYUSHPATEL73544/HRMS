
namespace Hrms.Core.Entities
{
    public class Family : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RelationshipId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public virtual Relationship Relationship { get; set; }

    }
}
