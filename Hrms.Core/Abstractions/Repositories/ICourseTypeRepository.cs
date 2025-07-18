﻿using Hrms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface ICourseTypeRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
