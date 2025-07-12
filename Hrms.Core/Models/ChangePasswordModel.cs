using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }

        public string Password { get; set; }
        public int EmployeeId { get; set; }

    }
}
