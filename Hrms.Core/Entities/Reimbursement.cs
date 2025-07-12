namespace Hrms.Core.Entities
{
    public class Reimbursement: EntityBase<int>
    { 
        public int EmployeeId { get; set; } 
        public string Description { get; set; }
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Remark { get; set; }

        public Employee Employee { get; set; }
    }
}
