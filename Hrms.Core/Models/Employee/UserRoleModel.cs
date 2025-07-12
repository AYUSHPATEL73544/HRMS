namespace Hrms.Core.Models.Employee
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }
    }
}
