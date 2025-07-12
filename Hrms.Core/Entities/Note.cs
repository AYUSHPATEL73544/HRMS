using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Entities
{
    public class Note : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public string Description { get; set; }
    }
}
