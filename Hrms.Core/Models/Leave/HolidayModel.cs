
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Leave
{
    public class HolidayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
