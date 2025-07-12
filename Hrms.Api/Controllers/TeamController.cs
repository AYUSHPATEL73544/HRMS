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
    public class TeamController: ControllerBase
    {
        private readonly ITeamManager _manager;
        public TeamController(ITeamManager teamManager)
        {
            _manager = teamManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] TeamModel model)
        {
            try
            {
                if (await _manager.IsManagerAssignedAsync(model.EmployeeId, model.ManagerId.Value))
                {
                    return BadRequest("Manager already assigned for this employee.");
                }

                await _manager.AddAsync(model);
                return Ok();
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<TeamModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEmployeeId([FromRoute] int id)
        {
            return Ok(await _manager.GetByEmployeeIdAsync(id));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(TeamModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _manager.GetByIdAsync(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TeamModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manager.GetByUserIdAsync(User.GetUserId()));
        }

        [HttpGet("get-reprotess-list")]
        [ProducesResponseType(typeof(List<TeamReportessModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportessList()
        {
            return Ok(await _manager.GetReportessListAsync(User.GetUserId()));
        }

        [HttpGet("get-by-manager/{id}")]
        [ProducesResponseType(typeof(List<TeamReportessModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByManagerId([FromRoute] int id)
        {
            return Ok(await _manager.GetByManagerIdAsync(id));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TeamModel model)
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
