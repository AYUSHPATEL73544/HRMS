using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hrms.Core.Models.JobApplication;
using Microsoft.Extensions.Hosting.Internal;

namespace Hrms.Api.Controllers
{
    [Route("candidate")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class JobApplicationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IJobApplicationManager _manager;

        public JobApplicationController (ILogger<JobApplicationController> logger, 
            IJobApplicationManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] ApplicantModel model)
        {
            try
            {
                if(await _manager.IsExistsAsync(model.Email, model.Phone))
                {
                    return BadRequest("Candidate already exists");
                }
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Candidate");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetDetailsAsync(id));
        }

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetListAsync(model));
        }

        [HttpGet("get-shortlist-list")]
        [ProducesResponseType(typeof(MatTableResponse<ShortlistCandidateModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetShortlistPageList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetShortlistPageListAsync(model));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicantModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpPut("shortlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Shortlist([FromBody]int id)
        {
            try
            {
                await _manager.ShortlistAsync(id, User.GetUserId());
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Shorlist Candidate");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] ApplicantModel model)
        {
            try
            {
                if (await _manager.IsExistsAsync(model.Id, model.Email, model.Phone))
                {
                    return BadRequest("Candidate already exists");
                }

                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Candidate");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("change-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeStatus([FromBody] ApplicantChangeStatusModel model)
        {
            await _manager.ChangeStatusAsync(model, User.GetUserId());
            return Ok();
        }

        [HttpPut("hire")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Hire([FromBody] HireModel model)
        {
            try
            {
                await _manager.HireAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hire Candidate");
                return BadRequest(ex.Message);
            }
        }

    }

    #region private

    #endregion
}
