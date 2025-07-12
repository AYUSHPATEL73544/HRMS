using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Attendance
{
    public class AttedanceLogChangeStatusModel
    {
        public int Id { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
