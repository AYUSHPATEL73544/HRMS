using Hrms.Core.Entities;
using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill entity);
        Task<IEnumerable<SelectListItemModel>> GetSkillListItemsAsync();
        Task<SelectListItemModel> GetAsync(string skillName);

    }
}
