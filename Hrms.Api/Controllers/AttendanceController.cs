using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AttendanceController: ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAttendanceManager _manager;

        public AttendanceController(ILogger<AttendanceController> logger,
            IAttendanceManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AttendanceModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Attendance");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AttendanceModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(List<AttendanceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _manager.GetListAsync());
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] AttendanceModel model)
        {
            try
            {
               await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Attendance");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }
    }
}
