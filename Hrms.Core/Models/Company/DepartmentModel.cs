using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Company
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int Peoples { get; set; }
    }
}
