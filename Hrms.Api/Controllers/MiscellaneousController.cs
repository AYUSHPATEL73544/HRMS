using Hrms.Core.Abstractions.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    

    public class MiscellaneousController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IMiscellaneousManager _manager;
        public MiscellaneousController(ILogger<MiscellaneousController> logger,
            IMiscellaneousManager miscellaneousManager)
        {
            _logger = logger;
            _manager = miscellaneousManager;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update()
        {

            try
            {
                await _manager.UpdateLeavesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update LeaveLog");

                return BadRequest(ex.Message);
            }

        }

    }
}
