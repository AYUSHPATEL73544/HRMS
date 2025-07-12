using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hrms.Core.Models.Employee;
using Hrms.Core.Abstractions.Managers;

namespace Hrms.Core.Managers
{
    public class WorkHistoryManager: IWorkHistoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkHistroyRepository _workHistroyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public WorkHistoryManager(IWorkHistroyRepository workHistroyRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _workHistroyRepository = workHistroyRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WorkHistoryModel>> GetAsync(int employeeId)
        {
            return await _workHistroyRepository.GetAsync(employeeId);
        }

        public async Task<List<WorkHistoryModel>>GetByUserIdAsync(int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);

            return await _workHistroyRepository.GetAsync(employeeId);
        }

        public async Task<WorkHistoryModel> GetByIdAsync(int id)
        {
            return await _workHistroyRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(WorkHistoryModel model)
        {
            var workHistory = await _workHistroyRepository.FindAsync(model.Id);
            workHistory.DesignationId = model.DesignationId;
            workHistory.DepartmentId = model.DepartmentId;
            workHistory.From = model.From;
            workHistory.To = model.To;
            workHistory.Status = model.Status;
            if(model.To == null) 
            {
                var employee = await _employeeRepository.FindAsync(model.EmployeeId);
                employee.DepartmentId = model.DepartmentId;
                employee.DesignationId = model.DesignationId;
                _employeeRepository.Update(employee);
            }
           

            _workHistroyRepository.Update(workHistory);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
