using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Models.Reimbursement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class ReimbursementController : ControllerBase
    {
        private readonly IReimbursementManager _manager;
        private readonly ILogger<ReimbursementController> _logger;

        public ReimbursementController(IReimbursementManager manager, ILogger<ReimbursementController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] ReimbursementModel model)
        {
            await _manager.AddAsync(model, User.GetUserId());
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ReimbursementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListAsync([FromQuery] MatDataTableRequest model)
        {
            try
            {
                return Ok(await _manager.GetListAsync(model));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        } 

        [HttpGet("pending-list")]
        [ProducesResponseType(typeof(List<ReimbursementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPendingList([FromQuery] MatDataTableRequest model)
        {
            try
            {
                return Ok(await _manager.GetPendingListAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reimbursement-history/{id}")]
        [ProducesResponseType(typeof(List<ReimbursementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByEmployeeId([FromRoute] int id, [FromQuery] ReimbursementFilterModel model)
        {
            try
            {
                return Ok(await _manager.GetByEmployeeIdAsync(id, model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("page-list")]
        [ProducesResponseType(typeof(List<ReimbursementModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPageList([FromQuery] MatDataTableRequest model)
        {
            try
            {
                return Ok(await _manager.GetPageListAsync(User.GetUserId(), model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReimbursementModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _manager.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReimbursementModel model)
        { 
                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok(); 
        }

        [HttpPut("change-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeStatus([FromBody] ReimbursementChangeStatusModel model)
        {
            try
            {
                await _manager.ChangeStatusAsync(model, User.GetUserId());
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Change status.");
                return BadRequest(ex.Message);
            }
        }

    }
}
