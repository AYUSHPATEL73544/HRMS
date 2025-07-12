using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompanyManager _manager;

        public CompanyController(ILogger<CompanyController> logger,
            ICompanyManager companyManager)
        {
            _logger = logger;
            _manager = companyManager;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CompanyModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await _manager.GetAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Detail");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] CompanyModel model)
        {
            try
            {
                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update company");
                return BadRequest(ex.Message);
            }
        }
    }
}
