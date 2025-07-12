using Hrms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Managers
{
    public interface ICityManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int stateId);
    }
}
