using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AttendanceLogController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAttendanceLogManager _manager;

        public AttendanceLogController(ILogger<AttendanceLogController> logger,
            IAttendanceLogManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AttendanceLogModel model)
        {
            try
            {
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clock-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClockIn([FromBody] AttendanceLogModel model)
        {
            try
            {
                await _manager.ClockInAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Clock in ");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clock-out")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClockOut([FromBody] AttendanceLogModel model)
        {
            try
            {
                await _manager.ClockOutAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Clock out");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("detail-by-employeeid/{id}")]
        [ProducesResponseType(typeof(List<AttendanceLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAttendanceDeatilByEmployeeId(id));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AttendanceLogModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailById([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(List<AttendanceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _manager.GetListAsync());
        }

      
        [HttpGet("get-by-userId/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAttendance([FromQuery] AttedanceFilterModel model, [FromRoute]int id)
        {
            return Ok(await _manager.GetAttendanceHistoryByEmployeeId(model,id, User.GetUserId()));
        }

        [HttpGet("get-employee-attendance-history")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAttendanceHistory([FromQuery] AttedanceFilterModel model)
        {
            return Ok(await _manager.GetEmployeeAttendanceHistoryAsync(model, User.GetUserId()));
        }

        [HttpGet]
        [ProducesResponseType(typeof(AttendanceLogModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetails()
        {
            return Ok(await _manager.GetDetailAsync(User.GetUserId()));
        }

        [HttpGet("get-by-attendanceId/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttendanceDetail([FromQuery] AttedanceFilterModel model, [FromRoute] int id)
        {
            return Ok(await _manager.GetAttendancDetailsForEmployeeAsync(model, id));
        }

        [HttpGet("get-attendance-history")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttendanceHistory([FromQuery] AttedanceFilterModel model)
        {
            return Ok(await _manager.GetAttendanceHistoryAsync(model));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] AttendanceLogModel model)
        {
            try
            {
                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update AttendanceLog");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAttendanceLog([FromRoute] int id)
        {
            await _manager.DeleteAttendanceLogAsync(id);
            return Ok();
        }

        [HttpDelete("by-attendance/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAttendance([FromRoute] int id)
        {
            try
            {
                await _manager.DeleteAttendanceAsync(id);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Add");
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("paged-list")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model)
        {
            try
            {
                return Ok(await _manager.GetPagedListAsync(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("paged-list-for-employee")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedListForEmployee([FromQuery] MatDataTableRequest model)
        {
            try
            {
                return Ok(await _manager.GetPagedListForEmployeeAsync(model, User.GetUserId()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get");
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("get-attendancelog-event/{year}/{month}/{employeeId}")]
        [ProducesResponseType(typeof(List<AttendanceEventModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttendanceLogEvent([FromRoute] int year, [FromRoute] int month, [FromRoute] int employeeId)
        {
            return Ok(await _manager.GetAttendanceLogEventAsync(year, month, employeeId));
        }

        [HttpGet("get-employee-attendance-log/{year}/{month}")]
        [ProducesResponseType(typeof(List<AttendanceEventModel>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAttendanceLogEvent([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _manager.GetEmployeeAttendanceLogEventAsync(year, month, User.GetUserId()));
        }

        [HttpGet("get-absent-event/{year}/{month}/{employeeId}")]
        [ProducesResponseType(typeof(List<AttendanceEventModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAbsentEvent([FromRoute] int year, [FromRoute] int month, [FromRoute] int employeeId)
        {
            return Ok(await _manager.GetAbsentEventAsync(year, month, employeeId));
        }

        [HttpGet("get-employee-absent-event/{year}/{month}")]
        [ProducesResponseType(typeof(List<AttendanceEventModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAbsentEvent([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _manager.GetEmployeeAbsentEventAsync(year, month, User.GetUserId()));
        }
    }
}
