using Hrms.Core.Utilities;
namespace Hrms.Core.Entities
{
    public class Document:EntityBase<int>
    {
        public string Name { get; set; }
        public int IdentificationId { get; set; }
        public string Key { get; set; }
        public Constants.DocumentType DocumentType { get; set; }
       
    }
}
