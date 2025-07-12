using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Reimbursement
{
    public class ReimbursementChangeStatusModel
    {
        public int Id { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string Remark { get; set; }
        public DateTime PaymentDate { get; set; }
    }
} 