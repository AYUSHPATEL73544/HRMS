using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsManager _manager;

        public DepartmentController(ILogger<DepartmentController> logger,
            IDepartmentsManager departmentManager)
        {
            _logger = logger;
            _manager = departmentManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] DepartmentModel model)
        {
            if (await _manager.IsExistsAsync(model.Name))
            {
                return BadRequest("Department already exist");
            }
            try
            {
                await _manager.AddAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Department");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetDetailAsync(id));
        }

        [HttpGet("get-select-list-item")]
        [ProducesResponseType(typeof(List<SelectListItemModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSelectListItem()
        {
            return Ok(await _manager.GetSelectListItemAsync());
        }

        [HttpGet("page-list")]
        [ProducesResponseType(typeof(MatTableResponse<DepartmentModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PageList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetPagedListAsync(model));
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(DepartmentModel model)
        {
            try
            {
                await _manager.UpdateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update designations");
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
