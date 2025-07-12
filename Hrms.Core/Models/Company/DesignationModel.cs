
using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Company
{
    public class DesignationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Peoples { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
