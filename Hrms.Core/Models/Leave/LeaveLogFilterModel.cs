namespace Hrms.Core.Models.Leave
{
    public class LeaveLogFilterModel: MatDataTableRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}

    }
}
