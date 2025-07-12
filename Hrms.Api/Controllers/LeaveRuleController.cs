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
    public class LeaveRuleController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly ILeaveRuleManager _manager;
        public LeaveRuleController(ILogger<LeaveRuleController> logger,
            ILeaveRuleManager leaveRuleManager)
        {
            _logger = logger;
            _manager = leaveRuleManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] LeaveRuleModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add LeaveRule");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LeaveRuleModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveRuleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetListAsync(model, User.GetUserId()));
        }
        

        [HttpGet("get-select-list-item")]
        [ProducesResponseType(typeof(List<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSelectListItem()
        {
            return Ok(await _manager.GetSelectListItemAsync());
        }

        [HttpGet("paged-list")]
        [ProducesResponseType(typeof(MatTableResponse<LeaveRuleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPageList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPageListAsync(model));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] LeaveRuleModel model)
        {
            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update LeaveRule");
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
