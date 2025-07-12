using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Entities
{
    public class Education : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int QualificationTypeId { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public string Stream { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
    }
}
