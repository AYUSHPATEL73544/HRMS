using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Managers
{
    public class QualificationTypeManager: IQualificationTypeManager
    {
        private readonly IQualificationTypeRepository _qualificationTypeRepository;
        public QualificationTypeManager(IQualificationTypeRepository qualificationTypeRepository)
        {
            _qualificationTypeRepository = qualificationTypeRepository;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _qualificationTypeRepository.GetSelectListItemsAsync();
        }
    }
}
