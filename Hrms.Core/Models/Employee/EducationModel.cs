using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Employee
{
    public class EducationModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public string Stream { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string CollegeName { get; set; }
        public string UniversityName { get; set; }
        public int QualificationTypeId { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
