using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Assest
{
    public class AssetAllocationModel
    {
       public int Id {  get; set; }
       public int AssetId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Name { get; set; }
        public Constants.RecordStatus Status { get; set; }


    }
}
