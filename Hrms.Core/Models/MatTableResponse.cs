
namespace Hrms.Core.Models
{
    public class MatTableResponse<T> where T:class
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
