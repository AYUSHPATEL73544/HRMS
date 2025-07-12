using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using System;

using Hrms.Core.Models.Employee;
using Hrms.Core.Entities;
using Hrms.Core.Utilities;
using Hrms.Core.Abstractions.Managers;

namespace Hrms.Core.Managers
{
    public class TeamManager : ITeamManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeManagerRepository _employeeManagerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;
        public TeamManager(IEmployeeManagerRepository employeeManagerRepository,
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IDesignationRepository designationRepository,
            IUnitOfWork unitOfWork)
        {
            _employeeManagerRepository = employeeManagerRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(TeamModel model)
        {
            var team = new Team
            {
                EmployeeId = model.EmployeeId,
                ManagerId = model.ManagerId,
                Type = model.Type,
                CreatedOn = Utility.GetDateTime(),
                EffectiveFrom = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };

            await _employeeManagerRepository.AddAsync(team);

            var employee = await _employeeRepository.FindAsync(model.EmployeeId);
            employee.ManagerId = team.ManagerId;
            _employeeRepository.Update(employee);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TeamModel>> GetByEmployeeIdAsync(int employeeId)
        {
            var teams = await _employeeManagerRepository.GetByEmployeeIdAsync(employeeId);

            foreach (var team in teams)
            {
                if (team.ManagerId != null && team.ManagerId.HasValue)
                {
                    var employee = await _employeeRepository.GetAsync(team.ManagerId.Value);

                    if (employee == null)
                    {
                        continue;
                    }
                    var departemnt = await _departmentRepository.GetDetailAsync(employee.DepartmentId);
                    var designation = await _designationRepository.GetDetailAsync(employee.DesignationId);

                    team.ManagerName = employee.FirstName + " " + employee.LastName;

                    if (departemnt != null)
                    {
                        team.Department = departemnt.Name;
                    }
                    if (designation != null)
                    {
                        team.Designation = designation.Name;
                    }
                }
            }
            return teams;
        }

        public async Task<List<TeamModel>> GetByUserIdAsync(int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            var teams = await _employeeManagerRepository.GetByEmployeeIdAsync(employeeId);
            foreach (var team in teams)
            {
                if (team.ManagerId != null && team.ManagerId.HasValue)
                {
                    var employee = await _employeeRepository.GetAsync(team.ManagerId.Value);
                    var departemnt = await _departmentRepository.GetDetailAsync(employee.DepartmentId);
                    var designation = await _designationRepository.GetDetailAsync(employee.DesignationId);

                    team.ManagerName = employee.FirstName + " " + employee.LastName;
                    if(departemnt != null)
                    {
                        team.Department = departemnt.Name;
                    }
                    if(designation != null)
                    {
                        team.Designation = designation.Name;
                    }
                    
                }
            }
            return teams;
        }

        public async Task<List<TeamReportessModel>> GetReportessListAsync(int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            var teams = await _employeeManagerRepository.GetByManagerIdAsync(employeeId);
            foreach (var team in teams)
            {
                var employee = await _employeeRepository.GetAsync(team.Id);
                var departemnt = await _departmentRepository.GetDetailAsync(employee.DepartmentId);
                var designation = await _designationRepository.GetDetailAsync(employee.DesignationId);

                if (departemnt != null)
                {
                    team.DepartmentName = departemnt.Name;
                }
                if (designation != null)
                {
                    team.DesignationName = designation.Name;
                }
            }
            return teams;
        }

        public async Task<TeamModel> GetByIdAsync(int id)
        {
            return await _employeeManagerRepository.GetByIdAsync(id);
        }

        public async Task<List<TeamReportessModel>> GetByManagerIdAsync(int id)
        {
            var teams = await _employeeManagerRepository.GetByManagerIdAsync(id);
            foreach (var team in teams)
            {
                var employee = await _employeeRepository.GetAsync(team.Id);
                var department = await _departmentRepository.GetDetailAsync(employee.DepartmentId);
                if (department != null)
                {
                    team.DepartmentName = department.Name;
                }
                var designation = await _designationRepository.GetDetailAsync(employee.DesignationId);
                if (designation != null)
                {
                    team.DesignationName = designation.Name;
                }
            }
            return teams;
        }

        public async Task<bool> IsManagerAssignedAsync(int employeeId, int managerId)
        {
            return await _employeeManagerRepository.IsManagerAssignedAsync(employeeId, managerId);
        }

        public async Task UpdateAsync(TeamModel model)
        {
            var team = await _employeeManagerRepository.FindAsync(model.Id);

            if (team != null)
            {
                team.ManagerId = model.ManagerId;
                team.Type = model.Type;
                _employeeManagerRepository.Update(team);
            }


            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var team = await _employeeManagerRepository.FindAsync(id);

            team.Status = Constants.RecordStatus.Deleted;

            _employeeManagerRepository.Update(team);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
