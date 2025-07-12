
namespace Hrms.Core.Models
{
    public class MatDataTableRequest
    {
        public string FilterKey { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string SortExpression()
        {
            return $"{Sort} {Order}";
        }

        public int RecordsToSkip()
        {
            if (PageSize == 0)
            {
                PageSize = 10;
            }

            return PageIndex * PageSize;
        }
    }
}
