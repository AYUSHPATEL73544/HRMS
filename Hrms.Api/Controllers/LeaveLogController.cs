using Hrms.Core.Abstractions.Managers;
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
    public class LeaveLogController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILeaveLogManager _manager;
        public LeaveLogController(ILogger<LeaveLogController> logger,
            ILeaveLogManager leaveLogManager)
        {
            _logger = logger;
            _manager = leaveLogManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] LeaveLogModel model)
        {
            if (await _manager.IsExistsAsync(User.GetUserId(), model.StartDate, model.EndDate))
            {
                return BadRequest("You have already applied for leave during the selected date range.");
            }
            try
            {
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add LeaveLog");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LeaveLogModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("paged-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveLogModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetailByUserId([FromQuery] LeaveLogFilterModel model)
        {
            try
            {
                return Ok(await _manager.PagedListByUserIdAsync(model,User.GetUserId()));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Page list LeaveLog");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("reportee-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReporteePagedList([FromQuery] LeaveLogFilterModel model)
        {
            return Ok(await _manager.GetReporteePagedListAsync(model , User.GetUserId()));
        }

        [HttpGet("paged-list-by-employee/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveLogModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPageListByEmployeeId([FromQuery] LeaveLogFilterModel model,[FromRoute] int id)
        {
            try
            {
                return Ok(await _manager.PagedListByEmployeeIdAsync(model, id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Page list LeaveLog");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] LeaveLogFilterModel model)
        {
            return Ok(await _manager.GetPagedListAsync(model));
        }



        [HttpGet("pending-leaves-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveLogModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingLeavesList([FromQuery] LeaveLogFilterModel model)
        {
            return Ok(await _manager.GetPendingLeavesPagedListAsync(model));
        }

        [HttpGet("total-leave-count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalCount([FromQuery] LeaveLogModel model)
        {
            try
            {
                return Ok(await _manager.GetTotalLeaveCountAsync(model, User.GetUserId()));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Getting total leave count.");
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] LeaveLogModel model)
        {
            if (await _manager.IsExistsAsync(User.GetUserId(), model.StartDate, model.EndDate, model.Id))
            {
                return BadRequest("You have already applied for leave during the selected date range.");
            }

            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update LeaveLog");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeStatus([FromBody] LeaveLogChangeStatusModel model)
        {
            try
            {
                await _manager.ChangeStatusAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Change Status");
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
        [HttpGet("{startDate}/{endDate}")]
        [ProducesResponseType(typeof(List<LeaveLogModel>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaveLog([FromRoute] DateTime startDate,[FromRoute] DateTime endDate)
        {
            return Ok(await _manager.GetLeaveLog(startDate,endDate));
        }
    }
}
