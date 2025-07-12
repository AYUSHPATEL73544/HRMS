using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]

    public class UserRoleController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserRoleManager _manager;

        public UserRoleController(ILogger<UserRoleController> logger,
        IUserRoleManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] UserRoleModel model)
        {
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add User role");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpGet()]
        public async Task<IActionResult> GetRoleSelectListItems()
        {
            return Ok(await _manager.GetRoleSelectListItemsAsync());
        }
    }
}
