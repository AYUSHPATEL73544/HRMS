using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class FamilyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFamilyManager _manager;
        public FamilyController(ILogger<FamilyController> logger,
            IFamilyManager familyManager)
        {
            _logger = logger;
            _manager = familyManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] FamilyModel model)
        {
            try
            {
                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Family");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FamilyModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult>Get()
        {
            return Ok(await _manager.GetAsync());
        }

        [HttpGet("list/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<FamilyModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEmployeeIdAsync([FromQuery] MatDataTableRequest model, [FromRoute] int id)
        {
            return Ok(await _manager.GetPagedListAsync(model,id));
        }

        [HttpGet("paged-list-by-user-id")]
        [ProducesResponseType(typeof(MatTableResponse<FamilyModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetByUserIdAsync(model,User.GetUserId()));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FamilyModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _manager.GetByIdAsync(id));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] FamilyModel model)
        {
            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Update");
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _manager.DeleteAsync(id);
            return Ok();
        }
    }
}
