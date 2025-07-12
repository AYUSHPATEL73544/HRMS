
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.JobApplication
{
    public class ApplicantChangeStatusModel
    {
        public int Id { get; set; } 
        public Constants.RecordStatus Status { get; set; }
        public string RejectionReason { get; set; }
    }
}
