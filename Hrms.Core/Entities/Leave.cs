namespace Hrms.Core.Entities
{
    public class Leave : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int RuleId { get; set; }
        public decimal Total { get; set; }
        public decimal Credited { get; set; }
        public decimal Available { get; set; }
        public decimal Applied { get; set; }
    }
}
