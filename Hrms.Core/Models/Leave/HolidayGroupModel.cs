

namespace Hrms.Core.Models.Leave
{
    public class HolidayGroupModel
    {
        public int Year { get; set; }
        public List<HolidayModel> Holidays { get; set; }
        public bool ForwardToNextYear { get; set; }
    }
}
