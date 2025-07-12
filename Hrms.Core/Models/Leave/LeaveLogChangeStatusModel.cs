using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Leave
{
    public class LeaveLogChangeStatusModel
    {
        public int Id { get; set; }
        public string RejectionReason { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
