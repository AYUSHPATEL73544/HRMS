using Hrms.Core.Utilities;

namespace Hrms.Core.Models.JobApplication
{
    public class FileDetailModel
    {
        public int Id { get; set; }
        public int IdentificationId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public Constants.DocumentType DocumentType { get; set; }
        public bool Uploaded { get; set; }
    }
}
