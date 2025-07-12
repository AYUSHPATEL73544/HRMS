using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("skill")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class SkillController : ControllerBase
    {

        private readonly ISkillManager _manager;

        public SkillController( ISkillManager manager)
        {
            _manager = manager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSkillSelectListItems()
        {
            return Ok(await _manager.GetSkillListItemsAsync());
        }
    }
}
