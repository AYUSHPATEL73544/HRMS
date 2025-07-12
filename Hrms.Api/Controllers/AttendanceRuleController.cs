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
    public class AttendanceRuleController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAttendanceRuleManager _manager;

        public AttendanceRuleController(ILogger<AttendanceRuleController> logger,
            IAttendanceRuleManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AttendanceRuleModel model)
        {
            if(await _manager.IsExistsAsync(model.Year) || await _manager.IsExistsAsync(model.Year+1))
            {
                return BadRequest("Attendance rule already exists.");
            }    
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AttendanceRuleModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceRuleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPagedListAsync(model, User.GetUserId()));
        }

        [HttpGet("get-select-list-item")]
        [ProducesResponseType(typeof(List<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSelectListItem()
        {
            return Ok(await _manager.GetSelectListItemAsync());
        }

        [HttpGet("get-paged-result")]
        [ProducesResponseType(typeof(MatTableResponse<AttendanceRuleListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedResult([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPagedResultAsync(model));
        }

        [HttpGet("getByYear/{year}")]
        [ProducesResponseType(typeof(AttendanceRuleModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByYearAsync(int year)
        {
            return Ok(await _manager.GetByYearAsync(year));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] AttendanceRuleModel model)
        {
            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update AttendanceRule");
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
