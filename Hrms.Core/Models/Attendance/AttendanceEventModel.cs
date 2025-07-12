using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Attendance
{
    public class AttendanceEventModel
    {
        public TimeSpan? FirstClockIn { get; set; }
        public TimeSpan? LastClockOut { get; set; }
        public TimeSpan? WorkDuration { get; set; }
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int EndDay { get; set; }
        public int AttendanceRuleId { get; set; }
        public List<AttendanceLogModel> Logs { get; set; }
    }
}
