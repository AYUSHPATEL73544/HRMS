using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("interview")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class InterviewController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IInterviewManager _manager;

        public InterviewController(ILogger<JobApplicationController> logger, IInterviewManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] InterviewModel model)
        {
            try
            {
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Interview");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(MatTableResponse<InterviewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPagedListAsync(model, User.GetUserId()));
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet("list/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromRoute]int id)
        {
            return Ok(await _manager.GetListByCandidateIdAsync(id));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatTableResponse<ApplicantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetDetailAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InterviewModel model)
        {
            await _manager.UpdateAsync(model, User.GetUserId());
            return Ok();
        }

    }
}
