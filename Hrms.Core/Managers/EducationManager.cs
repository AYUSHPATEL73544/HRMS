using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class EducationManager : IEducationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEducationRepository _educationRepository;
        private readonly IEmployeeRepository _employeeRepository;
      
        public EducationManager(IUnitOfWork unitOfWork,
            IEducationRepository educationRepository,
            IEmployeeRepository employeeRepository)
        {
            _unitOfWork = unitOfWork;
            _educationRepository = educationRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task AddAsync(EducationModel model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);

            var education = new Education
            {
                EmployeeId = employee,
                CourseTypeId = model.CourseTypeId,
                QualificationTypeId = model.QualificationTypeId,
                CollegeName = model.CollegeName,
                UniversityName = model.UniversityName,
                Stream = model.Stream,
                Start = model.Start,
                End = model.End,
                CourseName = model.CourseName,
                CreatedOn = Utility.GetDateTime(),
                EffectiveFrom = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };
            await _educationRepository.AddAsync(education);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<EducationModel> GetByIdAsync(int id)
        {
            return await _educationRepository.GetByIdAsync(id);
        }

        public async Task<MatTableResponse<EducationModel>> GetPageListByEmployeeIdAsync(MatDataTableRequest model, int employeeId)
        {
            return await _educationRepository.GetPageListAsync(model,employeeId);
        }

        public async Task<MatTableResponse<EducationModel>> GetPageListAsync(MatDataTableRequest model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);
           var education = await _educationRepository.GetPageListAsync(model, employee);

            return education;
        }

        public async Task UpdateAsync(EducationModel model)
        {
            var education = await _educationRepository.FindAsync(model.Id);

            education.QualificationTypeId = model.QualificationTypeId;
            education.CourseTypeId = model.CourseTypeId;
            education.Stream = model.Stream;
            education.CollegeName = model.CollegeName;
            education.UniversityName = model.UniversityName;
            education.Start = model.Start;
            education.End = model.End;

            _educationRepository.Update(education);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var education = await _educationRepository.FindAsync(id);

            education.Status = Constants.RecordStatus.Deleted;

            _educationRepository.Update(education);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
