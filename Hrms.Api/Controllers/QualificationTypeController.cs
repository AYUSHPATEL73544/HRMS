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
    public class QualificationTypeController : ControllerBase
    {
        private readonly IQualificationTypeManager _manager;
        public QualificationTypeController(IQualificationTypeManager qualificationTypeManager)
        {
            _manager = qualificationTypeManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStateSelectListItems()
        {
            return Ok(await _manager.GetSelectListItemsAsync());
        }
    }
}
