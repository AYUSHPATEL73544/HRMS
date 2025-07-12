using Hrms.Core.Abstractions;
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
    public class SkillManager : ISkillManager
    {
        private readonly ISkillRepository _skillRepository;

        public SkillManager(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSkillListItemsAsync()
        {
            return await _skillRepository.GetSkillListItemsAsync();
        }

      
    }
}
