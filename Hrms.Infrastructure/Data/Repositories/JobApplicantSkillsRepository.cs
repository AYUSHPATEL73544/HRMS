using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class JobApplicantSkillsRepository : IJobApplicantSkillsRepository
    {
        private readonly DataContext _dataContext;

        public JobApplicantSkillsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(ApplicantsSkill candidateSkill)
        {
            await _dataContext.AddAsync(candidateSkill);
        }

    }
}
