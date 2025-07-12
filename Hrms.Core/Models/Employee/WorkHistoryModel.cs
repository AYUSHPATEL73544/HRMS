using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Employee
{
    public class WorkHistoryModel
    {
        public int EmployeeId { get;set; }
        public int Id { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
