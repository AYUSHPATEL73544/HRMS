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
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerManager _manager;
        public ManufacturerController(IManufacturerManager manufacturerManager)
        {
            _manager = manufacturerManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetManufacturerListItems([FromQuery]  int assetTypeId)
        {
            return Ok(await _manager.GetSelectListItemsAsync(assetTypeId));
        }

    }
}
