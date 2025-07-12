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
    public class VariantController : ControllerBase
    {
        private readonly IVariantManager _manager;
        public VariantController(IVariantManager variantManager)
        {
            _manager = variantManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetManufacturerListItems([FromQuery] int ManufacturerId)
        {
            return Ok(await _manager.GetSelectListItemsAsync(ManufacturerId));
        }
    }
}
