using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities; 

namespace Hrms.Core.Models.Reimbursement
{
    public class ReimbursementModel 
    { 
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Description { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Remark { get; set; } 
        public Constants.RecordStatus Status { get; set; }
        public FileDetailModel DocumentDetails { get; set; }
    }
}
