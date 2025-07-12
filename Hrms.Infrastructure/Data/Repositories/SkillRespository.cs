using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class SkillRespository: ISkillRepository
    {
        private readonly DataContext _dataContext;
        public SkillRespository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Skill entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSkillListItemsAsync()
        {
            return await (from s in _dataContext.Skills
                          .AsNoTracking().OrderBy(x => x.Name)
                          select new SelectListItemModel
                          {
                              Key = s.Id,
                              Value = s.Name

                          }).ToListAsync();
        }

        public async Task<SelectListItemModel> GetAsync(string skillName)
        {
            return await _dataContext.Skills
                           .Where(x => x.Name.ToLower() == skillName.ToLower())
                           .Select(x => new SelectListItemModel
                           {
                               Key = x.Id,
                               Value = x.Name
                           }).SingleOrDefaultAsync();
        }
    }
}
