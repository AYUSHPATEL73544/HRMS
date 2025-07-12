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
    public class AssetTypeController : ControllerBase
    {
        private readonly IAssetTypeManager _manager;
        public AssetTypeController(IAssetTypeManager assetTypeManager)
        {
            _manager = assetTypeManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssetTypeListItems()
        {
            return Ok(await _manager.GetSelectListItemsAsync());
        }

    }
}
