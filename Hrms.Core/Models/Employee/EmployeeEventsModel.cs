using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Employee
{
    public class CompanyEventsModel
    {
        public string Title { get; set; }    
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Constants.RecordStatus? Status { get; set; }
        public bool IsHalfDay { get; set; }
    }
}
