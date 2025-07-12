namespace Hrms.Core.Models.Reimbursement
{
    public class ReimbursementFilterModel: MatDataTableRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
