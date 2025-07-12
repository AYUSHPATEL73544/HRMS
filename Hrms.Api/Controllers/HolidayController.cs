using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class HolidayController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IHolidayManager _manager;
        public HolidayController(ILogger<HolidayController> logger,
            IHolidayManager holidayManager)
        {
            _logger = logger;
            _manager = holidayManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] HolidayGroupModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add holiday");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("holiday-event/{year}/{month}")]
        [ProducesResponseType(typeof(List<HolidayModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _manager.GetListAsync(year, month));
        }

        [HttpGet("detail")]
        [ProducesResponseType(typeof(List<HolidayModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByYear([FromQuery] int year)
        {
            return Ok(await _manager.GetByYearAsync(year));
        }

        [HttpGet("previous-year/{year}/{isChecked}")]
        [ProducesResponseType(typeof(List<HolidayModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPreviousYear([FromRoute] int year, [FromRoute] bool isChecked)
        {
            return Ok(await _manager.GetPreviousYearAsync(year, isChecked));
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
