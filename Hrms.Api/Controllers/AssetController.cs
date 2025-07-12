using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Assest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]

    public class AssetController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAssetManager _manager;

        public AssetController(ILogger<AssetController> logger,
          IAssetManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet("get-list")]
        [ProducesResponseType(typeof(MatTableResponse<AssetModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] MatDataTableRequest model)
        {
            return Ok(await _manager.GetListAsync(model));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AssetModel model)
        {
            try
            {
                if (await _manager.IsSerialNumberExistsAsync(model.SerialNumber))
                {
                    return BadRequest("Serial Number already exists.");
                }

                await _manager.AddAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Asset");
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromRoute] int id)
        {
            return Ok(await _manager.GetAsync(id));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] AssetModel model)
        {
            try
            {
                await _manager.UpdateAsync(model, User.GetUserId());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Asset");
                return BadRequest(ex.Message);
            }
        }

    } 
}
