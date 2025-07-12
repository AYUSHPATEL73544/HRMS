using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]

    public class StateController : ControllerBase
    {
        private readonly IStateManager _manager;
        public StateController(IStateManager stateManager)
        {
            _manager = stateManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStateSelectListItems([FromQuery] int countryId)
        {
            return Ok(await _manager.GetSelectListItemsAsync(countryId));
        }
    }
}
