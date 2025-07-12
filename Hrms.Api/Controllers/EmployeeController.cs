using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using Hrms.Core.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeManager _manager;

        public EmployeeController(ILogger<EmployeeController> logger,
            IEmployeeManager employeeManager)
        {
            _logger = logger;
            _manager = employeeManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] EmployeeModel model)
        {
            try
            {

                //if (await _manager.IsCodeExistAsync(model.Code))
                //{
                //    return BadRequest("Employee code already exists.");
                //}

                //if (await _manager.IsEmailExistsAsync(model.Email))
                //{
                //    return BadRequest("Email already exists.");
                //}

                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Employee");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("name/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNameById([FromRoute] int id)
        {
            try
            {
                return Ok(await _manager.GetNameByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("get-select-list-item")]
        [ProducesResponseType(typeof(List<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSelectListItem()
        {
            return Ok(await _manager.GetSelectListItemAsync());
        }

        [HttpGet("manager-list")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetManagerList()
        {
            return Ok(await _manager.GetManagerListAsync(User.GetUserId()));
        }


        [HttpGet("select-list-items/{leaveRuleId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeListItemByLeaveRuleId([FromRoute] int leaveRuleId)
        {
            return Ok(await _manager.GetEmployeeListItemByLeaveRuleIdAsync(leaveRuleId));
        }

        [HttpGet("select-list/{attendanceRuleId}")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeListByAttendanceRuleId([FromRoute] int attendanceRuleId)
        {
            return Ok(await _manager.GetEmployeeListByAttendanceRuleIdAsync(attendanceRuleId));
        }



        [HttpGet()]
        [ProducesResponseType(typeof(List<EmployeeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserIdDetail()
        {
            return Ok(await _manager.GetByUserIdAsync(User.GetUserId()));
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _manager.GetListAsync());
        }

        [HttpGet("page-list")]
        [ProducesResponseType(typeof(MatTableResponse<EmployeeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model ,int employeeStatus)
        {
            return Ok(await _manager.GetPageListAsync(model , employeeStatus));  
        }

        [HttpGet("getByDepartmentId/{departmentId}")]
        [ProducesResponseType(typeof(List<EmployeeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDepartmentId([FromRoute] int departmentId)
        {
            return Ok(await _manager.GetByDepartmentId(departmentId));
        }

        [HttpGet("getByDesignationId/{designationId}")]
        [ProducesResponseType(typeof(List<EmployeeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDesignationId([FromRoute] int designationId)
        {
            return Ok(await _manager.GetByDesignationId(designationId));
        }


        [HttpGet("inactive-employee-list")]
        [ProducesResponseType(typeof(MatTableResponse<EmployeeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInactiveEmployeeList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetInactiveEmployeeListAsync(model));
        }

        [HttpGet("calendar-event/{year}/{month}")]
        [ProducesResponseType(typeof(List<CompanyEventsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalendarEvent([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _manager.GetCalendarEventAsync(year, month));
        }

        [HttpGet("calendar-leave-event/{year}/{month}/{employeeId}")]
        [ProducesResponseType(typeof(List<CompanyEventsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCalendarLeaveEvent([FromRoute] int year, [FromRoute] int month, [FromRoute] int employeeId)
        {
            return Ok(await _manager.GetLeaveLogCalendarEventAsync(year, month,employeeId));
        }

        [HttpGet("employee-leave-calendar-event/{year}/{month}")]
        [ProducesResponseType(typeof(List<CompanyController>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeLeaveCalendarEvent([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _manager.GetEmployeeLeaveCalendarEventAsync(year, month, User.GetUserId()));
        }

        [HttpGet("get-candidate-list")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCandidateList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetCandidateListAsync(model, User.GetUserId()));
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] EmployeeModel model)
        {
            try
            {
                //if (await _manager.IsCodeExistAsync(model.Id, model.Code))
                //{
                //    return BadRequest("Employee code already exists.");
                //}

                //if (await _manager.IsEmailExistsAsync(model.Id, model.Email))
                //{
                //    return BadRequest("Email already exists.");
                //}

                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Employee");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }
    }
}
