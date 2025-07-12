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
    public class CityController : ControllerBase
    {
        private readonly ICityManager _manager;
        public CityController(ICityManager cityManager)
        {
            _manager = cityManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSelectListItems([FromQuery] int stateId)
        {
            return Ok(await _manager.GetSelectListItemsAsync(stateId));
        }
    }
}
