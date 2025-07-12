using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Company;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class DepartmentManager : IDepartmentsManager
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentManager(IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(DepartmentModel model)
        {
            var Department = new Department
            {
                Name = model.Name,
                Code = model.Code,
                Description  = model.Description,
                Status = Constants.RecordStatus.Active
            };
            await _departmentRepository.AddAsync(Department);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DepartmentModel> GetDetailAsync(int id)
        {
            return await _departmentRepository.GetDetailAsync(id);
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _departmentRepository.GetSelectListItemAsync();
        }

        public async Task<MatTableResponse<DepartmentModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            return await _departmentRepository.GetPagedListAsync(model);
        }

        public async Task UpdateAsync(DepartmentModel model)
        {
            var department = await _departmentRepository.FindAsync(model.Id);

            department.Name = model.Name;
            department.Code = model.Code;
            department.Description = model.Description;
            _departmentRepository.Update(department);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsExistsAsync(string departmentName)
        {
            return await _departmentRepository.IsExistsAsync(departmentName);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _departmentRepository.FindAsync(id);
            department.Status = Constants.RecordStatus.Deleted;
            _departmentRepository.Update(department);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
