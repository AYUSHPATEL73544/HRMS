using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Entities;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;
using Hrms.Core.Models;
using Hrms.Core.Abstractions.Managers;


namespace Hrms.Core.Managers
{
    public class JobApplicationManager : IJobApplicationManager
    {
        private readonly IJobApplicationRepository _candidateRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IJobApplicantSkillsRepository _candidateSkillRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobApplicationManager(IJobApplicationRepository candidateRepository,
            IDocumentRepository documentRepository,
            IDepartmentRepository departmentRepository,
            IEmployeeRepository employeeRepository,
            ISkillRepository skillRepository,
            IJobApplicantSkillsRepository candidateSkillRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _candidateRepository = candidateRepository;
            _documentRepository = documentRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
            _candidateSkillRepository = candidateSkillRepository;
            _skillRepository = skillRepository;
            _userRepository = userRepository;
        }

        public async Task AddAsync(ApplicantModel model, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var candidate = new JobApplication
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    QualificationTypeId = model.QualificationTypeId,
                    CourseTypeId = model.CourseTypeId,
                    CourseName = model.CourseName,
                    Stream = model.Stream,
                    PassingYear = model.PassingYear,
                    Pursuing = model.IsPursuing,
                    Gender = model.Gender,
                    Remark = string.IsNullOrEmpty(model.Remark) ? string.Empty : model.Remark,
                    Hired = model.IsHired,
                    Shortlisted = model.IsShortlisted,
                    ShortlistedDate = model.ShortlistedDate,
                    CreatedOn = Utility.GetDateTime(),
                    CreatedById = userId,
                    EffectiveFrom = Utility.GetDateTime(),
                    Status = Constants.RecordStatus.Pending,
                    MarketingChannel = model.MarketingChannel,
                    ApplicantsSkills = new List<ApplicantsSkill>()
                };
                await _candidateRepository.AddAsync(candidate);
                await _unitOfWork.SaveChangesAsync();

