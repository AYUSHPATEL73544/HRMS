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
    public class RelationshipController: ControllerBase
    {
        private readonly IRelationshipManager _manager;
        public RelationshipController(IRelationshipManager relationshipManager)
        {
            _manager = relationshipManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStateSelectListItems()
        {
            return Ok(await _manager.GetSelectListItemsAsync());
        }
    }
}
