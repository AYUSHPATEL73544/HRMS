using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Company;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _dataContext;

        public CompanyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _dataContext.Companies
                 .Where(x => x.Id == id)
                 .SingleAsync();
        }

        public async Task<CompanyModel> GetAsync(int id)
        {
            return await _dataContext.Companies
                         .Where(x => x.Id == id)
                         .Select(x => new CompanyModel
                         {
                             Id = x.Id,
                             BrandName = x.BrandName,
                             Email = x.Email,
                             RegisteredName = x.RegisteredName,
                             Phone = x.Phone,
                             FacebookUrl = x.FacebookUrl,
                             WebsiteUrl = x.WebsiteUrl,
                             LinkedInUrl = x.LinkedInUrl,
                             TwitterUrl = x.TwitterUrl
                         }).SingleAsync();
        }

        public void Update(Company entity)
        {
            _dataContext.Companies.Update(entity);
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await (from u in _dataContext.Users
                          join c in _dataContext.Companies
                          on u.Id equals c.UserId into uc
                          from company in uc.DefaultIfEmpty()
                          join e in _dataContext.Employees
                          on u.Id equals e.UserId into ue
                          from e in ue.DefaultIfEmpty()
                          select company != null ? company.Id : e.CompanyId
                          ).FirstOrDefaultAsync();
        }
    }
}
