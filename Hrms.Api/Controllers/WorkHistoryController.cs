using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class WorkHistoryController : ControllerBase
    {
        private readonly IWorkHistoryManager _manager;
        public WorkHistoryController(IWorkHistoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet("get-by-employeeId/{employeeId}")]
        [ProducesResponseType(typeof(List<WorkHistoryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] int employeeId)
        {
            return Ok(await _manager.GetAsync(employeeId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkHistoryModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _manager.GetByIdAsync(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkHistoryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId()
        {
            return Ok(await _manager.GetByUserIdAsync(User.GetUserId()));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WorkHistoryModel model)
        {
            await _manager.UpdateAsync(model);
            return Ok();
        }
    }
}