                foreach (var skillName in model.SkillNames)
                {
                    var skill = await _skillRepository.GetAsync(skillName);
                    if (skill != null)
                    {
                        var candidateSkill = new ApplicantsSkill
                        {
                            ApplicantId = candidate.Id,
                            SkillId = (int)skill.Key
                        };
                        await _candidateSkillRepository.AddAsync(candidateSkill);
                    }
                    else
                    {
                        var entity = new Skill
                        {
                            Name = skillName,
                            Status = Constants.RecordStatus.Active
                        };
                        await _skillRepository.AddAsync(entity);
                        await _unitOfWork.SaveChangesAsync();
                        var candidateSkill = new ApplicantsSkill
                        {
                            ApplicantId = candidate.Id,
                            SkillId = entity.Id,

                        };
                        await _candidateSkillRepository.AddAsync(candidateSkill);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

                #region document 
                var document = new Document
                {
                    IdentificationId = candidate.Id,
                    Name = model.DocumentDetails.Name,
                    Key = model.DocumentDetails.Key,
                    DocumentType = Constants.DocumentType.Cv,
                    CreatedById = userId,
                    CreatedOn = Utility.GetDateTime(),
                    EffectiveFrom = Utility.GetDateTime(),
                    Status = Constants.RecordStatus.Active
                };
                await _documentRepository.AddAsync(document);
                await _unitOfWork.SaveChangesAsync();
                #endregion

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> IsExistsAsync(string email, string phoneNumber)
        {
            return await _candidateRepository.IsExistsAsync(email, phoneNumber);
        }

        public async Task<bool> IsExistsAsync(int id, string email, string phoneNumber)
        {
            return await _candidateRepository.IsExistsAsync(id, email, phoneNumber);
        }

        public async Task<MatTableResponse<ApplicantModel>> GetListAsync(MatDataTableRequest model)
        {
            return await _candidateRepository.GetListAsync(model);
        }

        public async Task<MatTableResponse<ShortlistCandidateModel>> GetShortlistPageListAsync(MatDataTableRequest model)
        {
            return await _candidateRepository.GetShortlistPageListAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _candidateRepository.FindAsync(id);
            entity.Status = Constants.RecordStatus.Deleted;
            entity.EffectiveTo = Utility.GetDateTime();
            _candidateRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ApplicantModel> GetDetailsAsync(int id)
        {
            return await _candidateRepository.GetDetailsAsync(id);
        }

        public async Task<ApplicantModel> GetAsync(int id)
        {
            return await _candidateRepository.GetAsync(id);
        }

        public async Task ShortlistAsync(int id, int userId)
        {
            var entity = await _candidateRepository.FindAsync(id);
            entity.Status = Constants.RecordStatus.Active;
            entity.Shortlisted = true;
            entity.UpdatedOn = Utility.GetDateTime();
            entity.UpdatedById = userId;
            entity.ShortlistedDate = Utility.GetDateTime();

            _candidateRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task HireAsync(HireModel model, int userId)
        {
            var entity = await _candidateRepository.FindAsync(model.Id);
            entity.Status = Constants.RecordStatus.Active;
            entity.Hired = true;
            entity.UpdatedOn = Utility.GetDateTime();
            entity.UpdatedById = userId;

            var uniqueCode = await _employeeRepository.LastEmployeeAsync();

            if(uniqueCode != 0)
            {
                uniqueCode++;
            }
            else
            {
                uniqueCode = 1;
            }

            var deparment = await _departmentRepository.GetDetailAsync(model.DepartmentId);

            string year = (model.DateOfJoining.Year % 100).ToString("00"); 
            string month = model.DateOfJoining.Month.ToString("00"); 
            var joiningCode = year + month;

            if (deparment.Code == "")
            {
                deparment.Code = "X";
            }
            var employeeCode = Constants.employeeCode.code + deparment.Code + "/" + joiningCode + "/" + uniqueCode ;
            var firstName = new string(entity.FirstName.Where(c => !char.IsWhiteSpace(c)).ToArray());
            var lastName = new string(entity.LastName.Where(c => !char.IsWhiteSpace(c)).ToArray());
            var email = firstName.ToLower() + "." + lastName.ToLower() + Constants.EmployeeMail.mail;

            var emailPattern = firstName.ToLower() + "." + lastName.ToLower();

            var emailCount = await _userRepository.SameEmailCountAsync(emailPattern);

            if(emailCount > 0)
            {
                email = firstName.ToLower() + "." + lastName.ToLower() + emailCount + Constants.EmployeeMail.mail;
            }

            var user = new User
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Status = Constants.RecordStatus.Active,
                CreatedOn = Utility.GetDateTime()
            };

            await _userRepository.CreateAsync(user, "Password@123");
            await _unitOfWork.SaveChangesAsync();

            var employee = new Employee
            {
                CompanyId = model.CompanyId,
                UserId = user.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                Phone= entity.Phone,
                DateOfJoining = model.DateOfJoining,
                Code = employeeCode.ToUpper(),
                Email = email,
                DepartmentId = model.DepartmentId,
                DesignationId = model.DesignationId,
                Status = Constants.RecordStatus.Active,
                EffectiveFrom = Utility.GetDateTime(),
                CreatedOn = Utility.GetDateTime(),
                CreatedById = userId,
            };

            await _employeeRepository.AddAsync(employee);
            _candidateRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(ApplicantModel model, int userId)
        {
            var entity = await _candidateRepository.GetByIdAsync(model.Id);

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Phone = model.Phone;
            entity.QualificationTypeId = model.QualificationTypeId;
            entity.CourseTypeId = model.CourseTypeId;
            entity.CourseName = model.CourseName;
            entity.Stream = model.Stream;
            entity.CourseName = model.CourseName;
            entity.PassingYear = model.PassingYear;
            entity.Pursuing = model.IsPursuing;
            entity.Gender = model.Gender;
            entity.UpdatedOn = Utility.GetDateTime();
            entity.Remark = model.Remark;
            entity.MarketingChannel = model.MarketingChannel;

            _candidateRepository.Remove(entity.ApplicantsSkills);

            foreach (var skillName in model.SkillNames)
            {
                var skill = await _skillRepository.GetAsync(skillName);
                if (skill != null)
                {
                    var candidateSkill = new ApplicantsSkill
                    {
                        ApplicantId = entity.Id,
                        SkillId = (int)skill.Key
                    };
                    await _candidateSkillRepository.AddAsync(candidateSkill);
                }
                else
                {
                    var skil = new Skill    // TOASK: var name 
                    {
                        Name = skillName,
                        Status = Constants.RecordStatus.Active
                    };
                    await _skillRepository.AddAsync(skil);
                    await _unitOfWork.SaveChangesAsync();
                    var candidateSkill = new ApplicantsSkill
                    {
                        ApplicantId = entity.Id,
                        SkillId = skil.Id,

                    };
                    await _candidateSkillRepository.AddAsync(candidateSkill);
                }
            }

            var document =  await _documentRepository.GetAsync(model.Id, Constants.DocumentType.Cv);
            
            document.Name = model.DocumentDetails.Name;
            document.Key = model.DocumentDetails.Key;
            document.UpdatedById = userId;
            document.UpdatedOn = Utility.GetDateTime();

            _documentRepository.Update(document);
            
            await _unitOfWork.SaveChangesAsync();

            _candidateRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(ApplicantChangeStatusModel model, int userId)
        {
            var candidate = await _candidateRepository.FindAsync(model.Id);
            if (model.Status == Constants.RecordStatus.Rejected)
            {
                candidate.Status = Constants.RecordStatus.Rejected;
                candidate.Remark = model.RejectionReason;
            }
            else
            {
                candidate.Status = Constants.RecordStatus.Deleted;
            }
            candidate.UpdatedOn = Utility.GetDateTime();
            candidate.UpdatedById = userId;
            candidate.EffectiveTo = Utility.GetDateTime();
            _candidateRepository.Update(candidate);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
