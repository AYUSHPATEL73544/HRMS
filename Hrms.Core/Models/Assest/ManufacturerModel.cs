using Hrms.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Models.Assest
{
    public class ManufacturerModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
