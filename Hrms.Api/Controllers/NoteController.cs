using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly INoteManager _manager;
        public NoteController(ILogger<NoteController> logger,
            INoteManager noteManager)
        {
            _logger = logger;
            _manager = noteManager;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] NoteModel model)
        {
            try
            {
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Note");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("note-list/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetList([FromRoute] int employeeId)
        {
            try
            {
                return Ok(await _manager.GetListAsync(employeeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await _manager.GetAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] NoteModel model)
        {
            try
            {
              await  _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update");
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }
    }
}
