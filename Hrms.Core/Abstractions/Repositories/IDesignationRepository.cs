using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IDesignationRepository
    {
        Task AddAsync(Designation entity);
        Task<DesignationModel> GetDetailAsync(int? id);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<MatTableResponse<DesignationModel>> GetPageListAsync(MatDataTableRequest model);
        void Update(Designation entity);
        Task<Designation> FindAsync(int id);
        Task<bool> IsDesignationExistAsync(string designation);

    }
}
