
namespace Hrms.Core.Models.Employee
{
    public class FamilyModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RelationshipId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int EmployeeId { get; set; }
    }
}
