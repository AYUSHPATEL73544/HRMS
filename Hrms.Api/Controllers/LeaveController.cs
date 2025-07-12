using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILeaveManager _manager;
        public LeaveController(ILogger<LeaveController> logger,
            ILeaveManager leaveManager)
        {
            _logger = logger;
            _manager = leaveManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] LeaveModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Leave");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LeaveModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailById([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail()
        {
            return Ok(await _manager.GetDetailAsync());
        }

       
        [HttpGet("by-ruleid/{id}")]
        [ProducesResponseType(typeof(Leave), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByRuleId([FromRoute]int id)
        {
            return Ok(await _manager.GetByRuleId(id, User.GetUserId()));
        }

        [HttpGet("paged-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPagedListAsync(model));
        }

        [HttpGet("assign-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssignRuleList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetAssignRuleListAsync(model));
        }


        [HttpGet("inactive-assign-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInactiveAssignListAsync([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetInactiveAssignListAsync(model));
        }

        [HttpGet("get-leave-balance")]
        [ProducesResponseType(typeof(List<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaveBalanceList()
        {
            return Ok(await _manager.GetLeaveBalanceListAsync());
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(List<LeaveModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _manager.GetListAsync(User.GetUserId()));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] LeaveModel model)
        {
            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Leave");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{employeeId}/{ruleId}")]
        public async Task<IActionResult> Delete([FromRoute] int employeeId, [FromRoute]int ruleId)
        {
            try
            {
                await _manager.DeleteAsync(employeeId, ruleId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Leave");
                return BadRequest(ex.Message);
            }
        }
    }
}
