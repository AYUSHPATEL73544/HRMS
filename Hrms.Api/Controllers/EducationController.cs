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

    public class EducationController : ControllerBase
    {
        private readonly IEducationManager _manager;
        public EducationController( IEducationManager educationManager)
        {
            _manager = educationManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] EducationModel model)
        {
            await _manager.AddAsync(model, User.GetUserId());
            return Ok();
        }

        [HttpGet("page-list")]
        [ProducesResponseType(typeof(MatTableResponse<EducationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListByUserId([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPageListAsync(model,User.GetUserId()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EducationModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _manager.GetByIdAsync(id));
        }

        [HttpGet("paged-list-by-employee-id/{id}")]
        [ProducesResponseType(typeof(MatTableResponse<EducationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListByEmployeeId([FromQuery] MatDataTableRequest model, [FromRoute] int id)
        {
            return Ok(await _manager.GetPageListByEmployeeIdAsync(model, id));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EducationModel model)
        {
            await _manager.UpdateAsync(model);
            return Ok();
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
