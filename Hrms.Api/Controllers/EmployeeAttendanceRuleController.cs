using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]

    public class EmployeeAttendanceRuleController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeAttendanceRuleManager _manager;

        public EmployeeAttendanceRuleController(ILogger<EmployeeAttendanceRuleController> logger,
            IEmployeeAttendanceRuleManager employeeAttendanceRuleManager)
        {
            _logger = logger;
            _manager = employeeAttendanceRuleManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] EmployeeAttendanceModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Employee attendance rule");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(MatTableResponse<EmployeeAttendanceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetpagedListAsync(model));
        }


        [HttpGet("inactive-list")]
        [ProducesResponseType(typeof(MatTableResponse<EmployeeAttendanceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInActivePagedList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetInActivepagedListAsync(model));
        }
    }
}
