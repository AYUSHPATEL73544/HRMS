using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models.Assest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class AssetAllocationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAssetAllocationManager _manager;

        public AssetAllocationController(ILogger<AssetController> logger,
          IAssetAllocationManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AssetAllocationModel model)
        {
            try
            {
               if(await _manager.IsAssignedAsync(model.EmployeeId , model.AssetId))
                   {
                       return BadRequest("Asset is already assigned to this employee.");
                   }

                await _manager.AddAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Asset");
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("history-by-assetId/{id}")]
        [ProducesResponseType(typeof(List<AssetAllocationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromRoute] int id)
        {
            return Ok(await _manager.GetListAsync(id));
        }
    }
}
