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
    public class CourseTypeController: ControllerBase
    {
        private readonly ICourseTypeManager _manager;
        public CourseTypeController(ICourseTypeManager courseTypeManager)
        {
            _manager = courseTypeManager;
        }

        [HttpGet("select-list-items")]
        [ProducesResponseType(typeof(IEnumerable<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStateSelectListItems()
        {
            return Ok(await _manager.GetSelectListItemsAsync());
        }
    }
}
