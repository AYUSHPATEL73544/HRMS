using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveLogRepository _leaveLogRepository;
        private readonly IWorkHistroyRepository _workHistroyRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _documentRepository;

        public EmployeeManager(IEmployeeRepository employeeRepository,
            IAddressRepository addressRepository,
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            IWorkHistroyRepository workHistroyRepository,
            ILeaveRepository leaveRepository,
            ILeaveLogRepository leaveLogRepository,
            IJobApplicationRepository jobApplicationRepository,
            IUnitOfWork unitOfWork,
            IDocumentRepository documentRepository)
        {
            _employeeRepository = employeeRepository;
            _addressRepository = addressRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _workHistroyRepository = workHistroyRepository;
            _leaveRepository = leaveRepository;
            _leaveLogRepository = leaveLogRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
        }

        public async Task<bool>IsCodeExistAsync(string code)
        {
            return await _employeeRepository.IsCodeExistAsync(code);
        }

        public async Task<bool> IsCodeExistAsync(int id, string code)
        {
            var employee = await _employeeRepository.FindAsync(id);

            if (!employee.Code.Equals(code))
            {

                return await _employeeRepository.IsCodeExistAsync(code);
            }
            else return false;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> IsEmailExistsAsync(int id , string email)
        {
            var employee = await _employeeRepository.FindAsync(id);

            if (!employee.Email.Equals(email)){
                
                return await _userRepository.EmailExistsAsync(email);
            }
            else return false;
        }

       


        public async Task AddAsync(EmployeeModel model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                var uniqueCode = await _employeeRepository.LastEmployeeAsync();

                if (uniqueCode != 0)
                {
                    uniqueCode++;
                }
                else
                {
                    uniqueCode = 1;
                }

                string year = (model.DateOfJoining.Year % 100).ToString("00");
                string month = model.DateOfJoining.Month.ToString("00");
                var joiningCode = year + month;

                var employeeCode = Constants.employeeCode.code + "X" + "/" + joiningCode + "/" + uniqueCode;

                var firstName = new string(model.FirstName.Where(c => !char.IsWhiteSpace(c)).ToArray());
                var lastName = new string(model.LastName.Where(c => !char.IsWhiteSpace(c)).ToArray());

                var email = firstName.ToLower() + "." + lastName.ToLower() + Constants.EmployeeMail.mail;

                var emailPattern = firstName.ToLower() + "." + lastName.ToLower();

                var emailCount = await _userRepository.SameEmailCountAsync(emailPattern);

                if (emailCount > 0)
                {
                    email = firstName.ToLower() + "." + lastName.ToLower() + emailCount + Constants.EmployeeMail.mail;
                }


                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
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
                    UserId = user.Id,
                    Code = employeeCode.ToUpper(),
                    CompanyId = model.CompanyId,
                    DesignationId = model.DesignationId,
                    DepartmentId = model.DepartmentId,
                    ManagerId = model.ManagerId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BloodGroup = model.BloodGroup,
                    MaritalStatus = model.MaritalStatus,
                    Gender = model.Gender,
                    Email = email,
                    Phone = model.Phone,
                    AlternateEmail = model.AlternateEmail,
                    AlternatePhone = model.AlternatePhone,
                    DateOfBirth = model.DateOfBirth,
                    DateOfJoining = model.DateOfJoining,
                    DateOfLeaving = model.DateOfLeaving,
                    Status = Constants.RecordStatus.Active,
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedOn = Utility.GetDateTime(),
                    CreatedById = user.Id,
                    ProbationPeriod = model.ProbationPeriod,
                    ExitDate = model.ExitDate,
                    Note = model.Note,
                    NoticePeriod = model.NoticePeriod,
                    EmployeeType = model.EmployeeType
                };
                await _employeeRepository.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            var employeeDetail = await _employeeRepository.GetAsync(id);
            if (employeeDetail != null)
            {
                employeeDetail.PermanentAddress = await _addressRepository.GetDetailAsync(employeeDetail.Id, Constants.AddressType.Permanent);

                employeeDetail.CurrentAddress = await _addressRepository.GetDetailAsync(employeeDetail.Id, Constants.AddressType.Current);
            }
            return employeeDetail;
        }

        public async Task<string> GetNameByIdAsync(int id)
        {
            return await _employeeRepository.GetNameByIdAsync(id);
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _employeeRepository.GetSelectListItemAsync();
        }

        public async Task<List<SelectListItemModel>> GetManagerListAsync(int userId)
        {
            return await _employeeRepository.GetManagerListAsync(userId);
        }

        public async Task<IEnumerable<SelectListItemModel>> GetEmployeeListItemByLeaveRuleIdAsync(int leaveRuleId)
        {
            return await _employeeRepository.GetEmployeeListItemByLeaveRuleIdAsync(leaveRuleId);
        }
        public async Task<IEnumerable<SelectListItemModel>> GetEmployeeListByAttendanceRuleIdAsync(int attendanceRuleId)
        {
            return await _employeeRepository.GetEmployeeListByAttendanceRuleIdAsync(attendanceRuleId);
        }
    
        public async Task<EmployeeModel> GetByUserIdAsync(int userId)
        {
            var employeeDetail = await _employeeRepository.GetByUserIdAsync(userId);

            if (employeeDetail != null)
            {
                employeeDetail.PermanentAddress = await _addressRepository.GetDetailAsync(employeeDetail.Id, Constants.AddressType.Permanent);

                employeeDetail.CurrentAddress = await _addressRepository.GetDetailAsync(employeeDetail.Id, Constants.AddressType.Current);
            }

            return employeeDetail;
        }

        public async Task<List<EmployeeModel>> GetByDepartmentId(int dpeartmentId)
        {
            return await _employeeRepository.GetByDepartmentId(dpeartmentId);
        }
        public async Task<List<EmployeeModel>> GetByDesignationId(int designationId)
        {
            return await _employeeRepository.GetByDesignationId(designationId);
        }


        public async Task<int>GetUserIdAsync(int id)
        {
            return await _employeeRepository.GetUserIdAsync(id);
        }

        public async Task<List<EmployeeModel>> GetListAsync()
        {
            var employees = await _employeeRepository.GetListAsync();

            foreach (var employee in employees)
            {
                employee.PermanentAddress = await _addressRepository.GetDetailAsync(employee.Id, Constants.AddressType.Permanent);
                employee.CurrentAddress = await _addressRepository.GetDetailAsync(employee.Id, Constants.AddressType.Current);
               // var manager = await _employeeManagerRepository.GetByEmployeeIdAsync(employee.Id);

            }
            return employees;
        }

        public async Task<MatTableResponse<EmployeeModel>> GetPageListAsync(MatDataTableRequest model, int employeeStatus)
        {
            return await _employeeRepository.GetPageListAsync(model, employeeStatus);
        }

        public async Task<MatTableResponse<EmployeeModel>> GetInactiveEmployeeListAsync(MatDataTableRequest model)
        {
            return await _employeeRepository.GetInactiveEmployeeListAsync(model);
        }

        public async Task<List<CompanyEventsModel>> GetCalendarEventAsync(int year, int month)
        {
            return await _employeeRepository.GetCalendarEventAsync(year, month);
        }

        public async Task<List<CompanyEventsModel>> GetLeaveLogCalendarEventAsync(int year, int month, int employeeId)
        {
            return await _employeeRepository.GetLeaveLogCalendarEventAsync(year, month, employeeId);
        }

        public async Task<List<CompanyEventsModel>> GetEmployeeLeaveCalendarEventAsync(int year, int month, int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _employeeRepository.GetLeaveLogCalendarEventAsync(year, month, employeeId);
        }
        public async Task<MatTableResponse<ApplicantModel>> GetCandidateListAsync(MatDataTableRequest model, int employeeId)
        {
            return await _jobApplicationRepository.GetCandidateListAsync(model, employeeId );
        }

        public async Task UpdateAsync(EmployeeModel model, int userId)
        {
            var employee = await _employeeRepository.FindAsync(model.Id);

            var email = employee.Email;
            var employeeCode = employee.Code;
            int lastSlashIndex = employee.Code.LastIndexOf('/');

            var uniqueCode = "";

            if (lastSlashIndex != -1)
            {
                uniqueCode = employee.Code.Substring(lastSlashIndex + 1);
            }


            if (!model.FirstName.Equals(employee.FirstName) || !model.LastName.Equals(employee.LastName))
            {

                var firstName = new string(model.FirstName.Where(c => !char.IsWhiteSpace(c)).ToArray());
                var lastName = new string(model.LastName.Where(c => !char.IsWhiteSpace(c)).ToArray());

                 email = firstName.ToLower() + "." + lastName.ToLower() + Constants.EmployeeMail.mail;

                var emailPattern = firstName.ToLower() + "." + lastName.ToLower();

                var emailCount = await _userRepository.SameEmailCountAsync(emailPattern);

                if (emailCount > 0)
                {
                    email = firstName.ToLower() + "." + lastName.ToLower() + emailCount + Constants.EmployeeMail.mail;
                }
            }

            if (model.DepartmentId != null)
            {
                var deparment = await _departmentRepository.GetDetailAsync(model.DepartmentId);

                string year = (model.DateOfJoining.Year % 100).ToString("00");
                string month = model.DateOfJoining.Month.ToString("00");

                var joiningCode  = year + month;

                employeeCode = Constants.employeeCode.code + deparment.Code + "/" + joiningCode + "/" + uniqueCode;
            }

            if (model.DateOfJoining != employee.DateOfJoining)
            {
                var leaves = await _leaveRepository.GetListByEmployeeIdAsync(employee.Id);
                foreach (var leave in leaves)
                {
                    var getLeave = await _leaveRepository.FindAsync(leave.Id);
                    var updateCerditedLeave = _leaveRepository.CalculateLeavesToCredit(model.DateOfJoining, (int)leave.Total);
                    getLeave.Credited = updateCerditedLeave;
                    getLeave.Available = updateCerditedLeave - leave.Applied;
                    _leaveRepository.Update(getLeave);
                }
            }
            if (model.DepartmentId != null
                && model.DesignationId != null
                && (model.DepartmentId != employee.DepartmentId
                || model.DesignationId != employee.DesignationId
                || employee.DepartmentId == null))
            {

                var perviousWorkHistory = await _workHistroyRepository.GetByPreviousWorkHistoryAsync(employee.DepartmentId, employee.DesignationId, employee.Id);
                if (perviousWorkHistory != null)
                {
                    perviousWorkHistory.To = model.StartedFrom.Value.Date.AddDays(-1);
                    _workHistroyRepository.Update(perviousWorkHistory);

                    var workHistory = new WorkHistory
                    {
                        EmployeeId = employee.Id,
                        DepartmentId = model.DepartmentId,
                        DesignationId = model.DesignationId,
                        From = model.StartedFrom.Value,
                        Status = Constants.RecordStatus.Active
                    };
                    await _workHistroyRepository.AddAsync(workHistory);
                }
                else
                {
                    var workHistory = new WorkHistory
                    {
                        EmployeeId = employee.Id,
                        DepartmentId = model.DepartmentId,
                        DesignationId = model.DesignationId,
                        From = model.DateOfJoining,
                        Status = Constants.RecordStatus.Active
                    };
                    await _workHistroyRepository.AddAsync(workHistory);
                }
            }


            employee.Code = employeeCode.ToUpper();
            employee.CompanyId = model.CompanyId;
            employee.DepartmentId = model.DepartmentId == null ? employee.DepartmentId : model.DepartmentId ;
            employee.DesignationId = model.DesignationId == null ? employee.DesignationId : model.DesignationId;
            employee.ManagerId = model.ManagerId;
            employee.BloodGroup = model.BloodGroup;
            employee.MaritalStatus = model.MaritalStatus;
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Gender = model.Gender;
            employee.Email = email;
            employee.Phone = model.Phone;
            employee.AlternatePhone = model.AlternatePhone;
            employee.AlternateEmail = model.AlternateEmail;
            employee.DateOfBirth = model.DateOfBirth;
            employee.DateOfJoining = model.DateOfJoining;
            employee.CreatedById = userId;
            employee.EffectiveFrom = Utility.GetDateTime();
            employee.ProbationPeriod = model.ProbationPeriod;
            employee.NoticePeriod = model.NoticePeriod;
            employee.Note = model.Note;
            employee.EmployeeType = model.EmployeeType;
            if (model.Status == Constants.RecordStatus.Active)
            {
                employee.ExitDate = null;
                employee.DateOfLeaving = null;
                employee.Note = null;
            }
            else
            {
                employee.ExitDate = model.ExitDate;
                employee.DateOfLeaving = model.DateOfLeaving;
                employee.Note = model.Note;
            }
            employee.Status = model.Status;


            _employeeRepository.Update(employee);
            await UpsertAddressAsync(model);

            var user = await _userRepository.FindAsync(employee.UserId);
            user.Email = email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = email;
            user.NormalizedEmail = email;
            user.NormalizedUserName = email;
            user.CreatedOn = Utility.GetDateTime();
            if (model.Status == Constants.RecordStatus.Inactive)
            {
                user.Status = Constants.RecordStatus.Inactive;
            }
            if (model.Status == Constants.RecordStatus.Active)
            {
                user.Status = Constants.RecordStatus.Active;
            }

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

           
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _employeeRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Inactive;
            _employeeRepository.Update(entity);
            await _userRepository.DeleteAsync(entity.UserId);

            var leaveLogs = await _leaveLogRepository.GetByEmployeeIdAsync(id);
            if (leaveLogs.Count > 0)
            {
                foreach (var log in leaveLogs)
                {
                    log.Status = Constants.RecordStatus.Inactive;
                    log.EffectiveTo = Utility.GetDateTime();
                    _leaveLogRepository.Update(log);
                }
            }
            var leaves = await _leaveRepository.GetListByEmployeeIdAsync(id);
            if (leaves.Count > 0)
            {
                foreach (var leave in leaves)
                {
                    leave.Status = Constants.RecordStatus.Inactive;
                    leave.EffectiveTo = Utility.GetDateTime();
                    _leaveRepository.Update(leave);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        #region private method
        private async Task UpsertAddressAsync(EmployeeModel model)
        {
            if (model.PermanentAddress != null)
            {
                if (model.PermanentAddress.Id > 0)
                {
                    var address = await _addressRepository.FindAsync(model.PermanentAddress.Id);
                    address.Line1 = model.PermanentAddress.Line1;
                    address.Line2 = model.PermanentAddress.Line2;
                    address.CityId = model.PermanentAddress.CityId;
                    address.PinCode = model.PermanentAddress.PinCode;
                    _addressRepository.Update(address);
                }
                else
                {
                    var address = new Address
                    {
                        IdentificationId = model.Id,
                        Line1 = model.PermanentAddress.Line1,
                        Line2 = model.PermanentAddress.Line2,
                        AddressType = Constants.AddressType.Permanent,
                        CityId = model.PermanentAddress.CityId,
                        PinCode = model.PermanentAddress.PinCode
                    };
                    await _addressRepository.AddAsync(address);
                }
            }
            if (model.CurrentAddress != null)
            {
                if (model.CurrentAddress.Id > 0)
                {
                    var address = await _addressRepository.FindAsync(model.CurrentAddress.Id);
                    address.Line1 = model.CurrentAddress.Line1;
                    address.Line2 = model.CurrentAddress.Line2;
                    address.CityId = model.CurrentAddress.CityId;
                    address.PinCode = model.CurrentAddress.PinCode;
                    _addressRepository.Update(address);
                }
                else
                {
                    var address = new Address
                    {
                        IdentificationId = model.Id,
                        Line1 = model.CurrentAddress.Line1,
                        Line2 = model.CurrentAddress.Line2,
                        AddressType = Constants.AddressType.Current,
                        CityId = model.CurrentAddress.CityId,
                        PinCode = model.CurrentAddress.PinCode
                    };
                    await _addressRepository.AddAsync(address);
                }
            }

        }
        #endregion

    }
}
